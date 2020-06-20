using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D98 RID: 3480
	public class CompTargetEffect_Resurrect : CompTargetEffect
	{
		// Token: 0x060054A1 RID: 21665 RVA: 0x001C37F4 File Offset: 0x001C19F4
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (!user.IsColonistPlayerControlled)
			{
				return;
			}
			if (!user.CanReserveAndReach(target, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			{
				return;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Resurrect, target, this.parent);
			job.count = 1;
			user.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}
}
