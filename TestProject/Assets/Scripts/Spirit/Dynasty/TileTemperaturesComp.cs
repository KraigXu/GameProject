using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02001201 RID: 4609
	public class TileTemperaturesComp : WorldComponent
	{
		// Token: 0x06006A8D RID: 27277 RVA: 0x00252AB4 File Offset: 0x00250CB4
		public TileTemperaturesComp(World world) : base(world)
		{
			this.ClearCaches();
		}

		// Token: 0x06006A8E RID: 27278 RVA: 0x00252AC4 File Offset: 0x00250CC4
		public override void WorldComponentTick()
		{
			for (int i = 0; i < this.usedSlots.Count; i++)
			{
				this.cache[this.usedSlots[i]].CheckCache();
			}
			if (Find.TickManager.TicksGame % 300 == 84 && this.usedSlots.Any<int>())
			{
				this.cache[this.usedSlots[0]] = null;
				this.usedSlots.RemoveAt(0);
			}
		}

		// Token: 0x06006A8F RID: 27279 RVA: 0x00252B40 File Offset: 0x00250D40
		public float GetOutdoorTemp(int tile)
		{
			return this.RetrieveCachedData(tile).GetOutdoorTemp();
		}

		// Token: 0x06006A90 RID: 27280 RVA: 0x00252B4E File Offset: 0x00250D4E
		public float GetSeasonalTemp(int tile)
		{
			return this.RetrieveCachedData(tile).GetSeasonalTemp();
		}

		// Token: 0x06006A91 RID: 27281 RVA: 0x00252B5C File Offset: 0x00250D5C
		public float OutdoorTemperatureAt(int tile, int absTick)
		{
			return this.RetrieveCachedData(tile).OutdoorTemperatureAt(absTick);
		}

		// Token: 0x06006A92 RID: 27282 RVA: 0x00252B6B File Offset: 0x00250D6B
		public float OffsetFromDailyRandomVariation(int tile, int absTick)
		{
			return this.RetrieveCachedData(tile).OffsetFromDailyRandomVariation(absTick);
		}

		// Token: 0x06006A93 RID: 27283 RVA: 0x00252B7A File Offset: 0x00250D7A
		public float AverageTemperatureForTwelfth(int tile, Twelfth twelfth)
		{
			return this.RetrieveCachedData(tile).AverageTemperatureForTwelfth(twelfth);
		}

		// Token: 0x06006A94 RID: 27284 RVA: 0x00252B8C File Offset: 0x00250D8C
		public bool SeasonAcceptableFor(int tile, ThingDef animalRace)
		{
			float seasonalTemp = this.GetSeasonalTemp(tile);
			return seasonalTemp > animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) && seasonalTemp < animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null);
		}

		// Token: 0x06006A95 RID: 27285 RVA: 0x00252BC4 File Offset: 0x00250DC4
		public bool OutdoorTemperatureAcceptableFor(int tile, ThingDef animalRace)
		{
			float outdoorTemp = this.GetOutdoorTemp(tile);
			return outdoorTemp > animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) && outdoorTemp < animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null);
		}

		// Token: 0x06006A96 RID: 27286 RVA: 0x00252BF9 File Offset: 0x00250DF9
		public bool SeasonAndOutdoorTemperatureAcceptableFor(int tile, ThingDef animalRace)
		{
			return this.SeasonAcceptableFor(tile, animalRace) && this.OutdoorTemperatureAcceptableFor(tile, animalRace);
		}

		// Token: 0x06006A97 RID: 27287 RVA: 0x00252C0F File Offset: 0x00250E0F
		public void ClearCaches()
		{
			this.cache = new TileTemperaturesComp.CachedTileTemperatureData[Find.WorldGrid.TilesCount];
			this.usedSlots = new List<int>();
		}

		// Token: 0x06006A98 RID: 27288 RVA: 0x00252C31 File Offset: 0x00250E31
		private TileTemperaturesComp.CachedTileTemperatureData RetrieveCachedData(int tile)
		{
			if (this.cache[tile] != null)
			{
				return this.cache[tile];
			}
			this.cache[tile] = new TileTemperaturesComp.CachedTileTemperatureData(tile);
			this.usedSlots.Add(tile);
			return this.cache[tile];
		}

		// Token: 0x04004283 RID: 17027
		private TileTemperaturesComp.CachedTileTemperatureData[] cache;

		// Token: 0x04004284 RID: 17028
		private List<int> usedSlots;

		// Token: 0x02001F90 RID: 8080
		private class CachedTileTemperatureData
		{
			// Token: 0x0600ADF9 RID: 44537 RVA: 0x00324E78 File Offset: 0x00323078
			public CachedTileTemperatureData(int tile)
			{
				this.tile = tile;
				int seed = Gen.HashCombineInt(tile, 199372327);
				this.dailyVariationPerlinCached = new Perlin(4.9999998736893758E-06, 2.0, 0.5, 3, seed, QualityMode.Medium);
				this.twelfthlyTempAverages = new float[12];
				for (int i = 0; i < 12; i++)
				{
					this.twelfthlyTempAverages[i] = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, (Twelfth)i);
				}
				this.CheckCache();
			}

			// Token: 0x0600ADFA RID: 44538 RVA: 0x00324F18 File Offset: 0x00323118
			public float GetOutdoorTemp()
			{
				return this.cachedOutdoorTemp;
			}

			// Token: 0x0600ADFB RID: 44539 RVA: 0x00324F20 File Offset: 0x00323120
			public float GetSeasonalTemp()
			{
				return this.cachedSeasonalTemp;
			}

			// Token: 0x0600ADFC RID: 44540 RVA: 0x00324F28 File Offset: 0x00323128
			public float OutdoorTemperatureAt(int absTick)
			{
				return this.CalculateOutdoorTemperatureAtTile(absTick, true);
			}

			// Token: 0x0600ADFD RID: 44541 RVA: 0x00324F32 File Offset: 0x00323132
			public float OffsetFromDailyRandomVariation(int absTick)
			{
				return (float)this.dailyVariationPerlinCached.GetValue((double)absTick, 0.0, 0.0) * 7f;
			}

			// Token: 0x0600ADFE RID: 44542 RVA: 0x00324F5A File Offset: 0x0032315A
			public float AverageTemperatureForTwelfth(Twelfth twelfth)
			{
				return this.twelfthlyTempAverages[(int)twelfth];
			}

			// Token: 0x0600ADFF RID: 44543 RVA: 0x00324F64 File Offset: 0x00323164
			public void CheckCache()
			{
				if (this.tickCachesNeedReset <= Find.TickManager.TicksGame)
				{
					this.tickCachesNeedReset = Find.TickManager.TicksGame + 60;
					Map map = Current.Game.FindMap(this.tile);
					this.cachedOutdoorTemp = this.OutdoorTemperatureAt(Find.TickManager.TicksAbs);
					if (map != null)
					{
						this.cachedOutdoorTemp += map.gameConditionManager.AggregateTemperatureOffset();
					}
					this.cachedSeasonalTemp = this.CalculateOutdoorTemperatureAtTile(Find.TickManager.TicksAbs, false);
				}
			}

			// Token: 0x0600AE00 RID: 44544 RVA: 0x00324FF0 File Offset: 0x003231F0
			private float CalculateOutdoorTemperatureAtTile(int absTick, bool includeDailyVariations)
			{
				if (absTick == 0)
				{
					absTick = 1;
				}
				float num = Find.WorldGrid[this.tile].temperature + GenTemperature.OffsetFromSeasonCycle(absTick, this.tile);
				if (includeDailyVariations)
				{
					num += this.OffsetFromDailyRandomVariation(absTick) + GenTemperature.OffsetFromSunCycle(absTick, this.tile);
				}
				return num;
			}

			// Token: 0x0400764A RID: 30282
			private int tile;

			// Token: 0x0400764B RID: 30283
			private int tickCachesNeedReset = int.MinValue;

			// Token: 0x0400764C RID: 30284
			private float cachedOutdoorTemp = float.MinValue;

			// Token: 0x0400764D RID: 30285
			private float cachedSeasonalTemp = float.MinValue;

			// Token: 0x0400764E RID: 30286
			private float[] twelfthlyTempAverages;

			// Token: 0x0400764F RID: 30287
			private Perlin dailyVariationPerlinCached;

			// Token: 0x04007650 RID: 30288
			private const int CachedTempUpdateInterval = 60;
		}
	}
}
