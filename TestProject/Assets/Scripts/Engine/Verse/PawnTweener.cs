using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000218 RID: 536
	public class PawnTweener
	{
		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000F16 RID: 3862 RVA: 0x00055E08 File Offset: 0x00054008
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x00055E10 File Offset: 0x00054010
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x00055E23 File Offset: 0x00054023
		public PawnTweener(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x00055E54 File Offset: 0x00054054
		public void PreDrawPosCalculation()
		{
			if (this.lastDrawFrame == RealTime.frameCount)
			{
				return;
			}
			if (this.lastDrawFrame < RealTime.frameCount - 1)
			{
				this.ResetTweenedPosToRoot();
			}
			else
			{
				this.lastTickSpringPos = this.tweenedPos;
				float tickRateMultiplier = Find.TickManager.TickRateMultiplier;
				if (tickRateMultiplier < 5f)
				{
					Vector3 a = this.TweenedPosRoot() - this.tweenedPos;
					float num = 0.09f * (RealTime.deltaTime * 60f * tickRateMultiplier);
					if (RealTime.deltaTime > 0.05f)
					{
						num = Mathf.Min(num, 1f);
					}
					this.tweenedPos += a * num;
				}
				else
				{
					this.tweenedPos = this.TweenedPosRoot();
				}
			}
			this.lastDrawFrame = RealTime.frameCount;
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x00055F17 File Offset: 0x00054117
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot();
			this.lastTickSpringPos = this.tweenedPos;
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x00055F34 File Offset: 0x00054134
		private Vector3 TweenedPosRoot()
		{
			if (!this.pawn.Spawned)
			{
				return this.pawn.Position.ToVector3Shifted();
			}
			float num = this.MovedPercent();
			return this.pawn.pather.nextCell.ToVector3Shifted() * num + this.pawn.Position.ToVector3Shifted() * (1f - num) + PawnCollisionTweenerUtility.PawnCollisionPosOffsetFor(this.pawn);
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x00055FB8 File Offset: 0x000541B8
		private float MovedPercent()
		{
			if (!this.pawn.pather.Moving)
			{
				return 0f;
			}
			if (this.pawn.stances.FullBodyBusy)
			{
				return 0f;
			}
			if (this.pawn.pather.BuildingBlockingNextPathCell() != null)
			{
				return 0f;
			}
			if (this.pawn.pather.NextCellDoorToWaitForOrManuallyOpen() != null)
			{
				return 0f;
			}
			if (this.pawn.pather.WillCollideWithPawnOnNextPathCell())
			{
				return 0f;
			}
			return 1f - this.pawn.pather.nextCellCostLeft / this.pawn.pather.nextCellCostTotal;
		}

		// Token: 0x04000B31 RID: 2865
		private Pawn pawn;

		// Token: 0x04000B32 RID: 2866
		private Vector3 tweenedPos = new Vector3(0f, 0f, 0f);

		// Token: 0x04000B33 RID: 2867
		private int lastDrawFrame = -1;

		// Token: 0x04000B34 RID: 2868
		private Vector3 lastTickSpringPos;

		// Token: 0x04000B35 RID: 2869
		private const float SpringTightness = 0.09f;
	}
}
