using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Spirit;

namespace Heroic
{

	public class Faction : IExposable, ILoadReferenceable, ICommunicable
	{

		public FactionDef def;

		private string name;

		public int loadID = -1;


		public int randomKey;


		public float colorFromSpectrum = -999f;


		public float centralMelanin = 0.5f;

		private List<FactionRelation> relations = new List<FactionRelation>();


		public Pawn leader;


		public KidnappedPawnsTracker kidnapped;


		private List<PredatorThreat> predatorThreats = new List<PredatorThreat>();


		public bool defeated;

		public int lastTraderRequestTick = -9999999;

	
		public int lastMilitaryAidRequestTick = -9999999;


		private int naturalGoodwillTimer;


		public bool allowRoyalFavorRewards = true;


		public bool allowGoodwillRewards = true;


		public List<string> questTags;

		// Token: 0x0400298A RID: 10634
		private List<Map> avoidGridsBasicKeysWorkingList;

		// Token: 0x0400298B RID: 10635
		private List<ByteGrid> avoidGridsBasicValuesWorkingList;

		// Token: 0x0400298C RID: 10636
		private List<Map> avoidGridsSmartKeysWorkingList;

		// Token: 0x0400298D RID: 10637
		private List<ByteGrid> avoidGridsSmartValuesWorkingList;

		// Token: 0x0400298E RID: 10638
		private static List<PawnKindDef> allPawnKinds = new List<PawnKindDef>();

		// Token: 0x0400298F RID: 10639
		public const int RoyalThingUseViolationGoodwillImpact = 4;


		public string Name
		{
			get
			{
				if (this.HasName)
				{
					return this.name;
				}
				return this.def.LabelCap;
			}
			set
			{
				this.name = value;
			}
		}

	
		public bool HasName
		{
			get
			{
				return this.name != null;
			}
		}

		public bool IsPlayer
		{
			get
			{
				return this.def.isPlayer;
			}
		}

		public int PlayerGoodwill
		{
			get
			{
				return this.GoodwillWith(Faction.OfPlayer);
			}
		}

		public FactionRelationKind PlayerRelationKind
		{
			get
			{
				return this.RelationKindWith(Faction.OfPlayer);
			}
		}


		public Color Color
		{
			get
			{
				if (this.def.colorSpectrum.NullOrEmpty<Color>())
				{
					return Color.white;
				}
				return ColorsFromSpectrum.Get(this.def.colorSpectrum, this.colorFromSpectrum);
			}
		}

		public string LeaderTitle
		{
			get
			{
				if (this.leader != null && this.leader.gender == Gender.Female && !string.IsNullOrEmpty(this.def.leaderTitleFemale))
				{
					return this.def.leaderTitleFemale;
				}
				return this.def.leaderTitle;
			}
		}

		public TaggedString NameColored
		{
			get
			{
				if (this.HasName)
				{
					return this.name.ApplyTag(this);
				}
				return this.def.LabelCap;
			}
		}

	
		[Obsolete]
		public bool CanGiveGoodwillRewards
		{
			get
			{
				return this.CanEverGiveGoodwillRewards;
			}
		}

		public bool CanEverGiveGoodwillRewards
		{
			get
			{
				return !this.def.permanentEnemy;
			}
		}

		
		public string GetReportText
		{
			get
			{
				return this.def.description + (this.def.HasRoyalTitles ? ("\n\n" + RoyalTitleUtility.GetTitleProgressionInfo(this, null)) : "");
			}
		}

