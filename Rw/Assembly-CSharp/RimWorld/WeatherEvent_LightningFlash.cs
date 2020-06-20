using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000AB0 RID: 2736
	public class WeatherEvent_LightningFlash : WeatherEvent
	{
		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x060040CB RID: 16587 RVA: 0x0015B36D File Offset: 0x0015956D
		public override bool Expired
		{
			get
			{
				return this.age > this.duration;
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x060040CC RID: 16588 RVA: 0x0015B37D File Offset: 0x0015957D
		public override SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(1f, WeatherEvent_LightningFlash.LightningFlashColors, 1f, 1f);
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x060040CD RID: 16589 RVA: 0x0015B398 File Offset: 0x00159598
		public override Vector2? OverrideShadowVector
		{
			get
			{
				return new Vector2?(this.shadowVector);
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x060040CE RID: 16590 RVA: 0x0015B3A5 File Offset: 0x001595A5
		public override float SkyTargetLerpFactor
		{
			get
			{
				return this.LightningBrightness;
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x060040CF RID: 16591 RVA: 0x0015B3AD File Offset: 0x001595AD
		protected float LightningBrightness
		{
			get
			{
				if (this.age <= 3)
				{
					return (float)this.age / 3f;
				}
				return 1f - (float)this.age / (float)this.duration;
			}
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x0015B3DC File Offset: 0x001595DC
		public WeatherEvent_LightningFlash(Map map) : base(map)
		{
			this.duration = Rand.Range(15, 60);
			this.shadowVector = new Vector2(Rand.Range(-5f, 5f), Rand.Range(-5f, 0f));
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x0015B428 File Offset: 0x00159628
		public override void FireEvent()
		{
			SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(this.map);
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x0015B43A File Offset: 0x0015963A
		public override void WeatherEventTick()
		{
			this.age++;
		}

		// Token: 0x0400259F RID: 9631
		private int duration;

		// Token: 0x040025A0 RID: 9632
		private Vector2 shadowVector;

		// Token: 0x040025A1 RID: 9633
		private int age;

		// Token: 0x040025A2 RID: 9634
		private const int FlashFadeInTicks = 3;

		// Token: 0x040025A3 RID: 9635
		private const int MinFlashDuration = 15;

		// Token: 0x040025A4 RID: 9636
		private const int MaxFlashDuration = 60;

		// Token: 0x040025A5 RID: 9637
		private const float FlashShadowDistance = 5f;

		// Token: 0x040025A6 RID: 9638
		private static readonly SkyColorSet LightningFlashColors = new SkyColorSet(new Color(0.9f, 0.95f, 1f), new Color(0.784313738f, 0.8235294f, 0.847058833f), new Color(0.9f, 0.95f, 1f), 1.15f);
	}
}
