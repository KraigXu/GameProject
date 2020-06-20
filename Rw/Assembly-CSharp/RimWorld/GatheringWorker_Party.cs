using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008C7 RID: 2247
	public class GatheringWorker_Party : GatheringWorker
	{
		// Token: 0x0600361D RID: 13853 RVA: 0x00125AB8 File Offset: 0x00123CB8
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			return new LordJob_Joinable_Party(spot, organizer, this.def);
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x00125AC7 File Offset: 0x00123CC7
		protected override bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot)
		{
			return RCellFinder.TryFindGatheringSpot(organizer, this.def, out spot);
		}
	}
}
