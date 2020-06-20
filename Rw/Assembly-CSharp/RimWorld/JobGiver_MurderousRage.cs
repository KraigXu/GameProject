using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000712 RID: 1810
	public class JobGiver_MurderousRage : ThinkNode_JobGiver
	{
		// Token: 0x06002FB4 RID: 12212 RVA: 0x0010CB00 File Offset: 0x0010AD00
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_MurderousRage mentalState_MurderousRage = pawn.MentalState as MentalState_MurderousRage;
			if (mentalState_MurderousRage == null || !mentalState_MurderousRage.IsTargetStillValidAndReachable())
			{
				return null;
			}
			Thing spawnedParentOrMe = mentalState_MurderousRage.target.SpawnedParentOrMe;
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, spawnedParentOrMe);
			job.canBash = true;
			job.killIncappedTarget = true;
			if (spawnedParentOrMe != mentalState_MurderousRage.target)
			{
				job.maxNumMeleeAttacks = 2;
			}
			return job;
		}
	}
}
