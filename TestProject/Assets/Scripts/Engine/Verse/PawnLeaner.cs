using System;
using UnityEngine;

namespace Verse
{
	
	public class PawnLeaner
	{
		
		
		public Vector3 LeanOffset
		{
			get
			{
				return this.shootSourceOffset.ToVector3() * 0.5f * this.leanOffsetCurPct;
			}
		}

		
		public PawnLeaner(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
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

		
		public bool ShouldLean()
		{
			return this.pawn.stances.curStance is Stance_Busy && !(this.shootSourceOffset == new IntVec3(0, 0, 0));
		}

		
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.shootSourceOffset = newShootLine.Source - this.pawn.Position;
		}

		
		private Pawn pawn;

		
		private IntVec3 shootSourceOffset = new IntVec3(0, 0, 0);

		
		private float leanOffsetCurPct;

		
		private const float LeanOffsetPctChangeRate = 0.075f;

		
		private const float LeanOffsetDistanceMultiplier = 0.5f;
	}
}
