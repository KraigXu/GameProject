using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006E1 RID: 1761
	public class JobGiver_Meditate : ThinkNode_JobGiver
	{
		// Token: 0x06002EE0 RID: 12000 RVA: 0x001075D0 File Offset: 0x001057D0
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

		// Token: 0x06002EE1 RID: 12001 RVA: 0x00107691 File Offset: 0x00105891
		protected virtual bool ValidatePawnState(Pawn pawn)
		{
			return pawn.CurrentBed() == null && !pawn.health.HasHediffsNeedingTendByPlayer(true) && !pawn.Downed;
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x001076B4 File Offset: 0x001058B4
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
