using System;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordJob_Steal : LordJob
	{
		
		// (get) Token: 0x060031F7 RID: 12791 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_StealCover lordToil_StealCover = new LordToil_StealCover();
			lordToil_StealCover.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_StealCover);
			LordToil_StealCover lordToil_StealCover2 = new LordToil_StealCover();
			lordToil_StealCover2.cover = false;
			lordToil_StealCover2.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_StealCover2);
			Transition transition = new Transition(lordToil_StealCover, lordToil_StealCover2, false, true);
			transition.AddTrigger(new Trigger_TicksPassedAndNoRecentHarm(1200));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}
	}
}
