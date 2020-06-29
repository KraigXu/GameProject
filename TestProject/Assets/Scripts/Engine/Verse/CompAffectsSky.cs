using System;
using UnityEngine;

namespace Verse
{
	
	public class CompAffectsSky : ThingComp
	{
		
		// (get) Token: 0x0600172F RID: 5935 RVA: 0x000851A4 File Offset: 0x000833A4
		public CompProperties_AffectsSky Props
		{
			get
			{
				return (CompProperties_AffectsSky)this.props;
			}
		}

		
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

		
		// (get) Token: 0x06001731 RID: 5937 RVA: 0x00085253 File Offset: 0x00083453
		public bool HasAutoAnimation
		{
			get
			{
				return Find.TickManager.TicksGame < this.autoAnimationStartTick + this.fadeInDuration + this.holdDuration + this.fadeOutDuration;
			}
		}

		
		// (get) Token: 0x06001732 RID: 5938 RVA: 0x0008527C File Offset: 0x0008347C
		public virtual SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(this.Props.glow, this.Props.skyColors, this.Props.lightsourceShineSize, this.Props.lightsourceShineIntensity);
			}
		}

		
		// (get) Token: 0x06001733 RID: 5939 RVA: 0x000852B0 File Offset: 0x000834B0
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.autoAnimationStartTick, "autoAnimationStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.fadeInDuration, "fadeInDuration", 0, false);
			Scribe_Values.Look<int>(ref this.holdDuration, "holdDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.autoAnimationTarget, "autoAnimationTarget", 0f, false);
		}

		
		public void StartFadeInHoldFadeOut(int fadeInDuration, int holdDuration, int fadeOutDuration, float target = 1f)
		{
			this.autoAnimationStartTick = Find.TickManager.TicksGame;
			this.fadeInDuration = fadeInDuration;
			this.holdDuration = holdDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.autoAnimationTarget = target;
		}

		
		private int autoAnimationStartTick;

		
		private int fadeInDuration;

		
		private int holdDuration;

		
		private int fadeOutDuration;

		
		private float autoAnimationTarget;
	}
}
