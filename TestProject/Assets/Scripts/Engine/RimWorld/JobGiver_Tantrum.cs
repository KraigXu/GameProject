using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000716 RID: 1814
	public class JobGiver_Tantrum : ThinkNode_JobGiver
	{
		// Token: 0x06002FBC RID: 12220 RVA: 0x0010CC84 File Offset: 0x0010AE84
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_Tantrum mentalState_Tantrum = pawn.MentalState as MentalState_Tantrum;
			if (mentalState_Tantrum == null || mentalState_Tantrum.target == null || !pawn.CanReach(mentalState_Tantrum.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Verb verbToUse = null;
			Pawn pawn2 = mentalState_Tantrum.target as Pawn;
			if (pawn2 != null)
			{
				if (pawn2.Downed)
				{
					return null;
				}
				if (!InteractionUtility.TryGetRandomVerbForSocialFight(pawn, out verbToUse))
				{
					return null;
				}
			}
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, mentalState_Tantrum.target);
			job.maxNumMeleeAttacks = 1;
			job.verbToUse = verbToUse;
			return job;
		}
	}
}
