using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001025 RID: 4133
	public class SteadyEnvironmentEffects
	{
		// Token: 0x060062EB RID: 25323 RVA: 0x0022598E File Offset: 0x00223B8E
		public SteadyEnvironmentEffects(Map map)
		{
			this.map = map;
		}

		// Token: 0x060062EC RID: 25324 RVA: 0x002259A0 File Offset: 0x00223BA0
		public void SteadyEnvironmentEffectsTick()
		{
			if ((float)Find.TickManager.TicksGame % 97f == 0f && Rand.Chance(0.02f))
			{
				this.RollForRainFire();
			}
			this.outdoorMeltAmount = this.MeltAmountAt(this.map.mapTemperature.OutdoorTemp);
			this.snowRate = this.map.weatherManager.SnowRate;
			this.rainRate = this.map.weatherManager.RainRate;
			this.deteriorationRate = Mathf.Lerp(1f, 5f, this.rainRate);
			int num = Mathf.CeilToInt((float)this.map.Area * 0.0006f);
			int area = this.map.Area;
			for (int i = 0; i < num; i++)
			{
				if (this.cycleIndex >= area)
				{
					this.cycleIndex = 0;
				}
				IntVec3 c = this.map.cellsInRandomOrder.Get(this.cycleIndex);
				this.DoCellSteadyEffects(c);
				this.cycleIndex++;
			}
		}

		// Token: 0x060062ED RID: 25325 RVA: 0x00225AA8 File Offset: 0x00223CA8
		private void DoCellSteadyEffects(IntVec3 c)
		{
			Room room = c.GetRoom(this.map, RegionType.Set_All);
			bool flag = this.map.roofGrid.Roofed(c);
			bool flag2 = room != null && room.UsesOutdoorTemperature;
			if (room == null || flag2)
			{
				if (this.outdoorMeltAmount > 0f)
				{
					this.map.snowGrid.AddDepth(c, -this.outdoorMeltAmount);
				}
				if (!flag && this.snowRate > 0.001f)
				{
					this.AddFallenSnowAt(c, 0.046f * this.map.weatherManager.SnowRate);
				}
			}
			if (room != null)
			{
				bool protectedByEdifice = SteadyEnvironmentEffects.ProtectedByEdifice(c, this.map);
				TerrainDef terrain = c.GetTerrain(this.map);
				List<Thing> thingList = c.GetThingList(this.map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					Filth filth = thing as Filth;
					if (filth != null)
					{
						if (!flag && thing.def.filth.rainWashes && Rand.Chance(this.rainRate))
						{
							filth.ThinFilth();
						}
						if (filth.DisappearAfterTicks != 0 && filth.TicksSinceThickened > filth.DisappearAfterTicks)
						{
							filth.Destroy(DestroyMode.Vanish);
						}
					}
					else
					{
						this.TryDoDeteriorate(thing, flag, flag2, protectedByEdifice, terrain);
					}
				}
				if (!flag2)
				{
					float temperature = room.Temperature;
					if (temperature > 0f)
					{
						float num = this.MeltAmountAt(temperature);
						if (num > 0f)
						{
							this.map.snowGrid.AddDepth(c, -num);
						}
						if (room.RegionType.Passable() && temperature > SteadyEnvironmentEffects.AutoIgnitionTemperatureRange.min)
						{
							float value = Rand.Value;
							if (value < SteadyEnvironmentEffects.AutoIgnitionTemperatureRange.InverseLerpThroughRange(temperature) * 0.7f && Rand.Chance(FireUtility.ChanceToStartFireIn(c, this.map)))
							{
								FireUtility.TryStartFireIn(c, this.map, 0.1f);
							}
							if (value < 0.33f)
							{
								MoteMaker.ThrowHeatGlow(c, this.map, 2.3f);
							}
						}
					}
				}
			}
			this.map.gameConditionManager.DoSteadyEffects(c, this.map);
		}

		// Token: 0x060062EE RID: 25326 RVA: 0x00225CC4 File Offset: 0x00223EC4
		private static bool ProtectedByEdifice(IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.building != null && edifice.def.building.preventDeteriorationOnTop;
		}

		// Token: 0x060062EF RID: 25327 RVA: 0x00225CFE File Offset: 0x00223EFE
		private float MeltAmountAt(float temperature)
		{
			if (temperature < 0f)
			{
				return 0f;
			}
			if (temperature < 10f)
			{
				return temperature * temperature * 0.0058f * 0.1f;
			}
			return temperature * 0.0058f;
		}

		// Token: 0x060062F0 RID: 25328 RVA: 0x00225D30 File Offset: 0x00223F30
		public void AddFallenSnowAt(IntVec3 c, float baseAmount)
		{
			if (this.snowNoise == null)
			{
				this.snowNoise = new Perlin(0.039999999105930328, 2.0, 0.5, 5, Rand.Range(0, 651431), QualityMode.Medium);
			}
			float num = this.snowNoise.GetValue(c);
			num += 1f;
			num *= 0.5f;
			if (num < 0.5f)
			{
				num = 0.5f;
			}
			float depthToAdd = baseAmount * num;
			this.map.snowGrid.AddDepth(c, depthToAdd);
		}

		// Token: 0x060062F1 RID: 25329 RVA: 0x00225DBC File Offset: 0x00223FBC
		public static float FinalDeteriorationRate(Thing t, List<string> reasons = null)
		{
			if (t.Spawned)
			{
				Room room = t.GetRoom(RegionType.Set_Passable);
				return SteadyEnvironmentEffects.FinalDeteriorationRate(t, t.Position.Roofed(t.Map), room != null && room.UsesOutdoorTemperature, SteadyEnvironmentEffects.ProtectedByEdifice(t.Position, t.Map), t.Position.GetTerrain(t.Map), reasons);
			}
			return SteadyEnvironmentEffects.FinalDeteriorationRate(t, false, false, false, null, reasons);
		}

		// Token: 0x060062F2 RID: 25330 RVA: 0x00225E2C File Offset: 0x0022402C
		public static float FinalDeteriorationRate(Thing t, bool roofed, bool roomUsesOutdoorTemperature, bool protectedByEdifice, TerrainDef terrain, List<string> reasons = null)
		{
			if (!t.def.CanEverDeteriorate)
			{
				return 0f;
			}
			if (protectedByEdifice)
			{
				return 0f;
			}
			float statValue = t.GetStatValue(StatDefOf.DeteriorationRate, true);
			if (statValue <= 0f)
			{
				return 0f;
			}
			float num = 0f;
			if (!roofed)
			{
				num += 0.5f;
				if (reasons != null)
				{
					reasons.Add("DeterioratingUnroofed".Translate());
				}
			}
			if (roomUsesOutdoorTemperature)
			{
				num += 0.5f;
				if (reasons != null)
				{
					reasons.Add("DeterioratingOutdoors".Translate());
				}
			}
			if (terrain != null && terrain.extraDeteriorationFactor != 0f)
			{
				num += terrain.extraDeteriorationFactor;
				if (reasons != null)
				{
					reasons.Add(terrain.label);
				}
			}
			if (num <= 0f)
			{
				return 0f;
			}
			return statValue * num;
		}

		// Token: 0x060062F3 RID: 25331 RVA: 0x00225F00 File Offset: 0x00224100
		private void TryDoDeteriorate(Thing t, bool roofed, bool roomUsesOutdoorTemperature, bool protectedByEdifice, TerrainDef terrain)
		{
			Corpse corpse = t as Corpse;
			if (corpse != null && corpse.InnerPawn.apparel != null)
			{
				List<Apparel> wornApparel = corpse.InnerPawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					this.TryDoDeteriorate(wornApparel[i], roofed, roomUsesOutdoorTemperature, protectedByEdifice, terrain);
				}
			}
			float num = SteadyEnvironmentEffects.FinalDeteriorationRate(t, roofed, roomUsesOutdoorTemperature, protectedByEdifice, terrain, null);
			if (num < 0.001f)
			{
				return;
			}
			if (Rand.Chance(this.deteriorationRate * num / 36f))
			{
				IntVec3 position = t.Position;
				Map map = t.Map;
				bool flag = t.IsInAnyStorage();
				t.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 1f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				if (flag && t.Destroyed && t.def.messageOnDeteriorateInStorage)
				{
					Messages.Message("MessageDeterioratedAway".Translate(t.Label), new TargetInfo(position, map, false), MessageTypeDefOf.NegativeEvent, true);
				}
			}
		}

		// Token: 0x060062F4 RID: 25332 RVA: 0x00226010 File Offset: 0x00224210
		private void RollForRainFire()
		{
			if (!Rand.Chance(0.2f * (float)this.map.listerBuildings.allBuildingsColonistElecFire.Count * this.map.weatherManager.RainRate))
			{
				return;
			}
			Building building = this.map.listerBuildings.allBuildingsColonistElecFire.RandomElement<Building>();
			if (!this.map.roofGrid.Roofed(building.Position))
			{
				ShortCircuitUtility.TryShortCircuitInRain(building);
			}
		}

		// Token: 0x04003C0D RID: 15373
		private Map map;

		// Token: 0x04003C0E RID: 15374
		private ModuleBase snowNoise;

		// Token: 0x04003C0F RID: 15375
		private int cycleIndex;

		// Token: 0x04003C10 RID: 15376
		private float outdoorMeltAmount;

		// Token: 0x04003C11 RID: 15377
		private float snowRate;

		// Token: 0x04003C12 RID: 15378
		private float rainRate;

		// Token: 0x04003C13 RID: 15379
		private float deteriorationRate;

		// Token: 0x04003C14 RID: 15380
		private const float MapFractionCheckPerTick = 0.0006f;

		// Token: 0x04003C15 RID: 15381
		private const float RainFireCheckInterval = 97f;

		// Token: 0x04003C16 RID: 15382
		private const float RainFireChanceOverall = 0.02f;

		// Token: 0x04003C17 RID: 15383
		private const float RainFireChancePerBuilding = 0.2f;

		// Token: 0x04003C18 RID: 15384
		private const float SnowFallRateFactor = 0.046f;

		// Token: 0x04003C19 RID: 15385
		private const float SnowMeltRateFactor = 0.0058f;

		// Token: 0x04003C1A RID: 15386
		private static readonly FloatRange AutoIgnitionTemperatureRange = new FloatRange(240f, 1000f);

		// Token: 0x04003C1B RID: 15387
		private const float AutoIgnitionChanceFactor = 0.7f;

		// Token: 0x04003C1C RID: 15388
		private const float FireGlowRate = 0.33f;
	}
}
