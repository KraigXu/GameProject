using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class WeatherEvent_LightningFlash : WeatherEvent
	{
		
		// (get) Token: 0x060040CB RID: 16587 RVA: 0x0015B36D File Offset: 0x0015956D
		public override bool Expired
		{
			get
			{
				return this.age > this.duration;
			}
		}

		
		// (get) Token: 0x060040CC RID: 16588 RVA: 0x0015B37D File Offset: 0x0015957D
		public override SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(1f, WeatherEvent_LightningFlash.LightningFlashColors, 1f, 1f);
			}
		}

		
		// (get) Token: 0x060040CD RID: 16589 RVA: 0x0015B398 File Offset: 0x00159598
		public override Vector2? OverrideShadowVector
		{
			get
			{
				return new Vector2?(this.shadowVector);
			}
		}

		
		// (get) Token: 0x060040CE RID: 16590 RVA: 0x0015B3A5 File Offset: 0x001595A5
		public override float SkyTargetLerpFactor
		{
			get
			{
				return this.LightningBrightness;
			}
		}

		
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

		
		public WeatherEvent_LightningFlash(Map map) : base(map)
		{
			this.duration = Rand.Range(15, 60);
			this.shadowVector = new Vector2(Rand.Range(-5f, 5f), Rand.Range(-5f, 0f));
		}

		
		public override void FireEvent()
		{
			SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(this.map);
		}

		
		public override void WeatherEventTick()
		{
			this.age++;
		}

		
		private int duration;

		
		private Vector2 shadowVector;

		
		private int age;

		
		private const int FlashFadeInTicks = 3;

		
		private const int MinFlashDuration = 15;

		
		private const int MaxFlashDuration = 60;

		
		private const float FlashShadowDistance = 5f;

		
		private static readonly SkyColorSet LightningFlashColors = new SkyColorSet(new Color(0.9f, 0.95f, 1f), new Color(0.784313738f, 0.8235294f, 0.847058833f), new Color(0.9f, 0.95f, 1f), 1.15f);
	}
}
