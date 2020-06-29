using System;

namespace Verse.AI.Group
{
	
	public class LordJob_ExitMapBest : LordJob
	{
		
		public LordJob_ExitMapBest()
		{
		}

		
		public LordJob_ExitMapBest(LocomotionUrgency locomotion, bool canDig = false, bool canDefendSelf = false)
		{
			this.locomotion = locomotion;
			this.canDig = canDig;
			this.canDefendSelf = canDefendSelf;
		}

		
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

		
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.canDefendSelf, "canDefendSelf", false, false);
		}

		
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		
		private bool canDig;

		
		private bool canDefendSelf;
	}
}
