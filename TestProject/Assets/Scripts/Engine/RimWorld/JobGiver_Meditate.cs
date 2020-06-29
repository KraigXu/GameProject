using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobGiver_Meditate : ThinkNode_JobGiver
	{
		
		public override float GetPriority(Pawn pawn)
		{
			Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
			bool flag = pawn.HasPsylink && psychicEntropy != null && psychicEntropy.CurrentPsyfocus < Mathf.Min(psychicEntropy.TargetPsyfocus, 0.95f);
			if (!this.ValidatePawnState(pawn))
			{
				return 0f;
			}
			Pawn_TimetableTracker timetable = pawn.timetable;
			if (((timetable != null) ? timetable.CurrentAssignment : null) == TimeAssignmentDefOf.Meditate)
			{
				return 9f;
			}
			if (pawn.CurrentBed() == null)
			{
				Pawn_TimetableTracker timetable2 = pawn.timetable;
				if (((timetable2 != null) ? timetable2.CurrentAssignment : null) == TimeAssignmentDefOf.Anything && flag)
				{
					return 7.1f;
				}
			}
			else if (flag && pawn.health.hediffSet.PainTotal <= 0.3f && pawn.CurrentBed() != null)
			{
				return 6f;
			}
			return 0f;
		}

		
		protected virtual bool ValidatePawnState(Pawn pawn)
		{
			return pawn.CurrentBed() == null && !pawn.health.HasHediffsNeedingTendByPlayer(true) && !pawn.Downed;
		}

		
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return null;
			}
			if (!MeditationUtility.CanMeditateNow(pawn))
			{
				return null;
			}
			return MeditationUtility.GetMeditationJob(pawn, false);
		}
	}
}
