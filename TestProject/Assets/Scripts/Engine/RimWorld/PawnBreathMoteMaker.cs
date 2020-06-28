using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AEA RID: 2794
	public class PawnBreathMoteMaker
	{
		// Token: 0x06004207 RID: 16903 RVA: 0x00160C9B File Offset: 0x0015EE9B
		public PawnBreathMoteMaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x00160CAC File Offset: 0x0015EEAC
		public void BreathMoteMakerTick()
		{
			if (!this.pawn.RaceProps.Humanlike || this.pawn.RaceProps.IsMechanoid)
			{
				return;
			}
			int num = Mathf.Abs(Find.TickManager.TicksGame + this.pawn.HashOffset()) % 320;
			if (num == 0)
			{
				this.doThisBreath = (this.pawn.AmbientTemperature < 0f && this.pawn.GetPosture() == PawnPosture.Standing);
			}
			if (this.doThisBreath && num < 80 && num % 8 == 0)
			{
				this.TryMakeBreathMote();
			}
		}

		// Token: 0x06004209 RID: 16905 RVA: 0x00160D44 File Offset: 0x0015EF44
		private void TryMakeBreathMote()
		{
			Vector3 loc = this.pawn.Drawer.DrawPos + this.pawn.Drawer.renderer.BaseHeadOffsetAt(this.pawn.Rotation) + this.pawn.Rotation.FacingCell.ToVector3() * 0.21f + PawnBreathMoteMaker.BreathOffset;
			Vector3 lastTickTweenedVelocity = this.pawn.Drawer.tweener.LastTickTweenedVelocity;
			MoteMaker.ThrowBreathPuff(loc, this.pawn.Map, this.pawn.Rotation.AsAngle, lastTickTweenedVelocity);
		}

		// Token: 0x04002620 RID: 9760
		private Pawn pawn;

		// Token: 0x04002621 RID: 9761
		private bool doThisBreath;

		// Token: 0x04002622 RID: 9762
		private const int BreathDuration = 80;

		// Token: 0x04002623 RID: 9763
		private const int BreathInterval = 320;

		// Token: 0x04002624 RID: 9764
		private const int MoteInterval = 8;

		// Token: 0x04002625 RID: 9765
		private const float MaxBreathTemperature = 0f;

		// Token: 0x04002626 RID: 9766
		private static readonly Vector3 BreathOffset = new Vector3(0f, 0f, -0.04f);

		// Token: 0x04002627 RID: 9767
		private const float BreathRotationOffsetDist = 0.21f;
	}
}
