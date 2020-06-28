using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200077E RID: 1918
	public class LordJob_Joinable_MarriageCeremony : LordJob_VoluntarilyJoinable
	{
		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06003217 RID: 12823 RVA: 0x00117067 File Offset: 0x00115267
		public override bool LostImportantReferenceDuringLoading
		{
			get
			{
				return this.firstPawn == null || this.secondPawn == null;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06003218 RID: 12824 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x00116F32 File Offset: 0x00115132
		public LordJob_Joinable_MarriageCeremony()
		{
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x0011707C File Offset: 0x0011527C
		public LordJob_Joinable_MarriageCeremony(Pawn firstPawn, Pawn secondPawn, IntVec3 spot)
		{
			this.firstPawn = firstPawn;
			this.secondPawn = secondPawn;
			this.spot = spot;
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x0011709C File Offset: 0x0011529C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Party lordToil_Party = new LordToil_Party(this.spot, GatheringDefOf.Party, 3.5E-05f);
			stateGraph.AddToil(lordToil_Party);
			LordToil_MarriageCeremony lordToil_MarriageCeremony = new LordToil_MarriageCeremony(this.firstPawn, this.secondPawn, this.spot);
			stateGraph.AddToil(lordToil_MarriageCeremony);
			LordToil_Party lordToil_Party2 = new LordToil_Party(this.spot, GatheringDefOf.Party, 3.5E-05f);
			stateGraph.AddToil(lordToil_Party2);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(lordToil_Party, lordToil_MarriageCeremony, false, true);
			transition.AddTrigger(new Trigger_TickCondition(() => this.lord.ticksInToil >= 5000 && this.AreFiancesInPartyArea(), 1));
			transition.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyStarts".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.PositiveEvent, this.firstPawn, null, 1f));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_MarriageCeremony, lordToil_Party2, false, true);
			transition2.AddTrigger(new Trigger_TickCondition(() => this.firstPawn.relations.DirectRelationExists(PawnRelationDefOf.Spouse, this.secondPawn), 1));
			transition2.AddPreAction(new TransitionAction_Message("MessageNewlyMarried".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.PositiveEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			transition2.AddPreAction(new TransitionAction_Custom(delegate
			{
				this.AddAttendedWeddingThoughts();
			}));
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_Party2, lordToil_End, false, true);
			transition3.AddTrigger(new Trigger_TickCondition(() => this.ShouldAfterPartyBeCalledOff(), 1));
			transition3.AddTrigger(new Trigger_PawnKilled());
			stateGraph.AddTransition(transition3, false);
			this.afterPartyTimeoutTrigger = new Trigger_TicksPassed(7500);
			Transition transition4 = new Transition(lordToil_Party2, lordToil_End, false, true);
			transition4.AddTrigger(this.afterPartyTimeoutTrigger);
			transition4.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyAfterPartyFinished".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.PositiveEvent, this.firstPawn, null, 1f));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_MarriageCeremony, lordToil_End, false, true);
			transition5.AddSource(lordToil_Party);
			transition5.AddTrigger(new Trigger_TickCondition(() => this.lord.ticksInToil >= 120000 && (this.firstPawn.Drafted || this.secondPawn.Drafted || !this.firstPawn.Position.InHorDistOf(this.spot, 4f) || !this.secondPawn.Position.InHorDistOf(this.spot, 4f)), 1));
			transition5.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyCalledOff".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_MarriageCeremony, lordToil_End, false, true);
			transition6.AddSource(lordToil_Party);
			transition6.AddTrigger(new Trigger_TickCondition(() => this.ShouldCeremonyBeCalledOff(), 1));
			transition6.AddTrigger(new Trigger_PawnKilled());
			transition6.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyCalledOff".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition6, false);
			return stateGraph;
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x001174B0 File Offset: 0x001156B0
		private bool AreFiancesInPartyArea()
		{
			return this.lord.ownedPawns.Contains(this.firstPawn) && this.lord.ownedPawns.Contains(this.secondPawn) && this.firstPawn.Map == base.Map && GatheringsUtility.InGatheringArea(this.firstPawn.Position, this.spot, base.Map) && this.secondPawn.Map == base.Map && GatheringsUtility.InGatheringArea(this.secondPawn.Position, this.spot, base.Map);
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x00117558 File Offset: 0x00115758
		private bool ShouldCeremonyBeCalledOff()
		{
			return this.firstPawn.Destroyed || this.secondPawn.Destroyed || !this.firstPawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, this.secondPawn) || (this.spot.GetDangerFor(this.firstPawn, base.Map) != Danger.None || this.spot.GetDangerFor(this.secondPawn, base.Map) != Danger.None) || (!GatheringsUtility.AcceptableGameConditionsToContinueGathering(base.Map) || !MarriageCeremonyUtility.FianceCanContinueCeremony(this.firstPawn, this.secondPawn) || !MarriageCeremonyUtility.FianceCanContinueCeremony(this.secondPawn, this.firstPawn));
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x0011760C File Offset: 0x0011580C
		private bool ShouldAfterPartyBeCalledOff()
		{
			return this.firstPawn.Destroyed || this.secondPawn.Destroyed || (this.firstPawn.Downed || this.secondPawn.Downed) || (this.spot.GetDangerFor(this.firstPawn, base.Map) != Danger.None || this.spot.GetDangerFor(this.secondPawn, base.Map) != Danger.None) || !GatheringsUtility.AcceptableGameConditionsToContinueGathering(base.Map);
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x00117698 File Offset: 0x00115898
		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			if (this.IsFiance(p))
			{
				if (!MarriageCeremonyUtility.FianceCanContinueCeremony(p, (p == this.firstPawn) ? this.secondPawn : this.firstPawn))
				{
					return 0f;
				}
				return VoluntarilyJoinableLordJobJoinPriorities.MarriageCeremonyFiance;
			}
			else
			{
				if (!this.IsGuest(p))
				{
					return 0f;
				}
				if (!MarriageCeremonyUtility.ShouldGuestKeepAttendingCeremony(p))
				{
					return 0f;
				}
				if (!this.lord.ownedPawns.Contains(p))
				{
					if (this.IsCeremonyAboutToEnd())
					{
						return 0f;
					}
					LordToil_MarriageCeremony lordToil_MarriageCeremony = this.lord.CurLordToil as LordToil_MarriageCeremony;
					IntVec3 intVec;
					if (lordToil_MarriageCeremony != null && !SpectatorCellFinder.TryFindSpectatorCellFor(p, lordToil_MarriageCeremony.Data.spectateRect, base.Map, out intVec, lordToil_MarriageCeremony.Data.spectateRectAllowedSides, 1, null))
					{
						return 0f;
					}
				}
				return VoluntarilyJoinableLordJobJoinPriorities.MarriageCeremonyGuest;
			}
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x00117760 File Offset: 0x00115960
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.firstPawn, "firstPawn", false);
			Scribe_References.Look<Pawn>(ref this.secondPawn, "secondPawn", false);
			Scribe_Values.Look<IntVec3>(ref this.spot, "spot", default(IntVec3), false);
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x001177A9 File Offset: 0x001159A9
		public override string GetReport(Pawn pawn)
		{
			return "LordReportAttendingMarriageCeremony".Translate();
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x001177BA File Offset: 0x001159BA
		private bool IsCeremonyAboutToEnd()
		{
			return this.afterPartyTimeoutTrigger.TicksLeft < 1200;
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x001177D1 File Offset: 0x001159D1
		private bool IsFiance(Pawn p)
		{
			return p == this.firstPawn || p == this.secondPawn;
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x001177E8 File Offset: 0x001159E8
		private bool IsGuest(Pawn p)
		{
			return p.RaceProps.Humanlike && p != this.firstPawn && p != this.secondPawn && (p.Faction == this.firstPawn.Faction || p.Faction == this.secondPawn.Faction);
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x00117840 File Offset: 0x00115A40
		private void AddAttendedWeddingThoughts()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (ownedPawns[i] != this.firstPawn && ownedPawns[i] != this.secondPawn && ownedPawns[i].needs.mood != null && (this.firstPawn.Position.InHorDistOf(ownedPawns[i].Position, 18f) || this.secondPawn.Position.InHorDistOf(ownedPawns[i].Position, 18f)))
				{
					ownedPawns[i].needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.AttendedWedding, null);
				}
			}
		}

		// Token: 0x04001B43 RID: 6979
		public Pawn firstPawn;

		// Token: 0x04001B44 RID: 6980
		public Pawn secondPawn;

		// Token: 0x04001B45 RID: 6981
		private IntVec3 spot;

		// Token: 0x04001B46 RID: 6982
		private Trigger_TicksPassed afterPartyTimeoutTrigger;
	}
}
