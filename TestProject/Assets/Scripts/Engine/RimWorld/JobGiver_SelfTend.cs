using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006ED RID: 1773
	public class JobGiver_SelfTend : ThinkNode_JobGiver
	{
		// Token: 0x06002F0A RID: 12042 RVA: 0x00108B58 File Offset: 0x00106D58
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.RaceProps.Humanlike || !pawn.health.HasHediffsNeedingTend(false) || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || pawn.InAggroMentalState)
			{
				return null;
			}
			if (pawn.IsColonist && pawn.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.TendPatient, pawn);
			job.endAfterTendedOnce = true;
			return job;
		}
	}
}
