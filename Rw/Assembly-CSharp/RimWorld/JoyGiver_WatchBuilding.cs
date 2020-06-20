using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006F3 RID: 1779
	public class JoyGiver_WatchBuilding : JoyGiver_InteractBuilding
	{
		// Token: 0x06002F21 RID: 12065 RVA: 0x00109028 File Offset: 0x00107228
		protected override bool CanInteractWith(Pawn pawn, Thing t, bool inBed)
		{
			if (!base.CanInteractWith(pawn, t, inBed))
			{
				return false;
			}
			if (inBed)
			{
				Building_Bed bed = pawn.CurrentBed();
				return WatchBuildingUtility.CanWatchFromBed(pawn, bed, t);
			}
			return true;
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x00109058 File Offset: 0x00107258
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			IntVec3 c;
			Building t2;
			if (!WatchBuildingUtility.TryFindBestWatchCell(t, pawn, this.def.desireSit, out c, out t2))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, t, c, t2);
		}
	}
}
