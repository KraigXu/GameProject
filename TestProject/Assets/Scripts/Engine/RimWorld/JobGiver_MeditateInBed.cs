using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobGiver_MeditateInBed : JobGiver_Meditate
	{
		
		protected override bool ValidatePawnState(Pawn pawn)
		{
			return pawn.CurrentBed() != null && pawn.Awake();
		}

		
		protected override Job TryGiveJob(Pawn pawn)
		{
			LocalTargetInfo targetC = MeditationUtility.BestFocusAt(pawn.Position, pawn);
			Job job = JobMaker.MakeJob(JobDefOf.Meditate, pawn.Position, pawn.InBed() ? pawn.CurrentBed() : new LocalTargetInfo(pawn.Position), targetC);
			job.ignoreJoyTimeAssignment = true;
			return job;
		}
	}
}
