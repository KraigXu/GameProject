﻿using System;
using UnityEngine;

namespace Verse
{
	
	public class PawnTweener
	{
		
		// (get) Token: 0x06000F16 RID: 3862 RVA: 0x00055E08 File Offset: 0x00054008
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x00055E10 File Offset: 0x00054010
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		
		public PawnTweener(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
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

		
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot();
			this.lastTickSpringPos = this.tweenedPos;
		}

		
		private Vector3 TweenedPosRoot()
		{
			if (!this.pawn.Spawned)
			{
				return this.pawn.Position.ToVector3Shifted();
			}
			float num = this.MovedPercent();
			return this.pawn.pather.nextCell.ToVector3Shifted() * num + this.pawn.Position.ToVector3Shifted() * (1f - num) + PawnCollisionTweenerUtility.PawnCollisionPosOffsetFor(this.pawn);
		}

		
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

		
		private Pawn pawn;

		
		private Vector3 tweenedPos = new Vector3(0f, 0f, 0f);

		
		private int lastDrawFrame = -1;

		
		private Vector3 lastTickSpringPos;

		
		private const float SpringTightness = 0.09f;
	}
}
