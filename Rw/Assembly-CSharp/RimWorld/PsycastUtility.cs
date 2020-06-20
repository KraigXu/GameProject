using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AE5 RID: 2789
	public class PsycastUtility
	{
		// Token: 0x060041E8 RID: 16872 RVA: 0x0016046C File Offset: 0x0015E66C
		public static float TotalEntropyFromQueuedPsycasts(Pawn pawn)
		{
			Job curJob = pawn.jobs.curJob;
			Verb_CastPsycast verb_CastPsycast = ((curJob != null) ? curJob.verbToUse : null) as Verb_CastPsycast;
			return ((verb_CastPsycast != null) ? verb_CastPsycast.Psycast.def.EntropyGain : 0f) + (from qj in pawn.jobs.jobQueue
			select qj.job.verbToUse).OfType<Verb_CastPsycast>().Sum((Verb_CastPsycast t) => t.Psycast.def.EntropyGain);
		}
	}
}
