using System;

namespace Verse.AI.Group
{
	// Token: 0x020005CB RID: 1483
	public class LordJob_Travel : LordJob
	{
		// Token: 0x0600296B RID: 10603 RVA: 0x000F4066 File Offset: 0x000F2266
		public LordJob_Travel()
		{
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x000F42BB File Offset: 0x000F24BB
		public LordJob_Travel(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x0600296D RID: 10605 RVA: 0x000F42CC File Offset: 0x000F24CC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.travelDest);
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(false);
			stateGraph.AddToil(lordToil_DefendPoint);
			Transition transition = new Transition(lordToil_Travel, lordToil_DefendPoint, false, true);
			transition.AddTrigger(new Trigger_PawnHarmed(1f, false, null));
			transition.AddPreAction(new TransitionAction_SetDefendLocalGroup());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_DefendPoint, lordToil_Travel, false, true);
			transition2.AddTrigger(new Trigger_TicksPassedWithoutHarm(1200));
			transition2.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			stateGraph.AddTransition(transition2, false);
			return stateGraph;
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x000F4368 File Offset: 0x000F2568
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}

		// Token: 0x040018F0 RID: 6384
		private IntVec3 travelDest;
	}
}
