using System;

namespace Verse.AI.Group
{
	// Token: 0x020005CC RID: 1484
	public class LordJob_TravelAndExit : LordJob
	{
		// Token: 0x0600296F RID: 10607 RVA: 0x000F4066 File Offset: 0x000F2266
		public LordJob_TravelAndExit()
		{
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x000F438F File Offset: 0x000F258F
		public LordJob_TravelAndExit(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x000F43A0 File Offset: 0x000F25A0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Travel(this.travelDest).CreateGraph()).StartingToil;
			stateGraph.StartingToil = startingToil;
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			stateGraph.AddTransition(new Transition(startingToil, lordToil_ExitMap, false, true)
			{
				triggers = 
				{
					new Trigger_Memo("TravelArrived")
				}
			}, false);
			return stateGraph;
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x000F440C File Offset: 0x000F260C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}

		// Token: 0x040018F1 RID: 6385
		private IntVec3 travelDest;
	}
}
