using System;

namespace Verse.AI.Group
{
	// Token: 0x020005C9 RID: 1481
	public class LordJob_ExitMapBest : LordJob
	{
		// Token: 0x06002963 RID: 10595 RVA: 0x000F40C3 File Offset: 0x000F22C3
		public LordJob_ExitMapBest()
		{
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x000F40D2 File Offset: 0x000F22D2
		public LordJob_ExitMapBest(LocomotionUrgency locomotion, bool canDig = false, bool canDefendSelf = false)
		{
			this.locomotion = locomotion;
			this.canDig = canDig;
			this.canDefendSelf = canDefendSelf;
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x000F40F8 File Offset: 0x000F22F8
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(this.locomotion, this.canDig, false);
			lordToil_ExitMap.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap);
			if (this.canDefendSelf)
			{
				LordToil_ExitMapFighting lordToil_ExitMapFighting = new LordToil_ExitMapFighting(LocomotionUrgency.Jog, this.canDig, false);
				stateGraph.AddToil(lordToil_ExitMapFighting);
				Transition transition = new Transition(lordToil_ExitMap, lordToil_ExitMapFighting, false, true);
				transition.AddTrigger(new Trigger_PawnHarmed(1f, false, null));
				transition.AddPostAction(new TransitionAction_WakeAll());
				transition.AddPostAction(new TransitionAction_EndAllJobs());
				stateGraph.AddTransition(transition, false);
			}
			return stateGraph;
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x000F4184 File Offset: 0x000F2384
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.canDefendSelf, "canDefendSelf", false, false);
		}

		// Token: 0x040018E7 RID: 6375
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x040018E8 RID: 6376
		private bool canDig;

		// Token: 0x040018E9 RID: 6377
		private bool canDefendSelf;
	}
}
