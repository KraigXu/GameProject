using System;
using Verse;

namespace RimWorld
{
	
	public class JobGiver_AIFollowEscortee : JobGiver_AIFollowPawn
	{
		
		// (get) Token: 0x06002E50 RID: 11856 RVA: 0x0010444C File Offset: 0x0010264C
		protected override int FollowJobExpireInterval
		{
			get
			{
				return 120;
			}
		}

		
		protected override Pawn GetFollowee(Pawn pawn)
		{
			return (Pawn)pawn.mindState.duty.focus.Thing;
		}

		
		protected override float GetRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
