using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200076E RID: 1902
	public class LordJob_FormAndSendCaravan : LordJob
	{
		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x060031AB RID: 12715 RVA: 0x00114CE6 File Offset: 0x00112EE6
		public bool GatheringItemsNow
		{
			get
			{
				return this.lord.CurLordToil == this.gatherItems;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x060031AC RID: 12716 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x060031AD RID: 12717 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x060031AE RID: 12718 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x060031AF RID: 12719 RVA: 0x00114CFC File Offset: 0x00112EFC
		public string Status
		{
			get
			{
				LordToil curLordToil = this.lord.CurLordToil;
				if (curLordToil == this.gatherAnimals)
				{
					return "FormingCaravanStatus_GatheringAnimals".Translate();
				}
				if (curLordToil == this.gatherAnimals_pause)
				{
					return "FormingCaravanStatus_GatheringAnimals_Pause".Translate();
				}
				if (curLordToil == this.gatherItems)
				{
					return "FormingCaravanStatus_GatheringItems".Translate();
				}
				if (curLordToil == this.gatherItems_pause)
				{
					return "FormingCaravanStatus_GatheringItems_Pause".Translate();
				}
				if (curLordToil == this.gatherSlaves)
				{
					return "FormingCaravanStatus_GatheringSlaves".Translate();
				}
				if (curLordToil == this.gatherSlaves_pause)
				{
					return "FormingCaravanStatus_GatheringSlaves_Pause".Translate();
				}
				if (curLordToil == this.gatherDownedPawns)
				{
					return "FormingCaravanStatus_GatheringDownedPawns".Translate();
				}
				if (curLordToil == this.gatherDownedPawns_pause)
				{
					return "FormingCaravanStatus_GatheringDownedPawns_Pause".Translate();
				}
				if (curLordToil == this.leave)
				{
					return "FormingCaravanStatus_Leaving".Translate();
				}
				if (curLordToil == this.leave_pause)
				{
					return "FormingCaravanStatus_Leaving_Pause".Translate();
				}
				return "FormingCaravanStatus_Waiting".Translate();
			}
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x000F4066 File Offset: 0x000F2266
		public LordJob_FormAndSendCaravan()
		{
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x00114E1E File Offset: 0x0011301E
		public LordJob_FormAndSendCaravan(List<TransferableOneWay> transferables, List<Pawn> downedPawns, IntVec3 meetingPoint, IntVec3 exitSpot, int startingTile, int destinationTile)
		{
			this.transferables = transferables;
			this.downedPawns = downedPawns;
			this.meetingPoint = meetingPoint;
			this.exitSpot = exitSpot;
			this.startingTile = startingTile;
			this.destinationTile = destinationTile;
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x00114E54 File Offset: 0x00113054
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			this.gatherAnimals = new LordToil_PrepareCaravan_GatherAnimals(this.meetingPoint);
			stateGraph.AddToil(this.gatherAnimals);
			this.gatherAnimals_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.gatherAnimals_pause);
			this.gatherItems = new LordToil_PrepareCaravan_GatherItems(this.meetingPoint);
			stateGraph.AddToil(this.gatherItems);
			this.gatherItems_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.gatherItems_pause);
			this.gatherSlaves = new LordToil_PrepareCaravan_GatherSlaves(this.meetingPoint);
			stateGraph.AddToil(this.gatherSlaves);
			this.gatherSlaves_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.gatherSlaves_pause);
			this.gatherDownedPawns = new LordToil_PrepareCaravan_GatherDownedPawns(this.meetingPoint, this.exitSpot);
			stateGraph.AddToil(this.gatherDownedPawns);
			this.gatherDownedPawns_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.gatherDownedPawns_pause);
			LordToil_PrepareCaravan_Wait lordToil_PrepareCaravan_Wait = new LordToil_PrepareCaravan_Wait(this.meetingPoint);
			stateGraph.AddToil(lordToil_PrepareCaravan_Wait);
			LordToil_PrepareCaravan_Pause lordToil_PrepareCaravan_Pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(lordToil_PrepareCaravan_Pause);
			this.leave = new LordToil_PrepareCaravan_Leave(this.exitSpot);
			stateGraph.AddToil(this.leave);
			this.leave_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.leave_pause);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(this.gatherAnimals, this.gatherItems, false, true);
			transition.AddTrigger(new Trigger_Memo("AllAnimalsGathered"));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(this.gatherItems, this.gatherSlaves, false, true);
			transition2.AddTrigger(new Trigger_Memo("AllItemsGathered"));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(this.gatherSlaves, this.gatherDownedPawns, false, true);
			transition3.AddTrigger(new Trigger_Memo("AllSlavesGathered"));
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(this.gatherDownedPawns, lordToil_PrepareCaravan_Wait, false, true);
			transition4.AddTrigger(new Trigger_Memo("AllDownedPawnsGathered"));
			transition4.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_PrepareCaravan_Wait, this.leave, false, true);
			transition5.AddTrigger(new Trigger_NoPawnsVeryTiredAndSleeping(0f));
			transition5.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(this.leave, lordToil_End, false, true);
			transition6.AddTrigger(new Trigger_Memo("ReadyToExitMap"));
			transition6.AddPreAction(new TransitionAction_Custom(new Action(this.SendCaravan)));
			stateGraph.AddTransition(transition6, false);
			Transition transition7 = this.PauseTransition(this.gatherAnimals, this.gatherAnimals_pause);
			stateGraph.AddTransition(transition7, false);
			Transition transition8 = this.UnpauseTransition(this.gatherAnimals_pause, this.gatherAnimals);
			stateGraph.AddTransition(transition8, false);
			Transition transition9 = this.PauseTransition(this.gatherItems, this.gatherItems_pause);
			stateGraph.AddTransition(transition9, false);
			Transition transition10 = this.UnpauseTransition(this.gatherItems_pause, this.gatherItems);
			stateGraph.AddTransition(transition10, false);
			Transition transition11 = this.PauseTransition(this.gatherSlaves, this.gatherSlaves_pause);
			stateGraph.AddTransition(transition11, false);
			Transition transition12 = this.UnpauseTransition(this.gatherSlaves_pause, this.gatherSlaves);
			stateGraph.AddTransition(transition12, false);
			Transition transition13 = this.PauseTransition(this.gatherDownedPawns, this.gatherDownedPawns_pause);
			stateGraph.AddTransition(transition13, false);
			Transition transition14 = this.UnpauseTransition(this.gatherDownedPawns_pause, this.gatherDownedPawns);
			stateGraph.AddTransition(transition14, false);
			Transition transition15 = this.PauseTransition(this.leave, this.leave_pause);
			stateGraph.AddTransition(transition15, false);
			Transition transition16 = this.UnpauseTransition(this.leave_pause, this.leave);
			stateGraph.AddTransition(transition16, false);
			Transition transition17 = this.PauseTransition(lordToil_PrepareCaravan_Wait, lordToil_PrepareCaravan_Pause);
			stateGraph.AddTransition(transition17, false);
			Transition transition18 = this.UnpauseTransition(lordToil_PrepareCaravan_Pause, lordToil_PrepareCaravan_Wait);
			stateGraph.AddTransition(transition18, false);
			return stateGraph;
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x00115238 File Offset: 0x00113438
		public override void LordJobTick()
		{
			base.LordJobTick();
			for (int i = this.downedPawns.Count - 1; i >= 0; i--)
			{
				if (this.downedPawns[i].Destroyed)
				{
					this.downedPawns.RemoveAt(i);
				}
				else if (!this.downedPawns[i].Downed)
				{
					this.lord.AddPawn(this.downedPawns[i]);
					this.downedPawns.RemoveAt(i);
				}
			}
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x001152BA File Offset: 0x001134BA
		public override string GetReport(Pawn pawn)
		{
			return "LordReportFormingCaravan".Translate();
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x001152CC File Offset: 0x001134CC
		private Transition PauseTransition(LordToil from, LordToil to)
		{
			Transition transition = new Transition(from, to, false, true);
			transition.AddPreAction(new TransitionAction_Message("MessageCaravanFormationPaused".Translate(), MessageTypeDefOf.NegativeEvent, () => this.lord.ownedPawns.FirstOrDefault((Pawn x) => x.InMentalState), null, 1f));
			transition.AddTrigger(new Trigger_MentalState());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			return transition;
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x0011532C File Offset: 0x0011352C
		private Transition UnpauseTransition(LordToil from, LordToil to)
		{
			Transition transition = new Transition(from, to, false, true);
			transition.AddPreAction(new TransitionAction_Message("MessageCaravanFormationUnpaused".Translate(), MessageTypeDefOf.SilentInput, null, 1f));
			transition.AddTrigger(new Trigger_NoMentalState());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			return transition;
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x00115380 File Offset: 0x00113580
		public override void ExposeData()
		{
			Scribe_Collections.Look<TransferableOneWay>(ref this.transferables, "transferables", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.downedPawns, "downedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<IntVec3>(ref this.meetingPoint, "meetingPoint", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.exitSpot, "exitSpot", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.startingTile, "startingTile", 0, false);
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.downedPawns.RemoveAll((Pawn x) => x.DestroyedOrNull());
			}
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x00115444 File Offset: 0x00113644
		private void SendCaravan()
		{
			this.caravanSent = true;
			CaravanFormingUtility.FormAndCreateCaravan(this.lord.ownedPawns.Concat(from x in this.downedPawns
			where JobGiver_PrepareCaravan_GatherDownedPawns.IsDownedPawnNearExitPoint(x, this.exitSpot)
			select x), this.lord.faction, base.Map.Tile, this.startingTile, this.destinationTile);
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x001154A6 File Offset: 0x001136A6
		public override void Notify_PawnAdded(Pawn p)
		{
			base.Notify_PawnAdded(p);
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x001154B5 File Offset: 0x001136B5
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			base.Notify_PawnLost(p, condition);
			ReachabilityUtility.ClearCacheFor(p);
			if (!this.caravanSent)
			{
				if (condition == PawnLostCondition.IncappedOrKilled && p.Downed)
				{
					this.downedPawns.Add(p);
				}
				CaravanFormingUtility.RemovePawnFromCaravan(p, this.lord, false);
			}
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool CanOpenAnyDoor(Pawn p)
		{
			return true;
		}

		// Token: 0x04001B14 RID: 6932
		public List<TransferableOneWay> transferables;

		// Token: 0x04001B15 RID: 6933
		public List<Pawn> downedPawns;

		// Token: 0x04001B16 RID: 6934
		private IntVec3 meetingPoint;

		// Token: 0x04001B17 RID: 6935
		private IntVec3 exitSpot;

		// Token: 0x04001B18 RID: 6936
		private int startingTile;

		// Token: 0x04001B19 RID: 6937
		private int destinationTile;

		// Token: 0x04001B1A RID: 6938
		private bool caravanSent;

		// Token: 0x04001B1B RID: 6939
		private LordToil gatherAnimals;

		// Token: 0x04001B1C RID: 6940
		private LordToil gatherAnimals_pause;

		// Token: 0x04001B1D RID: 6941
		private LordToil gatherItems;

		// Token: 0x04001B1E RID: 6942
		private LordToil gatherItems_pause;

		// Token: 0x04001B1F RID: 6943
		private LordToil gatherSlaves;

		// Token: 0x04001B20 RID: 6944
		private LordToil gatherSlaves_pause;

		// Token: 0x04001B21 RID: 6945
		private LordToil gatherDownedPawns;

		// Token: 0x04001B22 RID: 6946
		private LordToil gatherDownedPawns_pause;

		// Token: 0x04001B23 RID: 6947
		private LordToil leave;

		// Token: 0x04001B24 RID: 6948
		private LordToil leave_pause;

		// Token: 0x04001B25 RID: 6949
		public const float CustomWakeThreshold = 0.5f;
	}
}
