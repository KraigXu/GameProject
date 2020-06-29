using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class WorkGiver_Warden_ReleasePrisoner : WorkGiver_Warden
	{
		
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
