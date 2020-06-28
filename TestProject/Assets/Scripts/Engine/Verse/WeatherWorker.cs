using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001E7 RID: 487
	public class WeatherWorker
	{
		// Token: 0x06000DB7 RID: 3511 RVA: 0x0004E5C4 File Offset: 0x0004C7C4
		public WeatherWorker(WeatherDef def)
		{
			this.def = def;
			foreach (Type genericParam in def.overlayClasses)
			{
				SkyOverlay item = (SkyOverlay)GenGeneric.InvokeStaticGenericMethod(typeof(WeatherPartPool), genericParam, "GetInstanceOf");
				this.overlays.Add(item);
			}
			this.skyTargets[0] = new WeatherWorker.SkyThreshold(def.skyColorsNightMid, 0f);
			this.skyTargets[1] = new WeatherWorker.SkyThreshold(def.skyColorsNightEdge, 0.1f);
			this.skyTargets[2] = new WeatherWorker.SkyThreshold(def.skyColorsDusk, 0.6f);
			this.skyTargets[3] = new WeatherWorker.SkyThreshold(def.skyColorsDay, 1f);
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x0004E6CC File Offset: 0x0004C8CC
		public void DrawWeather(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0004E704 File Offset: 0x0004C904
		public void WeatherTick(Map map, float lerpFactor)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].TickOverlay(map);
			}
			for (int j = 0; j < this.def.eventMakers.Count; j++)
			{
				this.def.eventMakers[j].WeatherEventMakerTick(map, lerpFactor);
			}
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0004E76C File Offset: 0x0004C96C
		public SkyTarget CurSkyTarget(Map map)
		{
			float num = GenCelestial.CurCelestialSunGlow(map);
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < this.skyTargets.Length; i++)
			{
				num3 = i;
				if (num + 0.001f < this.skyTargets[i].celGlowThreshold)
				{
					break;
				}
				num2 = i;
			}
			WeatherWorker.SkyThreshold skyThreshold = this.skyTargets[num2];
			WeatherWorker.SkyThreshold skyThreshold2 = this.skyTargets[num3];
			float num4 = skyThreshold2.celGlowThreshold - skyThreshold.celGlowThreshold;
			float t;
			if (num4 == 0f)
			{
				t = 1f;
			}
			else
			{
				t = (num - skyThreshold.celGlowThreshold) / num4;
			}
			SkyTarget result = default(SkyTarget);
			result.glow = num;
			result.colors = SkyColorSet.Lerp(skyThreshold.colors, skyThreshold2.colors, t);
			if (GenCelestial.IsDaytime(num))
			{
				result.lightsourceShineIntensity = 1f;
				result.lightsourceShineSize = 1f;
			}
			else
			{
				result.lightsourceShineIntensity = 0.7f;
				result.lightsourceShineSize = 0.5f;
			}
			return result;
		}

		// Token: 0x04000A7E RID: 2686
		private WeatherDef def;

		// Token: 0x04000A7F RID: 2687
		public List<SkyOverlay> overlays = new List<SkyOverlay>();

		// Token: 0x04000A80 RID: 2688
		private WeatherWorker.SkyThreshold[] skyTargets = new WeatherWorker.SkyThreshold[4];

		// Token: 0x020013F4 RID: 5108
		private struct SkyThreshold
		{
			// Token: 0x0600786B RID: 30827 RVA: 0x00293528 File Offset: 0x00291728
			public SkyThreshold(SkyColorSet colors, float celGlowThreshold)
			{
				this.colors = colors;
				this.celGlowThreshold = celGlowThreshold;
			}

			// Token: 0x04004BDC RID: 19420
			public SkyColorSet colors;

			// Token: 0x04004BDD RID: 19421
			public float celGlowThreshold;
		}
	}
}
