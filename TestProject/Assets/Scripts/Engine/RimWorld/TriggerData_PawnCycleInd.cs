using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class TriggerData_PawnCycleInd : TriggerData
	{
		
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.pawnCycleInd, "pawnCycleInd", 0, false);
		}

		
		public int pawnCycleInd;
	}
}
