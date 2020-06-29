using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class PsycastUtility
	{
		
		public static float TotalEntropyFromQueuedPsycasts(Pawn pawn)
		{
			Job curJob = pawn.jobs.curJob;
			Verb_CastPsycast verb_CastPsycast = ((curJob != null) ? curJob.verbToUse : null) as Verb_CastPsycast;
			return ((verb_CastPsycast != null) ? verb_CastPsycast.Psycast.def.EntropyGain : 0f) + (from qj in pawn.jobs.jobQueue
			select qj.job.verbToUse).OfType<Verb_CastPsycast>().Sum((Verb_CastPsycast t) => t.Psycast.def.EntropyGain);
		}
	}
}
