﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobGiver_SocialFighting : ThinkNode_JobGiver
	{
		
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
