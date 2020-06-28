using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000319 RID: 793
	public class CompAffectsSky : ThingComp
	{
		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x0600172F RID: 5935 RVA: 0x000851A4 File Offset: 0x000833A4
		public CompProperties_AffectsSky Props
		{
			get
			{
				return (CompProperties_AffectsSky)this.props;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001730 RID: 5936 RVA: 0x000851B4 File Offset: 0x000833B4
		public virtual float LerpFactor
		{
			get
			{
				if (this.HasAutoAnimation)
				{
					int ticksGame = Find.TickManager.TicksGame;
					float num;
					if (ticksGame < this.autoAnimationStartTick + this.fadeInDuration)
					{
						num = (float)(ticksGame - this.autoAnimationStartTick) / (float)this.fadeInDuration;
					}
					else if (ticksGame < this.autoAnimationStartTick + this.fadeInDuration + this.holdDuration)
					{
						num = 1f;
					}
					else
					{
						num = 1f - (float)(ticksGame - this.autoAnimationStartTick - this.fadeInDuration - this.holdDuration) / (float)this.fadeOutDuration;
					}
					return Mathf.Clamp01(num * this.autoAnimationTarget);
				}
				return 0f;
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001731 RID: 5937 RVA: 0x00085253 File Offset: 0x00083453
		public bool HasAutoAnimation
		{
			get
			{
				return Find.TickManager.TicksGame < this.autoAnimationStartTick + this.fadeInDuration + this.holdDuration + this.fadeOutDuration;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001732 RID: 5938 RVA: 0x0008527C File Offset: 0x0008347C
		public virtual SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(this.Props.glow, this.Props.skyColors, this.Props.lightsourceShineSize, this.Props.lightsourceShineIntensity);
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001733 RID: 5939 RVA: 0x000852B0 File Offset: 0x000834B0
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x000852C8 File Offset: 0x000834C8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.autoAnimationStartTick, "autoAnimationStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.fadeInDuration, "fadeInDuration", 0, false);
			Scribe_Values.Look<int>(ref this.holdDuration, "holdDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.autoAnimationTarget, "autoAnimationTarget", 0f, false);
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x00085339 File Offset: 0x00083539
		public void StartFadeInHoldFadeOut(int fadeInDuration, int holdDuration, int fadeOutDuration, float target = 1f)
		{
			this.autoAnimationStartTick = Find.TickManager.TicksGame;
			this.fadeInDuration = fadeInDuration;
			this.holdDuration = holdDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.autoAnimationTarget = target;
		}

		// Token: 0x04000E9B RID: 3739
		private int autoAnimationStartTick;

		// Token: 0x04000E9C RID: 3740
		private int fadeInDuration;

		// Token: 0x04000E9D RID: 3741
		private int holdDuration;

		// Token: 0x04000E9E RID: 3742
		private int fadeOutDuration;

		// Token: 0x04000E9F RID: 3743
		private float autoAnimationTarget;
	}
}
