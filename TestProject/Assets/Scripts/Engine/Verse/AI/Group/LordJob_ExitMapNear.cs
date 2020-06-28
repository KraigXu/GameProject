using System;

namespace Verse.AI.Group
{
	// Token: 0x020005CA RID: 1482
	public class LordJob_ExitMapNear : LordJob
	{
		// Token: 0x06002967 RID: 10599 RVA: 0x000F41BC File Offset: 0x000F23BC
		public LordJob_ExitMapNear()
		{
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x000F41CB File Offset: 0x000F23CB
		public LordJob_ExitMapNear(IntVec3 near, LocomotionUrgency locomotion, float radius = 12f, bool canDig = false, bool useAvoidGridSmart = false)
		{
			this.near = near;
			this.locomotion = locomotion;
			this.radius = radius;
			this.canDig = canDig;
			this.useAvoidGridSmart = useAvoidGridSmart;
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x000F4200 File Offset: 0x000F2400
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_ExitMapNear lordToil_ExitMapNear = new LordToil_ExitMapNear(this.near, this.radius, this.locomotion, this.canDig);
			if (this.useAvoidGridSmart)
			{
				lordToil_ExitMapNear.useAvoidGrid = true;
			}
			stateGraph.AddToil(lordToil_ExitMapNear);
			return stateGraph;
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x000F4248 File Offset: 0x000F2448
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.near, "near", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
		}

		// Token: 0x040018EA RID: 6378
		private IntVec3 near;

		// Token: 0x040018EB RID: 6379
		private float radius;

		// Token: 0x040018EC RID: 6380
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x040018ED RID: 6381
		private bool canDig;

		// Token: 0x040018EE RID: 6382
		private bool useAvoidGridSmart;

		// Token: 0x040018EF RID: 6383
		public const float DefaultRadius = 12f;
	}
}
