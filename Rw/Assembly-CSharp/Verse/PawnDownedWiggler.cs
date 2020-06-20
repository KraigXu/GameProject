using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200021D RID: 541
	public class PawnDownedWiggler
	{
		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x000566E0 File Offset: 0x000548E0
		private static float RandomDownedAngle
		{
			get
			{
				float num = Rand.Range(45f, 135f);
				if (Rand.Value < 0.5f)
				{
					num += 180f;
				}
				return num;
			}
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00056712 File Offset: 0x00054912
		public PawnDownedWiggler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x0005672C File Offset: 0x0005492C
		public void WigglerTick()
		{
			if (this.pawn.Downed && this.pawn.Spawned && !this.pawn.InBed())
			{
				this.ticksToIncapIcon--;
				if (this.ticksToIncapIcon <= 0)
				{
					MoteMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, ThingDefOf.Mote_IncapIcon);
					this.ticksToIncapIcon = 200;
				}
				if (this.pawn.Awake())
				{
					int num = Find.TickManager.TicksGame % 300 * 2;
					if (num < 90)
					{
						this.downedAngle += 0.35f;
						return;
					}
					if (num < 390 && num >= 300)
					{
						this.downedAngle -= 0.35f;
					}
				}
			}
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x00056806 File Offset: 0x00054A06
		public void SetToCustomRotation(float rot)
		{
			this.downedAngle = rot;
			this.usingCustomRotation = true;
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00056818 File Offset: 0x00054A18
		public void Notify_DamageApplied(DamageInfo dam)
		{
			if ((this.pawn.Downed || this.pawn.Dead) && dam.Def.hasForcefulImpact)
			{
				this.downedAngle += 10f * Rand.Range(-1f, 1f);
				if (!this.usingCustomRotation)
				{
					if (this.downedAngle > 315f)
					{
						this.downedAngle = 315f;
					}
					if (this.downedAngle < 45f)
					{
						this.downedAngle = 45f;
					}
					if (this.downedAngle > 135f && this.downedAngle < 225f)
					{
						if (this.downedAngle > 180f)
						{
							this.downedAngle = 225f;
							return;
						}
						this.downedAngle = 135f;
						return;
					}
				}
				else
				{
					if (this.downedAngle >= 360f)
					{
						this.downedAngle -= 360f;
					}
					if (this.downedAngle < 0f)
					{
						this.downedAngle += 360f;
					}
				}
			}
		}

		// Token: 0x04000B40 RID: 2880
		private Pawn pawn;

		// Token: 0x04000B41 RID: 2881
		public float downedAngle = PawnDownedWiggler.RandomDownedAngle;

		// Token: 0x04000B42 RID: 2882
		public int ticksToIncapIcon;

		// Token: 0x04000B43 RID: 2883
		private bool usingCustomRotation;

		// Token: 0x04000B44 RID: 2884
		private const float DownedAngleWidth = 45f;

		// Token: 0x04000B45 RID: 2885
		private const float DamageTakenDownedAngleShift = 10f;

		// Token: 0x04000B46 RID: 2886
		private const int IncapWigglePeriod = 300;

		// Token: 0x04000B47 RID: 2887
		private const int IncapWiggleLength = 90;

		// Token: 0x04000B48 RID: 2888
		private const float IncapWiggleSpeed = 0.35f;

		// Token: 0x04000B49 RID: 2889
		private const int TicksBetweenIncapIcons = 200;
	}
}
