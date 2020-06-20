using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x020001E2 RID: 482
	public class WindManager
	{
		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000D99 RID: 3481 RVA: 0x0004DEE4 File Offset: 0x0004C0E4
		public float WindSpeed
		{
			get
			{
				return this.cachedWindSpeed;
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0004DEEC File Offset: 0x0004C0EC
		public WindManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0004DEFC File Offset: 0x0004C0FC
		public void WindManagerTick()
		{
			this.cachedWindSpeed = this.BaseWindSpeedAt(Find.TickManager.TicksAbs) * this.map.weatherManager.CurWindSpeedFactor;
			float curWindSpeedOffset = this.map.weatherManager.CurWindSpeedOffset;
			if (curWindSpeedOffset > 0f)
			{
				FloatRange floatRange = WindManager.WindSpeedRange * this.map.weatherManager.CurWindSpeedFactor;
				float num = (this.cachedWindSpeed - floatRange.min) / (floatRange.max - floatRange.min) * (floatRange.max - curWindSpeedOffset);
				this.cachedWindSpeed = curWindSpeedOffset + num;
			}
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.WindSource);
			for (int i = 0; i < list.Count; i++)
			{
				CompWindSource compWindSource = list[i].TryGetComp<CompWindSource>();
				this.cachedWindSpeed = Mathf.Max(this.cachedWindSpeed, compWindSource.wind);
			}
			if (Prefs.PlantWindSway)
			{
				this.plantSwayHead += Mathf.Min(this.WindSpeed, 1f);
			}
			else
			{
				this.plantSwayHead = 0f;
			}
			if (Find.CurrentMap == this.map)
			{
				for (int j = 0; j < WindManager.plantMaterials.Count; j++)
				{
					WindManager.plantMaterials[j].SetFloat(ShaderPropertyIDs.SwayHead, this.plantSwayHead);
				}
			}
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0004E054 File Offset: 0x0004C254
		public static void Notify_PlantMaterialCreated(Material newMat)
		{
			WindManager.plantMaterials.Add(newMat);
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0004E064 File Offset: 0x0004C264
		private float BaseWindSpeedAt(int ticksAbs)
		{
			if (this.windNoise == null)
			{
				int seed = Gen.HashCombineInt(this.map.Tile, 122049541) ^ Find.World.info.Seed;
				this.windNoise = new Perlin(3.9999998989515007E-05, 2.0, 0.5, 4, seed, QualityMode.Medium);
				this.windNoise = new ScaleBias(1.5, 0.5, this.windNoise);
				this.windNoise = new Clamp((double)WindManager.WindSpeedRange.min, (double)WindManager.WindSpeedRange.max, this.windNoise);
			}
			return (float)this.windNoise.GetValue((double)ticksAbs, 0.0, 0.0);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0004E135 File Offset: 0x0004C335
		public string DebugString()
		{
			return string.Concat(new object[]
			{
				"WindSpeed: ",
				this.WindSpeed,
				"\nplantSwayHead: ",
				this.plantSwayHead
			});
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0004E170 File Offset: 0x0004C370
		public void LogWindSpeeds()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Upcoming wind speeds:");
			for (int i = 0; i < 72; i++)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"Hour ",
					i,
					" - ",
					this.BaseWindSpeedAt(Find.TickManager.TicksAbs + 2500 * i).ToString("F2")
				}));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04000A6E RID: 2670
		private static readonly FloatRange WindSpeedRange = new FloatRange(0.04f, 2f);

		// Token: 0x04000A6F RID: 2671
		private Map map;

		// Token: 0x04000A70 RID: 2672
		private static List<Material> plantMaterials = new List<Material>();

		// Token: 0x04000A71 RID: 2673
		private float cachedWindSpeed;

		// Token: 0x04000A72 RID: 2674
		private ModuleBase windNoise;

		// Token: 0x04000A73 RID: 2675
		private float plantSwayHead;
	}
}
