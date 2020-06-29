using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class GatheringWorker_Party : GatheringWorker
	{
		
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			return new LordJob_Joinable_Party(spot, organizer, this.def);
		}

		
		protected override bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot)
		{
			return RCellFinder.TryFindGatheringSpot(organizer, this.def, out spot);
		}
	}
}