		// Token: 0x0600483D RID: 18493 RVA: 0x0018718C File Offset: 0x0018538C
		public Faction()
		{
			this.randomKey = Rand.Range(0, int.MaxValue);
			this.kidnapped = new KidnappedPawnsTracker(this);
		}


		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.leader, "leader", false);
			Scribe_Defs.Look<FactionDef>(ref this.def, "def");
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.randomKey, "randomKey", 0, false);
			Scribe_Values.Look<float>(ref this.colorFromSpectrum, "colorFromSpectrum", 0f, false);
			Scribe_Values.Look<float>(ref this.centralMelanin, "centralMelanin", 0f, false);
			Scribe_Collections.Look<FactionRelation>(ref this.relations, "relations", LookMode.Deep, Array.Empty<object>());
			Scribe_Deep.Look<KidnappedPawnsTracker>(ref this.kidnapped, "kidnapped", new object[]
			{
				this
			});
			Scribe_Collections.Look<PredatorThreat>(ref this.predatorThreats, "predatorThreats", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.defeated, "defeated", false, false);
			Scribe_Values.Look<int>(ref this.lastTraderRequestTick, "lastTraderRequestTick", -9999999, false);
			Scribe_Values.Look<int>(ref this.lastMilitaryAidRequestTick, "lastMilitaryAidRequestTick", -9999999, false);
			Scribe_Values.Look<int>(ref this.naturalGoodwillTimer, "naturalGoodwillTimer", 0, false);
			Scribe_Values.Look<bool>(ref this.allowRoyalFavorRewards, "allowRoyalFavorRewards", true, false);
			Scribe_Values.Look<bool>(ref this.allowGoodwillRewards, "allowGoodwillRewards", true, false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.predatorThreats.RemoveAll((PredatorThreat x) => x.predator == null);
			}
		}

		// Token: 0x0600483F RID: 18495 RVA: 0x001873A8 File Offset: 0x001855A8
		public void FactionTick()
		{
			this.CheckNaturalTendencyToReachGoodwillThreshold();
			this.kidnapped.KidnappedPawnsTrackerTick();
			for (int i = this.predatorThreats.Count - 1; i >= 0; i--)
			{
				PredatorThreat predatorThreat = this.predatorThreats[i];
				if (predatorThreat.Expired)
				{
					this.predatorThreats.RemoveAt(i);
					if (predatorThreat.predator.Spawned)
					{
						predatorThreat.predator.Map.attackTargetsCache.UpdateTarget(predatorThreat.predator);
					}
				}
			}
			if (Find.TickManager.TicksGame % 1000 == 200 && this.IsPlayer)
			{
				if (NamePlayerFactionAndSettlementUtility.CanNameFactionNow())
				{
					Settlement settlement = Find.WorldObjects.Settlements.Find((Settlement x) => NamePlayerFactionAndSettlementUtility.CanNameSettlementSoon(x));
					if (settlement != null)
					{
						Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(settlement));
						return;
					}
					Find.WindowStack.Add(new Dialog_NamePlayerFaction());
					return;
				}
				else
				{
					Settlement settlement2 = Find.WorldObjects.Settlements.Find((Settlement x) => NamePlayerFactionAndSettlementUtility.CanNameSettlementNow(x));
					if (settlement2 != null)
					{
						if (NamePlayerFactionAndSettlementUtility.CanNameFactionSoon())
						{
							Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(settlement2));
							return;
						}
						Find.WindowStack.Add(new Dialog_NamePlayerSettlement(settlement2));
					}
				}
			}
		}

		// Token: 0x06004840 RID: 18496 RVA: 0x00187500 File Offset: 0x00185700
		private void CheckNaturalTendencyToReachGoodwillThreshold()
		{
			if (this.IsPlayer)
			{
				return;
			}
			int playerGoodwill = this.PlayerGoodwill;
			if (this.def.naturalColonyGoodwill.Includes(playerGoodwill))
			{
				this.naturalGoodwillTimer = 0;
				return;
			}
			this.naturalGoodwillTimer++;
			if (playerGoodwill < this.def.naturalColonyGoodwill.min)
			{
				if (this.def.goodwillDailyGain != 0f)
				{
					int num = (int)(10f / this.def.goodwillDailyGain * 60000f);
					if (this.naturalGoodwillTimer >= num)
					{
						this.TryAffectGoodwillWith(Faction.OfPlayer, Mathf.Min(10, this.def.naturalColonyGoodwill.min - playerGoodwill), true, true, "GoodwillChangedReason_NaturallyOverTime".Translate(this.def.naturalColonyGoodwill.min.ToString()), null);
						this.naturalGoodwillTimer = 0;
						return;
					}
				}
			}
			else if (playerGoodwill > this.def.naturalColonyGoodwill.max && this.def.goodwillDailyFall != 0f)
			{
				int num2 = (int)(10f / this.def.goodwillDailyFall * 60000f);
				if (this.naturalGoodwillTimer >= num2)
				{
					this.TryAffectGoodwillWith(Faction.OfPlayer, -Mathf.Min(10, playerGoodwill - this.def.naturalColonyGoodwill.max), true, true, "GoodwillChangedReason_NaturallyOverTime".Translate(this.def.naturalColonyGoodwill.max.ToString()), null);
					this.naturalGoodwillTimer = 0;
				}
			}
		}

		// Token: 0x06004841 RID: 18497 RVA: 0x001876A8 File Offset: 0x001858A8
		public void TryMakeInitialRelationsWith(Faction other)
		{
			if (this.RelationWith(other, true) != null)
			{
				return;
			}
			int a = this.def.permanentEnemy ? -100 : this.def.startingGoodwill.RandomInRange;
			if (this.IsPlayer)
			{
				a = 100;
			}
			if (this.def.permanentEnemyToEveryoneExceptPlayer && !other.IsPlayer)
			{
				a = -100;
			}
			int b = other.def.permanentEnemy ? -100 : other.def.startingGoodwill.RandomInRange;
			if (other.IsPlayer)
			{
				b = 100;
			}
			if (other.def.permanentEnemyToEveryoneExceptPlayer && !this.IsPlayer)
			{
				b = -100;
			}
			int num = Mathf.Min(a, b);
			FactionRelationKind kind;
			if (num <= -10)
			{
				kind = FactionRelationKind.Hostile;
			}
			else if (num >= 75)
			{
				kind = FactionRelationKind.Ally;
			}
			else
			{
				kind = FactionRelationKind.Neutral;
			}
			FactionRelation factionRelation = new FactionRelation();
			factionRelation.other = other;
			factionRelation.goodwill = num;
			factionRelation.kind = kind;
			this.relations.Add(factionRelation);
			FactionRelation factionRelation2 = new FactionRelation();
			factionRelation2.other = this;
			factionRelation2.goodwill = num;
			factionRelation2.kind = kind;
			other.relations.Add(factionRelation2);
		}

		// Token: 0x06004842 RID: 18498 RVA: 0x001877C0 File Offset: 0x001859C0
		public PawnKindDef RandomPawnKind()
		{
			Faction.allPawnKinds.Clear();
			if (this.def.pawnGroupMakers != null)
			{
				for (int i = 0; i < this.def.pawnGroupMakers.Count; i++)
				{
					List<PawnGenOption> options = this.def.pawnGroupMakers[i].options;
					for (int j = 0; j < options.Count; j++)
					{
						Faction.allPawnKinds.Add(options[j].kind);
					}
				}
			}
			if (!Faction.allPawnKinds.Any<PawnKindDef>())
			{
				return this.def.basicMemberKind;
			}
			PawnKindDef result = Faction.allPawnKinds.RandomElement<PawnKindDef>();
			Faction.allPawnKinds.Clear();
			return result;
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x0018786C File Offset: 0x00185A6C
		public FactionRelation RelationWith(Faction other, bool allowNull = false)
		{
			if (other == this)
			{
				Log.Error("Tried to get relation between faction " + this + " and itself.", false);
				return new FactionRelation();
			}
			for (int i = 0; i < this.relations.Count; i++)
			{
				if (this.relations[i].other == other)
				{
					return this.relations[i];
				}
			}
			if (!allowNull)
			{
				Log.Error(string.Concat(new object[]
				{
					"Faction ",
					this.name,
					" has null relation with ",
					other,
					". Returning dummy relation."
				}), false);
				return new FactionRelation();
			}
			return null;
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x00187910 File Offset: 0x00185B10
		public int GoodwillWith(Faction other)
		{
			return this.RelationWith(other, false).goodwill;
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x0018791F File Offset: 0x00185B1F
		public FactionRelationKind RelationKindWith(Faction other)
		{
			return this.RelationWith(other, false).kind;
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x00187930 File Offset: 0x00185B30
		public bool CanChangeGoodwillFor(Faction other, int goodwillChange)
		{
			return !this.def.hidden && !other.def.hidden && !this.def.permanentEnemy && !other.def.permanentEnemy && !this.defeated && !other.defeated && other != this && (!this.def.permanentEnemyToEveryoneExceptPlayer || other.IsPlayer) && (!other.def.permanentEnemyToEveryoneExceptPlayer || this.IsPlayer) && (goodwillChange <= 0 || ((!this.IsPlayer || !SettlementUtility.IsPlayerAttackingAnySettlementOf(other)) && (!other.IsPlayer || !SettlementUtility.IsPlayerAttackingAnySettlementOf(this))));
		}

		// Token: 0x06004847 RID: 18503 RVA: 0x001879D8 File Offset: 0x00185BD8
		public bool TryAffectGoodwillWith(Faction other, int goodwillChange, bool canSendMessage = true, bool canSendHostilityLetter = true, string reason = null, GlobalTargetInfo? lookTarget = null)
		{
			if (!this.CanChangeGoodwillFor(other, goodwillChange))
			{
				return false;
			}
			if (goodwillChange == 0)
			{
				return true;
			}
			int num = this.GoodwillWith(other);
			int num2 = Mathf.Clamp(num + goodwillChange, -100, 100);
			if (num == num2)
			{
				return true;
			}
			FactionRelation factionRelation = this.RelationWith(other, false);
			factionRelation.goodwill = num2;
			bool flag;
			factionRelation.CheckKindThresholds(this, canSendHostilityLetter, reason, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
			FactionRelation factionRelation2 = other.RelationWith(this, false);
			FactionRelationKind kind = factionRelation2.kind;
			factionRelation2.goodwill = factionRelation.goodwill;
			factionRelation2.kind = factionRelation.kind;
			bool flag2;
			if (kind != factionRelation2.kind)
			{
				other.Notify_RelationKindChanged(this, kind, canSendHostilityLetter, reason, lookTarget ?? GlobalTargetInfo.Invalid, out flag2);
			}
			else
			{
				flag2 = false;
			}
			if (canSendMessage && !flag && !flag2 && Current.ProgramState == ProgramState.Playing && (this.IsPlayer || other.IsPlayer))
			{
				Faction faction = this.IsPlayer ? other : this;
				string text;
				if (!reason.NullOrEmpty())
				{
					text = "MessageGoodwillChangedWithReason".Translate(faction.name, num.ToString("F0"), factionRelation.goodwill.ToString("F0"), reason);
				}
				else
				{
					text = "MessageGoodwillChanged".Translate(faction.name, num.ToString("F0"), factionRelation.goodwill.ToString("F0"));
				}
				Messages.Message(text, lookTarget ?? GlobalTargetInfo.Invalid, ((float)goodwillChange > 0f) ? MessageTypeDefOf.PositiveEvent : MessageTypeDefOf.NegativeEvent, true);
			}
			return true;
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x00187BCA File Offset: 0x00185DCA
		public bool TrySetNotHostileTo(Faction other, bool canSendLetter = true, string reason = null, GlobalTargetInfo? lookTarget = null)
		{
			if (this.RelationKindWith(other) == FactionRelationKind.Hostile)
			{
				this.TrySetRelationKind(other, FactionRelationKind.Neutral, canSendLetter, reason, lookTarget);
			}
			return this.RelationKindWith(other) > FactionRelationKind.Hostile;
		}

		// Token: 0x06004849 RID: 18505 RVA: 0x00187BEC File Offset: 0x00185DEC
		public bool TrySetNotAlly(Faction other, bool canSendLetter = true, string reason = null, GlobalTargetInfo? lookTarget = null)
		{
			if (this.RelationKindWith(other) == FactionRelationKind.Ally)
			{
				this.TrySetRelationKind(other, FactionRelationKind.Neutral, canSendLetter, reason, lookTarget);
			}
			return this.RelationKindWith(other) != FactionRelationKind.Ally;
		}

		// Token: 0x0600484A RID: 18506 RVA: 0x00187C14 File Offset: 0x00185E14
		public bool TrySetRelationKind(Faction other, FactionRelationKind kind, bool canSendLetter = true, string reason = null, GlobalTargetInfo? lookTarget = null)
		{
			FactionRelation factionRelation = this.RelationWith(other, false);
			if (factionRelation.kind == kind)
			{
				return true;
			}
			switch (kind)
			{
				case FactionRelationKind.Hostile:
					this.TryAffectGoodwillWith(other, -75 - factionRelation.goodwill, false, canSendLetter, reason, lookTarget);
					return factionRelation.kind == FactionRelationKind.Hostile;
				case FactionRelationKind.Neutral:
					this.TryAffectGoodwillWith(other, 0 - factionRelation.goodwill, false, canSendLetter, reason, lookTarget);
					return factionRelation.kind == FactionRelationKind.Neutral;
				case FactionRelationKind.Ally:
					this.TryAffectGoodwillWith(other, 75 - factionRelation.goodwill, false, canSendLetter, reason, lookTarget);
					return factionRelation.kind == FactionRelationKind.Ally;
				default:
					throw new NotSupportedException(kind.ToString());
			}
		}

		// Token: 0x0600484B RID: 18507 RVA: 0x00187CC0 File Offset: 0x00185EC0
		public void RemoveAllRelations()
		{
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				if (faction != this)
				{
					faction.relations.RemoveAll((FactionRelation x) => x.other == this);
				}
			}
			this.relations.Clear();
		}

		// Token: 0x0600484C RID: 18508 RVA: 0x00187D38 File Offset: 0x00185F38
		public void TryAppendRelationKindChangedInfo(StringBuilder text, FactionRelationKind previousKind, FactionRelationKind newKind, string reason = null)
		{
			TaggedString taggedString = null;
			this.TryAppendRelationKindChangedInfo(ref taggedString, previousKind, newKind, reason);
			if (!taggedString.NullOrEmpty())
			{
				text.AppendLine();
				text.AppendLine();
				text.AppendTagged(taggedString);
			}
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x00187D78 File Offset: 0x00185F78
		public void TryAppendRelationKindChangedInfo(ref TaggedString text, FactionRelationKind previousKind, FactionRelationKind newKind, string reason = null)
		{
			if (previousKind == newKind)
			{
				return;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n\n";
			}
			if (newKind == FactionRelationKind.Hostile)
			{
				text += "LetterRelationsChange_Hostile".Translate(this.NameColored, this.PlayerGoodwill.ToStringWithSign(), -75.ToStringWithSign(), 0.ToStringWithSign());
				if (!reason.NullOrEmpty())
				{
					text += "\n\n" + "FinalStraw".Translate(reason.CapitalizeFirst());
					return;
				}
			}
			else if (newKind == FactionRelationKind.Ally)
			{
				text += "LetterRelationsChange_Ally".Translate(this.NameColored, this.PlayerGoodwill.ToStringWithSign(), 75.ToStringWithSign(), 0.ToStringWithSign());
				if (!reason.NullOrEmpty())
				{
					text += "\n\n" + "LastFactionRelationsEvent".Translate() + ": " + reason.CapitalizeFirst();
					return;
				}
			}
			else if (newKind == FactionRelationKind.Neutral)
			{
				if (previousKind == FactionRelationKind.Hostile)
				{
					text += "LetterRelationsChange_NeutralFromHostile".Translate(this.NameColored, this.PlayerGoodwill.ToStringWithSign(), 0.ToStringWithSign(), -75.ToStringWithSign(), 75.ToStringWithSign());
					if (!reason.NullOrEmpty())
					{
						text += "\n\n" + "LastFactionRelationsEvent".Translate() + ": " + reason.CapitalizeFirst();
						return;
					}
				}
				else
				{
					text += "LetterRelationsChange_NeutralFromAlly".Translate(this.NameColored, this.PlayerGoodwill.ToStringWithSign(), 0.ToStringWithSign(), -75.ToStringWithSign(), 75.ToStringWithSign());
					if (!reason.NullOrEmpty())
					{
						text += "\n\n" + "Reason".Translate() + ": " + reason.CapitalizeFirst();
					}
				}
			}
		}

		// Token: 0x0600484E RID: 18510 RVA: 0x00188020 File Offset: 0x00186220
		public void Notify_MemberTookDamage(Pawn member, DamageInfo dinfo)
		{
			if (dinfo.Instigator == null)
			{
				return;
			}
			if (this.IsPlayer)
			{
				return;
			}
			Pawn pawn = dinfo.Instigator as Pawn;
			if (pawn != null && pawn.CurJob != null && pawn.CurJob.def == JobDefOf.PredatorHunt)
			{
				this.TookDamageFromPredator(pawn);
			}
			if (dinfo.Instigator.Faction == null || !dinfo.Def.ExternalViolenceFor(member) || this.HostileTo(dinfo.Instigator.Faction))
			{
				return;
			}
			if (member.InAggroMentalState)
			{
				return;
			}
			if (pawn != null && pawn.InAggroMentalState)
			{
				return;
			}
			if (member.InMentalState && member.MentalStateDef.IsExtreme && member.MentalStateDef.category == MentalStateCategory.Malicious && this.PlayerRelationKind == FactionRelationKind.Ally)
			{
				return;
			}
			if (dinfo.Instigator.Faction == Faction.OfPlayer && (PrisonBreakUtility.IsPrisonBreaking(member) || member.IsQuestHelper()))
			{
				return;
			}
			if (dinfo.Instigator.Faction == Faction.OfPlayer && !this.IsMutuallyHostileCrossfire(dinfo))
			{
				float num = Mathf.Min(100f, dinfo.Amount);
				int goodwillChange = (int)(-1.3f * num);
				this.TryAffectGoodwillWith(dinfo.Instigator.Faction, goodwillChange, true, true, "GoodwillChangedReason_AttackedPawn".Translate(member.LabelShort, member), new GlobalTargetInfo?(member));
			}
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x00188184 File Offset: 0x00186384
		public void Notify_BuildingTookDamage(Building building, DamageInfo dinfo)
		{
			if (dinfo.Instigator == null || this.IsPlayer)
			{
				return;
			}
			if (dinfo.Instigator.Faction == null || !dinfo.Def.ExternalViolenceFor(building) || this.HostileTo(dinfo.Instigator.Faction))
			{
				return;
			}
			if (dinfo.Instigator.Faction == Faction.OfPlayer && !this.IsMutuallyHostileCrossfire(dinfo))
			{
				float num = Mathf.Min(100f, dinfo.Amount);
				int goodwillChange = (int)(-1f * num);
				this.TryAffectGoodwillWith(dinfo.Instigator.Faction, goodwillChange, true, true, "GoodwillChangedReason_AttackedPawn".Translate(building.LabelShort, building), new GlobalTargetInfo?(building));
			}
		}

		// Token: 0x06004850 RID: 18512 RVA: 0x00188250 File Offset: 0x00186450
		public void Notify_MemberCaptured(Pawn member, Faction violator)
		{
			if (violator == this)
			{
				return;
			}
			if (this.RelationKindWith(violator) != FactionRelationKind.Hostile)
			{
				this.TrySetRelationKind(violator, FactionRelationKind.Hostile, true, "GoodwillChangedReason_CapturedPawn".Translate(member.LabelShort, member), new GlobalTargetInfo?(member));
			}
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x001882A0 File Offset: 0x001864A0
		public void Notify_MemberStripped(Pawn member, Faction violator)
		{
			if (violator == this || this.def.hidden || member.Dead)
			{
				return;
			}
			if (violator == Faction.OfPlayer && this.RelationKindWith(violator) != FactionRelationKind.Hostile)
			{
				this.TryAffectGoodwillWith(Faction.OfPlayer, -10, true, true, "GoodwillChangedReason_PawnStripped".Translate(member), new GlobalTargetInfo?(member));
			}
		}

		// Token: 0x06004852 RID: 18514 RVA: 0x00188308 File Offset: 0x00186508
		public void Notify_MemberDied(Pawn member, DamageInfo? dinfo, bool wasWorldPawn, Map map)
		{
			if (this.IsPlayer)
			{
				return;
			}
			if (!wasWorldPawn && !PawnGenerator.IsBeingGenerated(member) && Current.ProgramState == ProgramState.Playing && map != null && map.IsPlayerHome && !this.HostileTo(Faction.OfPlayer))
			{
				if (dinfo != null && dinfo.Value.Category == DamageInfo.SourceCategory.Collapse)
				{
					bool canSendMessage = MessagesRepeatAvoider.MessageShowAllowed("FactionRelationAdjustmentCrushed-" + this.Name, 5f);
					this.TryAffectGoodwillWith(Faction.OfPlayer, member.RaceProps.Humanlike ? -25 : -15, canSendMessage, true, "GoodwillChangedReason_PawnCrushed".Translate(member.LabelShort, member), new GlobalTargetInfo?(new TargetInfo(member.Position, map, false)));
				}
				else if (dinfo != null && (dinfo.Value.Instigator == null || dinfo.Value.Instigator.Faction == null))
				{
					Pawn pawn = dinfo.Value.Instigator as Pawn;
					if (pawn == null || !pawn.RaceProps.Animal || pawn.mindState.mentalStateHandler.CurStateDef != MentalStateDefOf.ManhunterPermanent)
					{
						this.TryAffectGoodwillWith(Faction.OfPlayer, member.RaceProps.Humanlike ? -5 : -3, true, true, "GoodwillChangedReason_PawnDied".Translate(member.LabelShort, member), new GlobalTargetInfo?(member));
					}
				}
			}
			if (member == this.leader)
			{
				this.Notify_LeaderDied();
			}
		}

		// Token: 0x06004853 RID: 18515 RVA: 0x001884C4 File Offset: 0x001866C4
		public void Notify_LeaderDied()
		{
			Pawn pawn = this.leader;
			this.GenerateNewLeader();
			Find.LetterStack.ReceiveLetter("LetterLeadersDeathLabel".Translate(this.name, this.LeaderTitle).CapitalizeFirst(), "LetterLeadersDeath".Translate(pawn.Name.ToStringFull, this.NameColored, this.leader.Name.ToStringFull, this.LeaderTitle, pawn.Named("OLDLEADER"), this.leader.Named("NEWLEADER")).CapitalizeFirst(), LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, this, null, null, null);
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "NoLongerFactionLeader", pawn.Named("SUBJECT"), this.leader.Named("NEWFACTIONLEADER"));
		}

		// Token: 0x06004854 RID: 18516 RVA: 0x001885B8 File Offset: 0x001867B8
		public void Notify_LeaderLost()
		{
			Pawn pawn = this.leader;
			this.GenerateNewLeader();
			Find.LetterStack.ReceiveLetter("LetterLeaderChangedLabel".Translate(this.name, this.LeaderTitle).CapitalizeFirst(), "LetterLeaderChanged".Translate(pawn.Name.ToStringFull, this.NameColored, this.leader.Name.ToStringFull, this.LeaderTitle, pawn.Named("OLDLEADER"), this.leader.Named("NEWLEADER")).CapitalizeFirst(), LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, this, null, null, null);
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "NoLongerFactionLeader", pawn.Named("SUBJECT"), this.leader.Named("NEWFACTIONLEADER"));
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x001886AC File Offset: 0x001868AC
		public void Notify_RelationKindChanged(Faction other, FactionRelationKind previousKind, bool canSendLetter, string reason, GlobalTargetInfo lookTarget, out bool sentLetter)
		{
			if (Current.ProgramState != ProgramState.Playing || other != Faction.OfPlayer)
			{
				canSendLetter = false;
			}
			sentLetter = false;
			ColoredText.ClearCache();
			FactionRelationKind factionRelationKind = this.RelationKindWith(other);
			if (factionRelationKind == FactionRelationKind.Hostile)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_Alive.ToList<Pawn>())
					{
						if ((pawn.Faction == this && pawn.HostFaction == other) || (pawn.Faction == other && pawn.HostFaction == this))
						{
							pawn.guest.SetGuestStatus(pawn.HostFaction, true);
						}
					}
				}
				if (other == Faction.OfPlayer)
				{
					QuestUtility.SendQuestTargetSignals(this.questTags, "BecameHostileToPlayer", this.Named("SUBJECT"));
				}
			}
			if (other == Faction.OfPlayer && !this.HostileTo(Faction.OfPlayer))
			{
				List<Site> list = new List<Site>();
				List<Site> sites = Find.WorldObjects.Sites;
				for (int i = 0; i < sites.Count; i++)
				{
					if (sites[i].factionMustRemainHostile && sites[i].Faction == this && !sites[i].HasMap)
					{
						list.Add(sites[i]);
					}
				}
				if (list.Any<Site>())
				{
					string str;
					string str2;
					if (list.Count == 1)
					{
						str = "LetterLabelSiteNoLongerHostile".Translate();
						str2 = "LetterSiteNoLongerHostile".Translate(this.NameColored, list[0].Label);
					}
					else
					{
						StringBuilder stringBuilder = new StringBuilder();
						for (int j = 0; j < list.Count; j++)
						{
							if (stringBuilder.Length != 0)
							{
								stringBuilder.AppendLine();
							}
							stringBuilder.Append("  - " + list[j].LabelCap);
							ImportantPawnComp component = list[j].GetComponent<ImportantPawnComp>();
							if (component != null && component.pawn.Any)
							{
								stringBuilder.Append(" (" + component.pawn[0].LabelCap + ")");
							}
						}
						str = "LetterLabelSiteNoLongerHostileMulti".Translate();
						str2 = "LetterSiteNoLongerHostileMulti".Translate(this.NameColored) + ":\n\n" + stringBuilder;
					}
					Find.LetterStack.ReceiveLetter(str, str2, LetterDefOf.NeutralEvent, new LookTargets(from x in list
																										select new GlobalTargetInfo(x.Tile)), null, null, null, null);
					for (int k = 0; k < list.Count; k++)
					{
						list[k].Destroy();
					}
				}
			}
			if (other == Faction.OfPlayer && this.HostileTo(Faction.OfPlayer))
			{
				List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
				for (int l = 0; l < allWorldObjects.Count; l++)
				{
					if (allWorldObjects[l].Faction == this)
					{
						TradeRequestComp component2 = allWorldObjects[l].GetComponent<TradeRequestComp>();
						if (component2 != null && component2.ActiveRequest)
						{
							component2.Disable();
						}
					}
				}
				foreach (Map map in Find.Maps)
				{
					map.passingShipManager.RemoveAllShipsOfFaction(this);
				}
			}
			if (canSendLetter)
			{
				TaggedString text = "";
				this.TryAppendRelationKindChangedInfo(ref text, previousKind, factionRelationKind, reason);
				if (factionRelationKind == FactionRelationKind.Hostile)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRelationsChange_Hostile".Translate(this.Name), text, LetterDefOf.NegativeEvent, lookTarget, this, null, null, null);
					sentLetter = true;
				}
				else if (factionRelationKind == FactionRelationKind.Ally)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRelationsChange_Ally".Translate(this.Name), text, LetterDefOf.PositiveEvent, lookTarget, this, null, null, null);
					sentLetter = true;
				}
				else if (factionRelationKind == FactionRelationKind.Neutral)
				{
					if (previousKind == FactionRelationKind.Hostile)
					{
						Find.LetterStack.ReceiveLetter("LetterLabelRelationsChange_NeutralFromHostile".Translate(this.Name), text, LetterDefOf.PositiveEvent, lookTarget, this, null, null, null);
						sentLetter = true;
					}
					else
					{
						Find.LetterStack.ReceiveLetter("LetterLabelRelationsChange_NeutralFromAlly".Translate(this.Name), text, LetterDefOf.NeutralEvent, lookTarget, this, null, null, null);
						sentLetter = true;
					}
				}
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				List<Map> maps = Find.Maps;
				for (int m = 0; m < maps.Count; m++)
				{
					maps[m].attackTargetsCache.Notify_FactionHostilityChanged(this, other);
					LordManager lordManager = maps[m].lordManager;
					for (int n = 0; n < lordManager.lords.Count; n++)
					{
						Lord lord = lordManager.lords[n];
						if (lord.faction == other)
						{
							lord.Notify_FactionRelationsChanged(this, previousKind);
						}
						else if (lord.faction == this)
						{
							lord.Notify_FactionRelationsChanged(other, previousKind);
						}
					}
				}
			}
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x00188C0C File Offset: 0x00186E0C
		public void Notify_PlayerTraded(float marketValueSentByPlayer, Pawn playerNegotiator)
		{
			this.TryAffectGoodwillWith(Faction.OfPlayer, (int)(marketValueSentByPlayer / 600f), true, true, "GoodwillChangedReason_Traded".Translate(), new GlobalTargetInfo?(playerNegotiator));
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x00188C40 File Offset: 0x00186E40
		public void Notify_MemberExitedMap(Pawn member, bool free)
		{
			if (free && member.HostFaction != null)
			{
				bool flag;
				int goodwillGainForPrisonerRelease = this.GetGoodwillGainForPrisonerRelease(member, out flag);
				if (member.mindState.AvailableForGoodwillReward)
				{
					this.TryAffectGoodwillWith(member.HostFaction, goodwillGainForPrisonerRelease, true, true, flag ? "GoodwillChangedReason_ExitedMapHealthy".Translate(member.LabelShort, member) : "GoodwillChangedReason_Tended".Translate(member.LabelShort, member), null);
				}
			}
			member.mindState.timesGuestTendedToByPlayer = 0;
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x00188CD4 File Offset: 0x00186ED4
		public int GetGoodwillGainForPrisonerRelease(Pawn member, out bool isHealthy)
		{
			isHealthy = false;
			float num = 0f;
			if (!member.InMentalState && member.health.hediffSet.BleedRateTotal < 0.001f)
			{
				isHealthy = true;
				num += 12f;
				if (PawnUtility.IsFactionLeader(member))
				{
					num += 40f;
				}
			}
			return (int)(num + (float)Mathf.Min(member.mindState.timesGuestTendedToByPlayer, 10) * 1f);
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x00188D40 File Offset: 0x00186F40
		public void GenerateNewLeader()
		{
			this.leader = null;
			if (this.def.pawnGroupMakers != null)
			{
				List<PawnKindDef> list = new List<PawnKindDef>();
				foreach (PawnGroupMaker pawnGroupMaker in from x in this.def.pawnGroupMakers
														  where x.kindDef == PawnGroupKindDefOf.Combat
														  select x)
				{
					foreach (PawnGenOption pawnGenOption in pawnGroupMaker.options)
					{
						if (pawnGenOption.kind.factionLeader)
						{
							list.Add(pawnGenOption.kind);
						}
					}
				}
				if (this.def.fixedLeaderKinds != null)
				{
					list.AddRange(this.def.fixedLeaderKinds);
				}
				PawnKindDef kind;
				if (list.TryRandomElement(out kind))
				{
					PawnGenerationRequest request = new PawnGenerationRequest(kind, this, PawnGenerationContext.NonPlayer, -1, this.def.leaderForceGenerateNewPawn, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null);
					this.leader = PawnGenerator.GeneratePawn(request);
					if (this.leader.RaceProps.IsFlesh)
					{
						this.leader.relations.everSeenByPlayer = true;
					}
					if (!Find.WorldPawns.Contains(this.leader))
					{
						Find.WorldPawns.PassToWorld(this.leader, PawnDiscardDecideMode.KeepForever);
					}
				}
			}
		}

		// Token: 0x0600485A RID: 18522 RVA: 0x00188F0C File Offset: 0x0018710C
		public string GetCallLabel()
		{
			return this.name;
		}

		// Token: 0x0600485B RID: 18523 RVA: 0x00188F14 File Offset: 0x00187114
		public string GetInfoText()
		{
			return this.def.LabelCap + ("\n" + "goodwill".Translate().CapitalizeFirst() + ": " + this.PlayerGoodwill.ToStringWithSign());
		}

		// Token: 0x0600485C RID: 18524 RVA: 0x0006461A File Offset: 0x0006281A
		Faction ICommunicable.GetFaction()
		{
			return this;
		}

		// Token: 0x0600485D RID: 18525 RVA: 0x00188F74 File Offset: 0x00187174
		public void TryOpenComms(Pawn negotiator)
		{
			Dialog_Negotiation dialog_Negotiation = new Dialog_Negotiation(negotiator, this, FactionDialogMaker.FactionDialogFor(negotiator, this), true);
			dialog_Negotiation.soundAmbient = SoundDefOf.RadioComms_Ambience;
			Find.WindowStack.Add(dialog_Negotiation);
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x00188FA8 File Offset: 0x001871A8
		private bool LeaderIsAvailableToTalk()
		{
			return this.leader != null && (!this.leader.Spawned || (!this.leader.Downed && !this.leader.IsPrisoner && this.leader.Awake() && !this.leader.InMentalState));
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x00189004 File Offset: 0x00187204
		public FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator)
		{
			if (this.IsPlayer)
			{
				return null;
			}
			string text = "CallOnRadio".Translate(this.GetCallLabel());
			text = string.Concat(new string[]
			{
				text,
				" (",
				this.PlayerRelationKind.GetLabel(),
				", ",
				this.PlayerGoodwill.ToStringWithSign(),
				")"
			});
			if (!this.LeaderIsAvailableToTalk())
			{
				string str;
				if (this.leader != null)
				{
					str = "LeaderUnavailable".Translate(this.leader.LabelShort, this.leader);
				}
				else
				{
					str = "LeaderUnavailableNoLeader".Translate();
				}
				return new FloatMenuOption(text + " (" + str + ")", null, this.def.FactionIcon, this.Color, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate
			{
				console.GiveUseCommsJob(negotiator, this);
			}, this.def.FactionIcon, this.Color, MenuOptionPriority.InitiateSocial, null, null, 0f, null, null), negotiator, console, "ReservedBy");
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x00189160 File Offset: 0x00187360
		private void TookDamageFromPredator(Pawn predator)
		{
			for (int i = 0; i < this.predatorThreats.Count; i++)
			{
				if (this.predatorThreats[i].predator == predator)
				{
					this.predatorThreats[i].lastAttackTicks = Find.TickManager.TicksGame;
					return;
				}
			}
			PredatorThreat predatorThreat = new PredatorThreat();
			predatorThreat.predator = predator;
			predatorThreat.lastAttackTicks = Find.TickManager.TicksGame;
			this.predatorThreats.Add(predatorThreat);
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x001891DC File Offset: 0x001873DC
		public bool HasPredatorRecentlyAttackedAnyone(Pawn predator)
		{
			for (int i = 0; i < this.predatorThreats.Count; i++)
			{
				if (this.predatorThreats[i].predator == predator)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x00189216 File Offset: 0x00187416
		private bool IsMutuallyHostileCrossfire(DamageInfo dinfo)
		{
			return dinfo.Instigator != null && dinfo.IntendedTarget != null && dinfo.IntendedTarget.HostileTo(dinfo.Instigator) && dinfo.IntendedTarget.HostileTo(this);
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06004863 RID: 18531 RVA: 0x0018924E File Offset: 0x0018744E
		public static Faction OfPlayer
		{
			get
			{
				Faction ofPlayerSilentFail = Faction.OfPlayerSilentFail;
				if (ofPlayerSilentFail == null)
				{
					Log.Error("Could not find player faction.", false);
				}
				return ofPlayerSilentFail;
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06004864 RID: 18532 RVA: 0x00189263 File Offset: 0x00187463
		public static Faction OfMechanoids
		{
			get
			{
				return Find.FactionManager.OfMechanoids;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06004865 RID: 18533 RVA: 0x0018926F File Offset: 0x0018746F
		public static Faction OfInsects
		{
			get
			{
				return Find.FactionManager.OfInsects;
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06004866 RID: 18534 RVA: 0x0018927B File Offset: 0x0018747B
		public static Faction OfAncients
		{
			get
			{
				return Find.FactionManager.OfAncients;
			}
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06004867 RID: 18535 RVA: 0x00189287 File Offset: 0x00187487
		public static Faction OfAncientsHostile
		{
			get
			{
				return Find.FactionManager.OfAncientsHostile;
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06004868 RID: 18536 RVA: 0x00189293 File Offset: 0x00187493
		public static Faction Empire
		{
			get
			{
				return Find.FactionManager.Empire;
			}
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06004869 RID: 18537 RVA: 0x001892A0 File Offset: 0x001874A0
		public static Faction OfPlayerSilentFail
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					GameInitData gameInitData = Find.GameInitData;
					if (gameInitData != null && gameInitData.playerFaction != null)
					{
						return gameInitData.playerFaction;
					}
				}
				return Find.FactionManager.OfPlayer;
			}
		}

		// Token: 0x0600486A RID: 18538 RVA: 0x001892D8 File Offset: 0x001874D8
		public void Notify_RoyalThingUseViolation(Def implantOrWeapon, Pawn pawn, string violationSourceName, float detectionChance, int violationSourceLevel = 0)
		{
			if (!this.HostileTo(Faction.OfPlayer))
			{
				RoyalTitleDef minTitleToUse = ThingRequiringRoyalPermissionUtility.GetMinTitleToUse(implantOrWeapon, this, violationSourceLevel);
				string arg = (minTitleToUse == null) ? "None".Translate() : minTitleToUse.GetLabelCapFor(pawn);
				this.TryAffectGoodwillWith(pawn.Faction, -4, true, true, "GoodwillChangedReason_UsedForbiddenThing".Translate(pawn.Named("PAWN"), violationSourceName.Named("CULPRIT")), new GlobalTargetInfo?(pawn));
				Find.LetterStack.ReceiveLetter("LetterLawViolationDetectedLabel".Translate(pawn.Named("PAWN")).CapitalizeFirst(), "LetterLawViolationDetectedForbiddenThingUse".Translate(arg.Named("TITLE"), pawn.Named("PAWN"), violationSourceName.Named("CULPRIT"), this.Named("FACTION"), 4.ToString().Named("GOODWILL"), detectionChance.ToStringPercent().Named("CHANCE")), LetterDefOf.NegativeEvent, pawn, null, null, null, null);
			}
		}

		// Token: 0x0600486B RID: 18539 RVA: 0x001893EC File Offset: 0x001875EC
		public RoyalTitleDef GetMinTitleForImplant(HediffDef implantDef, int level = 0)
		{
			if (this.def.royalImplantRules == null || this.def.royalImplantRules.Count == 0)
			{
				return null;
			}
			foreach (RoyalImplantRule royalImplantRule in this.def.royalImplantRules)
			{
				if (royalImplantRule.implantHediff == implantDef && (royalImplantRule.maxLevel >= level || level == 0))
				{
					return royalImplantRule.minTitle;
				}
			}
			return null;
		}

		// Token: 0x0600486C RID: 18540 RVA: 0x00189480 File Offset: 0x00187680
		public RoyalImplantRule GetMaxAllowedImplantLevel(HediffDef implantDef, RoyalTitleDef title)
		{
			if (this.def.royalImplantRules == null || this.def.royalImplantRules.Count == 0)
			{
				return null;
			}
			if (title == null)
			{
				return null;
			}
			int myTitleIdx = this.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(title);
			return this.def.royalImplantRules.Where(delegate (RoyalImplantRule r)
			{
				if (r.implantHediff == implantDef)
				{
					RoyalTitleDef minTitleForImplant = this.GetMinTitleForImplant(implantDef, r.maxLevel);
					int num = this.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(minTitleForImplant);
					return myTitleIdx == -1 || myTitleIdx >= num;
				}
				return false;
			}).Last<RoyalImplantRule>();
		}

		// Token: 0x0600486D RID: 18541 RVA: 0x00189500 File Offset: 0x00187700
		public static Pair<Faction, RoyalTitleDef> GetMinTitleForImplantAllFactions(HediffDef implantDef)
		{
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				RoyalTitleDef minTitleForImplant = faction.GetMinTitleForImplant(implantDef, 0);
				if (minTitleForImplant != null)
				{
					return new Pair<Faction, RoyalTitleDef>(faction, minTitleForImplant);
				}
			}
			return default(Pair<Faction, RoyalTitleDef>);
		}

		public string GetUniqueLoadID()
		{
			return "Faction_" + this.loadID;
		}

		public override string ToString()
		{
			if (this.name != null)
			{
				return this.name;
			}
			if (this.def != null)
			{
				return this.def.defName;
			}
			return "[faction of no def]";
		}

	}
}
