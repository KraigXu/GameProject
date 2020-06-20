using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200076F RID: 1903
	public class LordJob_Kidnap : LordJob
	{
		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x060031BE RID: 12734 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x00115538 File Offset: 0x00113738
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_KidnapCover lordToil_KidnapCover = new LordToil_KidnapCover();
			lordToil_KidnapCover.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_KidnapCover);
			LordToil_KidnapCover lordToil_KidnapCover2 = new LordToil_KidnapCover();
			lordToil_KidnapCover2.cover = false;
			lordToil_KidnapCover2.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_KidnapCover2);
			Transition transition = new Transition(lordToil_KidnapCover, lordToil_KidnapCover2, false, true);
			transition.AddTrigger(new Trigger_TicksPassed(1200));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x00002681 File Offset: 0x00000881
		public override void ExposeData()
		{
		}
	}
}
