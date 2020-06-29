using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetPawn : QuestNode
	{
		
		private IEnumerable<Pawn> ExistingUsablePawns(Slate slate)
		{
			return from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where this.IsGoodPawn(x, slate)
			select x;
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<bool> mustBeFactionLeader;

		
		public SlateRef<bool> mustBeWorldPawn;

		
		public SlateRef<bool> ifWorldPawnThenMustBeFree;

		
		public SlateRef<bool> ifWorldPawnThenMustBeFreeOrLeader;

		
		public SlateRef<bool> mustHaveNoFaction;

		
		public SlateRef<bool> mustBeFreeColonist;

		
		public SlateRef<bool> mustBePlayerPrisoner;

		
		public SlateRef<bool> mustBeNotSuspended;

		
		public SlateRef<bool> mustHaveRoyalTitleInCurrentFaction;

		
		public SlateRef<bool> mustBeNonHostileToPlayer;

		
		public SlateRef<bool?> allowPermanentEnemyFaction;

		
		public SlateRef<bool> canGeneratePawn;

		
		public SlateRef<bool> requireResearchedBedroomFurnitureIfRoyal;

		
		public SlateRef<PawnKindDef> mustBeOfKind;

		
		public SlateRef<FloatRange> seniorityRange;

		
		public SlateRef<TechLevel> minTechLevel;

		
		public SlateRef<List<FactionDef>> excludeFactionDefs;

		
		public SlateRef<float?> hostileWeight;

		
		public SlateRef<float?> nonHostileWeight;

		
		public SlateRef<int> maxUsablePawnsToGenerate = 10;
	}
}
