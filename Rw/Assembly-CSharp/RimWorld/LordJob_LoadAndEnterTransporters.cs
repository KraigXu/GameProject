using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000770 RID: 1904
	public class LordJob_LoadAndEnterTransporters : LordJob
	{
		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x060031C2 RID: 12738 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x060031C3 RID: 12739 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060031C4 RID: 12740 RVA: 0x0011559B File Offset: 0x0011379B
		public LordJob_LoadAndEnterTransporters()
		{
		}

		// Token: 0x060031C5 RID: 12741 RVA: 0x001155AA File Offset: 0x001137AA
		public LordJob_LoadAndEnterTransporters(int transportersGroup)
		{
			this.transportersGroup = transportersGroup;
		}

		// Token: 0x060031C6 RID: 12742 RVA: 0x001155C0 File Offset: 0x001137C0
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.transportersGroup, "transportersGroup", 0, false);
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x001155D4 File Offset: 0x001137D4
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_LoadAndEnterTransporters startingToil = new LordToil_LoadAndEnterTransporters(this.transportersGroup);
			stateGraph.StartingToil = startingToil;
			LordToil_End toil = new LordToil_End();
			stateGraph.AddToil(toil);
			return stateGraph;
		}

		// Token: 0x04001B26 RID: 6950
		public int transportersGroup = -1;
	}
}
