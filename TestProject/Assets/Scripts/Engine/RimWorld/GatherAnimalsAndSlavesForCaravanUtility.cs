using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000785 RID: 1925
	public static class GatherAnimalsAndSlavesForCaravanUtility
	{
		// Token: 0x06003254 RID: 12884 RVA: 0x00118683 File Offset: 0x00116883
		public static bool IsFollowingAnyone(Pawn p)
		{
			return p.mindState.duty.focus.HasThing;
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x0011869A File Offset: 0x0011689A
		public static void SetFollower(Pawn p, Pawn follower)
		{
			p.mindState.duty.focus = follower;
			p.mindState.duty.radius = 10f;
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x001186C8 File Offset: 0x001168C8
		public static void CheckArrived(Lord lord, List<Pawn> pawns, IntVec3 meetingPoint, string memo, Predicate<Pawn> shouldCheckIfArrived, Predicate<Pawn> extraValidator = null)
		{
			bool flag = true;
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (shouldCheckIfArrived(pawn) && (!pawn.Spawned || !pawn.Position.InHorDistOf(meetingPoint, 10f) || !pawn.CanReach(meetingPoint, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn) || (extraValidator != null && !extraValidator(pawn))))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				lord.ReceiveMemo(memo);
			}
		}
	}
}
