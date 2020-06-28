using System;

namespace Verse.AI.Group
{
	// Token: 0x020005C8 RID: 1480
	public class LordJob_DefendPoint : LordJob
	{
		// Token: 0x0600295F RID: 10591 RVA: 0x000F4066 File Offset: 0x000F2266
		public LordJob_DefendPoint()
		{
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x000F406E File Offset: 0x000F226E
		public LordJob_DefendPoint(IntVec3 point)
		{
			this.point = point;
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x000F407D File Offset: 0x000F227D
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_DefendPoint(this.point, 28f));
			return stateGraph;
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x000F409C File Offset: 0x000F229C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
		}

		// Token: 0x040018E6 RID: 6374
		private IntVec3 point;
	}
}
