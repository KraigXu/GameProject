using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordJob_LoadAndEnterTransporters : LordJob
	{
		
		// (get) Token: 0x060031C2 RID: 12738 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x060031C3 RID: 12739 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		
		public LordJob_LoadAndEnterTransporters()
		{
		}

		
		public LordJob_LoadAndEnterTransporters(int transportersGroup)
		{
			this.transportersGroup = transportersGroup;
		}

		
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.transportersGroup, "transportersGroup", 0, false);
		}

		
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_LoadAndEnterTransporters startingToil = new LordToil_LoadAndEnterTransporters(this.transportersGroup);
			stateGraph.StartingToil = startingToil;
			LordToil_End toil = new LordToil_End();
			stateGraph.AddToil(toil);
			return stateGraph;
		}

		
		public int transportersGroup = -1;
	}
}
