using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000215 RID: 533
	public class PawnLeaner
	{
		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000F0B RID: 3851 RVA: 0x00055871 File Offset: 0x00053A71
		public Vector3 LeanOffset
		{
			get
			{
				return this.shootSourceOffset.ToVector3() * 0.5f * this.leanOffsetCurPct;
			}
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x00055893 File Offset: 0x00053A93
		public PawnLeaner(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x000558B0 File Offset: 0x00053AB0
		public void LeanerTick()
		{
			if (this.ShouldLean())
			{
				this.leanOffsetCurPct += 0.075f;
				if (this.leanOffsetCurPct > 1f)
				{
					this.leanOffsetCurPct = 1f;
					return;
				}
			}
			else
			{
				this.leanOffsetCurPct -= 0.075f;
				if (this.leanOffsetCurPct < 0f)
				{
					this.leanOffsetCurPct = 0f;
				}
			}
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0005591A File Offset: 0x00053B1A
		public bool ShouldLean()
		{
			return this.pawn.stances.curStance is Stance_Busy && !(this.shootSourceOffset == new IntVec3(0, 0, 0));
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x0005594D File Offset: 0x00053B4D
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.shootSourceOffset = newShootLine.Source - this.pawn.Position;
		}

		// Token: 0x04000B20 RID: 2848
		private Pawn pawn;

		// Token: 0x04000B21 RID: 2849
		private IntVec3 shootSourceOffset = new IntVec3(0, 0, 0);

		// Token: 0x04000B22 RID: 2850
		private float leanOffsetCurPct;

		// Token: 0x04000B23 RID: 2851
		private const float LeanOffsetPctChangeRate = 0.075f;

		// Token: 0x04000B24 RID: 2852
		private const float LeanOffsetDistanceMultiplier = 0.5f;
	}
}
