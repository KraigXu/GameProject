using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B2 RID: 1714
	public class JobGiver_AIFollowEscortee : JobGiver_AIFollowPawn
	{
		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06002E50 RID: 11856 RVA: 0x0010444C File Offset: 0x0010264C
		protected override int FollowJobExpireInterval
		{
			get
			{
				return 120;
			}
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x00104450 File Offset: 0x00102650
		protected override Pawn GetFollowee(Pawn pawn)
		{
			return (Pawn)pawn.mindState.duty.focus.Thing;
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x00102550 File Offset: 0x00100750
		protected override float GetRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
