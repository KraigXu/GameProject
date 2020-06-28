using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200076C RID: 1900
	public class LordJob_DefendAttackedTraderCaravan : LordJob
	{
		// Token: 0x060031A3 RID: 12707 RVA: 0x000F4066 File Offset: 0x000F2266
		public LordJob_DefendAttackedTraderCaravan()
		{
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x00114A60 File Offset: 0x00112C60
		public LordJob_DefendAttackedTraderCaravan(IntVec3 defendSpot)
		{
			this.defendSpot = defendSpot;
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x00114A70 File Offset: 0x00112C70
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendTraderCaravan lordToil_DefendTraderCaravan = new LordToil_DefendTraderCaravan(this.defendSpot);
			stateGraph.StartingToil = lordToil_DefendTraderCaravan;
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			Transition transition = new Transition(lordToil_DefendTraderCaravan, lordToil_ExitMap, false, true);
			transition.AddTrigger(new Trigger_BecameNonHostileToPlayer());
			transition.AddTrigger(new Trigger_TraderAndAllTraderCaravanGuardsLost());
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x00114AD0 File Offset: 0x00112CD0
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendSpot, "defendSpot", default(IntVec3), false);
		}

		// Token: 0x04001B11 RID: 6929
		private IntVec3 defendSpot;
	}
}
