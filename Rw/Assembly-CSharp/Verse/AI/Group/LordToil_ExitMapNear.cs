using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020005D9 RID: 1497
	public class LordToil_ExitMapNear : LordToil
	{
		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x060029B0 RID: 10672 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x060029B1 RID: 10673 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x000F4EFE File Offset: 0x000F30FE
		public LordToil_ExitMapNear(IntVec3 near, float radius, LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false)
		{
			this.near = near;
			this.radius = radius;
			this.locomotion = locomotion;
			this.canDig = canDig;
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x000F4F24 File Offset: 0x000F3124
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.ExitMapNearDutyTarget, this.near, this.radius);
				pawnDuty.locomotion = this.locomotion;
				pawnDuty.canDig = this.canDig;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x040018FE RID: 6398
		private IntVec3 near;

		// Token: 0x040018FF RID: 6399
		private float radius;

		// Token: 0x04001900 RID: 6400
		private LocomotionUrgency locomotion;

		// Token: 0x04001901 RID: 6401
		private bool canDig;
	}
}
