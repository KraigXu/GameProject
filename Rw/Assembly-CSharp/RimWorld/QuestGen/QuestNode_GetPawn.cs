using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001133 RID: 4403
	public class QuestNode_GetPawn : QuestNode
	{
		// Token: 0x060066E8 RID: 26344 RVA: 0x002405AC File Offset: 0x0023E7AC
		private IEnumerable<Pawn> ExistingUsablePawns(Slate slate)
		{
			return from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where this.IsGoodPawn(x, slate)
			select x;
		}

		// Token: 0x060066E9 RID: 26345 RVA: 0x002405E4 File Offset: 0x0023E7E4
		protected override bool TestRunInt(Slate slate)
		{
			if (this.mustHaveNoFaction.GetValue(slate) && this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate))
			{
				return false;
			}
			if (this.canGeneratePawn.GetValue(slate) && (this.mustBeFactionLeader.GetValue(slate) || this.mustBeWorldPawn.GetValue(slate) || this.mustBePlayerPrisoner.GetValue(slate) || this.mustBeFreeColonist.GetValue(slate)))
			{
				Log.Warning("QuestNode_GetPawn has incompatible flags set, when canGeneratePawn is true these flags cannot be set: mustBeFactionLeader, mustBeWorldPawn, mustBePlayerPrisoner, mustBeFreeColonist", false);
				return false;
			}
			Pawn pawn;
			if (slate.TryGet<Pawn>(this.storeAs.GetValue(slate), out pawn, false) && this.IsGoodPawn(pawn, slate))
			{
				return true;
			}
			IEnumerable<Pawn> source = this.ExistingUsablePawns(slate);
			if (source.Count<Pawn>() > 0)
			{
				slate.Set<Pawn>(this.storeAs.GetValue(slate), source.RandomElement<Pawn>(), false);
				return true;
			}
			if (!this.canGeneratePawn.GetValue(slate))
			{
				return false;
			}
			Faction faction;
			if (!this.mustHaveNoFaction.GetValue(slate) && !this.TryFindFactionForPawnGeneration(slate, out faction))
			{
				return false;
			}
			FloatRange senRange = this.seniorityRange.GetValue(slate);
			return !this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate) || !this.requireResearchedBedroomFurnitureIfRoyal.GetValue(slate) || DefDatabase<RoyalTitleDef>.AllDefsListForReading.Any((RoyalTitleDef x) => (senRange.max <= 0f || senRange.IncludesEpsilon((float)x.seniority)) && this.PlayerHasResearchedBedroomRequirementsFor(x));
		}

		// Token: 0x060066EA RID: 26346 RVA: 0x00240734 File Offset: 0x0023E934
		private bool TryFindFactionForPawnGeneration(Slate slate, out Faction faction)
		{
			return (from x in Find.FactionManager.GetFactions(false, false, false, TechLevel.Undefined)
			where (this.excludeFactionDefs.GetValue(slate) == null || !this.excludeFactionDefs.GetValue(slate).Contains(x.def)) && (!this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate) || x.def.HasRoyalTitles) && (!this.mustBeNonHostileToPlayer.GetValue(slate) || !x.HostileTo(Faction.OfPlayer)) && ((this.allowPermanentEnemyFaction.GetValue(slate) ?? false) || !x.def.permanentEnemy) && x.def.techLevel >= this.minTechLevel.GetValue(slate)
			select x).TryRandomElementByWeight(delegate(Faction x)
			{
				if (x.HostileTo(Faction.OfPlayer))
				{
					float? value = this.hostileWeight.GetValue(slate);
					if (value == null)
					{
						return 1f;
					}
					return value.GetValueOrDefault();
				}
				else
				{
					float? value = this.nonHostileWeight.GetValue(slate);
					if (value == null)
					{
						return 1f;
					}
					return value.GetValueOrDefault();
				}
			}, out faction);
		}

		// Token: 0x060066EB RID: 26347 RVA: 0x00240788 File Offset: 0x0023E988
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Pawn pawn;
			if (QuestGen.slate.TryGet<Pawn>(this.storeAs.GetValue(slate), out pawn, false) && this.IsGoodPawn(pawn, slate))
			{
				return;
			}
			IEnumerable<Pawn> source = this.ExistingUsablePawns(slate);
			int num = source.Count<Pawn>();
			Faction faction;
			if (Rand.Chance(this.canGeneratePawn.GetValue(slate) ? Mathf.Clamp01(1f - (float)num / (float)this.maxUsablePawnsToGenerate.GetValue(slate)) : 0f) && (this.mustHaveNoFaction.GetValue(slate) || this.TryFindFactionForPawnGeneration(slate, out faction)))
			{
				pawn = this.GeneratePawn(slate, null);
			}
			else
			{
				pawn = source.RandomElementByWeight(delegate(Pawn x)
				{
					if (x.Faction != null && x.Faction.HostileTo(Faction.OfPlayer))
					{
						float? value = this.hostileWeight.GetValue(slate);
						if (value == null)
						{
							return 1f;
						}
						return value.GetValueOrDefault();
					}
					else
					{
						float? value = this.nonHostileWeight.GetValue(slate);
						if (value == null)
						{
							return 1f;
						}
						return value.GetValueOrDefault();
					}
				});
			}
			if (pawn.Faction != null && !pawn.Faction.def.hidden)
			{
				QuestPart_InvolvedFactions questPart_InvolvedFactions = new QuestPart_InvolvedFactions();
				questPart_InvolvedFactions.factions.Add(pawn.Faction);
				QuestGen.quest.AddPart(questPart_InvolvedFactions);
			}
			QuestGen.slate.Set<Pawn>(this.storeAs.GetValue(slate), pawn, false);
		}

		// Token: 0x060066EC RID: 26348 RVA: 0x002408D4 File Offset: 0x0023EAD4
		private Pawn GeneratePawn(Slate slate, Faction faction = null)
		{
			PawnKindDef pawnKindDef = this.mustBeOfKind.GetValue(slate);
			if (faction == null && !this.mustHaveNoFaction.GetValue(slate))
			{
				if (!this.TryFindFactionForPawnGeneration(slate, out faction))
				{
					Log.Error("QuestNode_GetPawn tried generating pawn but couldn't find a proper faction for new pawn.", false);
				}
				else if (pawnKindDef == null)
				{
					pawnKindDef = faction.RandomPawnKind();
				}
			}
			RoyalTitleDef fixedTitle;
			if (this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate))
			{
				FloatRange senRange;
				if (!this.seniorityRange.TryGetValue(slate, out senRange))
				{
					senRange = FloatRange.Zero;
				}
				IEnumerable<RoyalTitleDef> source = from t in DefDatabase<RoyalTitleDef>.AllDefsListForReading
				where faction.def.RoyalTitlesAllInSeniorityOrderForReading.Contains(t) && (senRange.max <= 0f || senRange.IncludesEpsilon((float)t.seniority))
				select t;
				if (this.requireResearchedBedroomFurnitureIfRoyal.GetValue(slate) && source.Any((RoyalTitleDef x) => this.PlayerHasResearchedBedroomRequirementsFor(x)))
				{
					source = from x in source
					where this.PlayerHasResearchedBedroomRequirementsFor(x)
					select x;
				}
				fixedTitle = source.RandomElementByWeight((RoyalTitleDef t) => t.commonality);
				if (this.mustBeOfKind.GetValue(slate) == null && !(from k in DefDatabase<PawnKindDef>.AllDefsListForReading
				where k.titleRequired != null && k.titleRequired == fixedTitle
				select k).TryRandomElement(out pawnKindDef))
				{
					(from k in DefDatabase<PawnKindDef>.AllDefsListForReading
					where k.titleSelectOne != null && k.titleSelectOne.Contains(fixedTitle)
					select k).TryRandomElement(out pawnKindDef);
				}
			}
			else
			{
				fixedTitle = null;
			}
			if (pawnKindDef == null)
			{
				pawnKindDef = (from kind in DefDatabase<PawnKindDef>.AllDefsListForReading
				where kind.race.race.Humanlike
				select kind).RandomElement<PawnKindDef>();
			}
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKindDef, faction, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, fixedTitle));
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			if (pawn.royalty != null && pawn.royalty.AllTitlesForReading.Any<RoyalTitle>())
			{
				QuestPart_Hyperlinks questPart_Hyperlinks = new QuestPart_Hyperlinks();
				questPart_Hyperlinks.pawns.Add(pawn);
				QuestGen.quest.AddPart(questPart_Hyperlinks);
			}
			return pawn;
		}

		// Token: 0x060066ED RID: 26349 RVA: 0x00240B54 File Offset: 0x0023ED54
		private bool IsGoodPawn(Pawn pawn, Slate slate)
		{
			if (this.mustBeFactionLeader.GetValue(slate))
			{
				Faction faction = pawn.Faction;
				if (faction == null || faction.leader != pawn || !faction.def.humanlikeFaction || faction.defeated || faction.def.hidden || faction.IsPlayer || pawn.IsPrisoner)
				{
					return false;
				}
			}
			if (pawn.Faction != null && this.excludeFactionDefs.GetValue(slate) != null && this.excludeFactionDefs.GetValue(slate).Contains(pawn.Faction.def))
			{
				return false;
			}
			if (pawn.Faction != null && pawn.Faction.def.techLevel < this.minTechLevel.GetValue(slate))
			{
				return false;
			}
			if (this.mustBeOfKind.GetValue(slate) != null && pawn.kindDef != this.mustBeOfKind.GetValue(slate))
			{
				return false;
			}
			if (this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate) && (pawn.Faction == null || pawn.royalty == null || !pawn.royalty.HasAnyTitleIn(pawn.Faction)))
			{
				return false;
			}
			if (this.seniorityRange.GetValue(slate) != default(FloatRange) && (pawn.royalty == null || pawn.royalty.MostSeniorTitle == null || !this.seniorityRange.GetValue(slate).IncludesEpsilon((float)pawn.royalty.MostSeniorTitle.def.seniority)))
			{
				return false;
			}
			if (this.mustBeWorldPawn.GetValue(slate) && !pawn.IsWorldPawn())
			{
				return false;
			}
			if (this.ifWorldPawnThenMustBeFree.GetValue(slate) && pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.Free)
			{
				return false;
			}
			if (this.ifWorldPawnThenMustBeFreeOrLeader.GetValue(slate) && pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.Free && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.FactionLeader)
			{
				return false;
			}
			if (pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) == WorldPawnSituation.ReservedByQuest)
			{
				return false;
			}
			if (this.mustHaveNoFaction.GetValue(slate) && pawn.Faction != null)
			{
				return false;
			}
			if (this.mustBeFreeColonist.GetValue(slate) && !pawn.IsFreeColonist)
			{
				return false;
			}
			if (this.mustBePlayerPrisoner.GetValue(slate) && !pawn.IsPrisonerOfColony)
			{
				return false;
			}
			if (this.mustBeNotSuspended.GetValue(slate) && pawn.Suspended)
			{
				return false;
			}
			if (this.mustBeNonHostileToPlayer.GetValue(slate) && (pawn.HostileTo(Faction.OfPlayer) || (pawn.Faction != null && pawn.Faction != Faction.OfPlayer && pawn.Faction.HostileTo(Faction.OfPlayer))))
			{
				return false;
			}
			if (!(this.allowPermanentEnemyFaction.GetValue(slate) ?? true) && pawn.Faction != null && pawn.Faction.def.permanentEnemy)
			{
				return false;
			}
			if (this.requireResearchedBedroomFurnitureIfRoyal.GetValue(slate))
			{
				RoyalTitle royalTitle = pawn.royalty.HighestTitleWithBedroomRequirements();
				if (royalTitle != null && !this.PlayerHasResearchedBedroomRequirementsFor(royalTitle.def))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060066EE RID: 26350 RVA: 0x00240E68 File Offset: 0x0023F068
		private bool PlayerHasResearchedBedroomRequirementsFor(RoyalTitleDef title)
		{
			if (title.bedroomRequirements == null)
			{
				return true;
			}
			for (int i = 0; i < title.bedroomRequirements.Count; i++)
			{
				if (!title.bedroomRequirements[i].PlayerHasResearched())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04003F01 RID: 16129
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003F02 RID: 16130
		public SlateRef<bool> mustBeFactionLeader;

		// Token: 0x04003F03 RID: 16131
		public SlateRef<bool> mustBeWorldPawn;

		// Token: 0x04003F04 RID: 16132
		public SlateRef<bool> ifWorldPawnThenMustBeFree;

		// Token: 0x04003F05 RID: 16133
		public SlateRef<bool> ifWorldPawnThenMustBeFreeOrLeader;

		// Token: 0x04003F06 RID: 16134
		public SlateRef<bool> mustHaveNoFaction;

		// Token: 0x04003F07 RID: 16135
		public SlateRef<bool> mustBeFreeColonist;

		// Token: 0x04003F08 RID: 16136
		public SlateRef<bool> mustBePlayerPrisoner;

		// Token: 0x04003F09 RID: 16137
		public SlateRef<bool> mustBeNotSuspended;

		// Token: 0x04003F0A RID: 16138
		public SlateRef<bool> mustHaveRoyalTitleInCurrentFaction;

		// Token: 0x04003F0B RID: 16139
		public SlateRef<bool> mustBeNonHostileToPlayer;

		// Token: 0x04003F0C RID: 16140
		public SlateRef<bool?> allowPermanentEnemyFaction;

		// Token: 0x04003F0D RID: 16141
		public SlateRef<bool> canGeneratePawn;

		// Token: 0x04003F0E RID: 16142
		public SlateRef<bool> requireResearchedBedroomFurnitureIfRoyal;

		// Token: 0x04003F0F RID: 16143
		public SlateRef<PawnKindDef> mustBeOfKind;

		// Token: 0x04003F10 RID: 16144
		public SlateRef<FloatRange> seniorityRange;

		// Token: 0x04003F11 RID: 16145
		public SlateRef<TechLevel> minTechLevel;

		// Token: 0x04003F12 RID: 16146
		public SlateRef<List<FactionDef>> excludeFactionDefs;

		// Token: 0x04003F13 RID: 16147
		public SlateRef<float?> hostileWeight;

		// Token: 0x04003F14 RID: 16148
		public SlateRef<float?> nonHostileWeight;

		// Token: 0x04003F15 RID: 16149
		public SlateRef<int> maxUsablePawnsToGenerate = 10;
	}
}
