using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000737 RID: 1847
	public class WorkGiver_Warden_ReleasePrisoner : WorkGiver_Warden
	{
		// Token: 0x06003071 RID: 12401 RVA: 0x0010FC30 File Offset: 0x0010DE30
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.guest.interactionMode != PrisonerInteractionModeDefOf.Release || pawn2.Downed)
			{
				return null;
			}
			IntVec3 c;
			if (!RCellFinder.TryFindPrisonerReleaseCell(pawn2, pawn, out c))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.ReleasePrisoner, pawn2, c);
			job.count = 1;
			return job;
		}
	}
}
