using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006A3 RID: 1699
	public abstract class JobGiver_Binge : ThinkNode_JobGiver
	{
		// Token: 0x06002E08 RID: 11784 RVA: 0x001031FB File Offset: 0x001013FB
		protected bool IgnoreForbid(Pawn pawn)
		{
			return pawn.InMentalState;
		}

		// Token: 0x06002E09 RID: 11785
		protected abstract int IngestInterval(Pawn pawn);

		// Token: 0x06002E0A RID: 11786 RVA: 0x00103204 File Offset: 0x00101404
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Find.TickManager.TicksGame - pawn.mindState.lastIngestTick > this.IngestInterval(pawn))
			{
				Job job = this.IngestJob(pawn);
				if (job != null)
				{
					return job;
				}
			}
			return null;
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x00103240 File Offset: 0x00101440
		private Job IngestJob(Pawn pawn)
		{
			Thing thing = this.BestIngestTarget(pawn);
			if (thing == null)
			{
				return null;
			}
			ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(thing, false);
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, thing);
			job.count = finalIngestibleDef.ingestible.maxNumToIngestAtOnce;
			job.ignoreForbidden = this.IgnoreForbid(pawn);
			job.overeat = true;
			return job;
		}

		// Token: 0x06002E0C RID: 11788
		protected abstract Thing BestIngestTarget(Pawn pawn);
	}
}
