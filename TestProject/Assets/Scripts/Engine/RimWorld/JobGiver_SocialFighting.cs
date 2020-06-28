using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000715 RID: 1813
	public class JobGiver_SocialFighting : ThinkNode_JobGiver
	{
		// Token: 0x06002FBA RID: 12218 RVA: 0x0010CC24 File Offset: 0x0010AE24
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike && pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return null;
			}
			Pawn otherPawn = ((MentalState_SocialFighting)pawn.MentalState).otherPawn;
			Verb verbToUse;
			if (!InteractionUtility.TryGetRandomVerbForSocialFight(pawn, out verbToUse))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.SocialFight, otherPawn);
			job.maxNumMeleeAttacks = 1;
			job.verbToUse = verbToUse;
			return job;
		}
	}
}
