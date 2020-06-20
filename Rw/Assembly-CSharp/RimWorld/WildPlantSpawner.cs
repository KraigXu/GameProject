using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ABA RID: 2746
	public class WildPlantSpawner : IExposable
	{
		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06004103 RID: 16643 RVA: 0x0015C285 File Offset: 0x0015A485
		public float CurrentPlantDensity
		{
			get
			{
				return this.map.Biome.plantDensity * this.map.gameConditionManager.AggregatePlantDensityFactor(this.map);
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06004104 RID: 16644 RVA: 0x0015C2B0 File Offset: 0x0015A4B0
		public float CurrentWholeMapNumDesiredPlants
		{
			get
			{
				CellRect cellRect = CellRect.WholeMap(this.map);
				float currentPlantDensity = this.CurrentPlantDensity;
				float num = 0f;
				foreach (IntVec3 intVec in cellRect)
				{
					num += this.GetDesiredPlantsCountAt(intVec, intVec, currentPlantDensity);
				}
				return num;
			}
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06004105 RID: 16645 RVA: 0x0015C324 File Offset: 0x0015A524
		public int CurrentWholeMapNumNonZeroFertilityCells
		{
			get
			{
				CellRect cellRect = CellRect.WholeMap(this.map);
				int num = 0;
				using (CellRect.Enumerator enumerator = cellRect.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.GetTerrain(this.map).fertility > 0f)
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06004106 RID: 16646 RVA: 0x0015C398 File Offset: 0x0015A598
		public float CavePlantsCommonalitiesSum
		{
			get
			{
				if (this.cachedCavePlantsCommonalitiesSum == null)
				{
					this.cachedCavePlantsCommonalitiesSum = new float?(0f);
					for (int i = 0; i < WildPlantSpawner.allCavePlants.Count; i++)
					{
						this.cachedCavePlantsCommonalitiesSum += this.GetCommonalityOfPlant(WildPlantSpawner.allCavePlants[i]);
					}
				}
				return this.cachedCavePlantsCommonalitiesSum.Value;
			}
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x0015C423 File Offset: 0x0015A623
		public WildPlantSpawner(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004108 RID: 16648 RVA: 0x0015C432 File Offset: 0x0015A632
		public static void ResetStaticData()
		{
			WildPlantSpawner.allCavePlants.Clear();
			WildPlantSpawner.allCavePlants.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Plant && x.plant.cavePlant
			select x);
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x0015C474 File Offset: 0x0015A674
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.cycleIndex, "cycleIndex", 0, false);
			Scribe_Values.Look<float>(ref this.calculatedWholeMapNumDesiredPlants, "calculatedWholeMapNumDesiredPlants", 0f, false);
			Scribe_Values.Look<float>(ref this.calculatedWholeMapNumDesiredPlantsTmp, "calculatedWholeMapNumDesiredPlantsTmp", 0f, false);
			Scribe_Values.Look<bool>(ref this.hasWholeMapNumDesiredPlantsCalculated, "hasWholeMapNumDesiredPlantsCalculated", true, false);
			Scribe_Values.Look<int>(ref this.calculatedWholeMapNumNonZeroFertilityCells, "calculatedWholeMapNumNonZeroFertilityCells", 0, false);
			Scribe_Values.Look<int>(ref this.calculatedWholeMapNumNonZeroFertilityCellsTmp, "calculatedWholeMapNumNonZeroFertilityCellsTmp", 0, false);
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x0015C4F8 File Offset: 0x0015A6F8
		public void WildPlantSpawnerTick()
		{
			if (DebugSettings.fastEcology || DebugSettings.fastEcologyRegrowRateOnly)
			{
				for (int i = 0; i < 2000; i++)
				{
					this.WildPlantSpawnerTickInternal();
				}
				return;
			}
			this.WildPlantSpawnerTickInternal();
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x0015C530 File Offset: 0x0015A730
		private void WildPlantSpawnerTickInternal()
		{
			int area = this.map.Area;
			int num = Mathf.CeilToInt((float)area * 0.0001f);
			float currentPlantDensity = this.CurrentPlantDensity;
			if (!this.hasWholeMapNumDesiredPlantsCalculated)
			{
				this.calculatedWholeMapNumDesiredPlants = this.CurrentWholeMapNumDesiredPlants;
				this.calculatedWholeMapNumNonZeroFertilityCells = this.CurrentWholeMapNumNonZeroFertilityCells;
				this.hasWholeMapNumDesiredPlantsCalculated = true;
			}
			int num2 = Mathf.CeilToInt(10000f);
			float chance = this.calculatedWholeMapNumDesiredPlants / (float)this.calculatedWholeMapNumNonZeroFertilityCells;
			for (int i = 0; i < num; i++)
			{
				if (this.cycleIndex >= area)
				{
					this.calculatedWholeMapNumDesiredPlants = this.calculatedWholeMapNumDesiredPlantsTmp;
					this.calculatedWholeMapNumDesiredPlantsTmp = 0f;
					this.calculatedWholeMapNumNonZeroFertilityCells = this.calculatedWholeMapNumNonZeroFertilityCellsTmp;
					this.calculatedWholeMapNumNonZeroFertilityCellsTmp = 0;
					this.cycleIndex = 0;
				}
				IntVec3 intVec = this.map.cellsInRandomOrder.Get(this.cycleIndex);
				this.calculatedWholeMapNumDesiredPlantsTmp += this.GetDesiredPlantsCountAt(intVec, intVec, currentPlantDensity);
				if (intVec.GetTerrain(this.map).fertility > 0f)
				{
					this.calculatedWholeMapNumNonZeroFertilityCellsTmp++;
				}
				float mtb = this.GoodRoofForCavePlant(intVec) ? 130f : this.map.Biome.wildPlantRegrowDays;
				if (Rand.Chance(chance) && Rand.MTBEventOccurs(mtb, 60000f, (float)num2) && this.CanRegrowAt(intVec))
				{
					this.CheckSpawnWildPlantAt(intVec, currentPlantDensity, this.calculatedWholeMapNumDesiredPlants, false);
				}
				this.cycleIndex++;
			}
		}

		// Token: 0x0600410C RID: 16652 RVA: 0x0015C6AC File Offset: 0x0015A8AC
		public bool CheckSpawnWildPlantAt(IntVec3 c, float plantDensity, float wholeMapNumDesiredPlants, bool setRandomGrowth = false)
		{
			if (plantDensity <= 0f || c.GetPlant(this.map) != null || c.GetCover(this.map) != null || c.GetEdifice(this.map) != null || this.map.fertilityGrid.FertilityAt(c) <= 0f || !PlantUtility.SnowAllowsPlanting(c, this.map))
			{
				return false;
			}
			bool cavePlants = this.GoodRoofForCavePlant(c);
			if (this.SaturatedAt(c, plantDensity, cavePlants, wholeMapNumDesiredPlants))
			{
				return false;
			}
			this.CalculatePlantsWhichCanGrowAt(c, WildPlantSpawner.tmpPossiblePlants, cavePlants, plantDensity);
			if (!WildPlantSpawner.tmpPossiblePlants.Any<ThingDef>())
			{
				return false;
			}
			this.CalculateDistancesToNearbyClusters(c);
			WildPlantSpawner.tmpPossiblePlantsWithWeight.Clear();
			for (int i = 0; i < WildPlantSpawner.tmpPossiblePlants.Count; i++)
			{
				float value = this.PlantChoiceWeight(WildPlantSpawner.tmpPossiblePlants[i], c, WildPlantSpawner.distanceSqToNearbyClusters, wholeMapNumDesiredPlants, plantDensity);
				WildPlantSpawner.tmpPossiblePlantsWithWeight.Add(new KeyValuePair<ThingDef, float>(WildPlantSpawner.tmpPossiblePlants[i], value));
			}
			KeyValuePair<ThingDef, float> keyValuePair;
			if (!WildPlantSpawner.tmpPossiblePlantsWithWeight.TryRandomElementByWeight((KeyValuePair<ThingDef, float> x) => x.Value, out keyValuePair))
			{
				return false;
			}
			Plant plant = (Plant)ThingMaker.MakeThing(keyValuePair.Key, null);
			if (setRandomGrowth)
			{
				plant.Growth = Rand.Range(0.07f, 1f);
				if (plant.def.plant.LimitedLifespan)
				{
					plant.Age = Rand.Range(0, Mathf.Max(plant.def.plant.LifespanTicks - 50, 0));
				}
			}
			GenSpawn.Spawn(plant, c, this.map, WipeMode.Vanish);
			return true;
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x0015C844 File Offset: 0x0015AA44
		private float PlantChoiceWeight(ThingDef plantDef, IntVec3 c, Dictionary<ThingDef, float> distanceSqToNearbyClusters, float wholeMapNumDesiredPlants, float plantDensity)
		{
			float commonalityOfPlant = this.GetCommonalityOfPlant(plantDef);
			float commonalityPctOfPlant = this.GetCommonalityPctOfPlant(plantDef);
			float num = commonalityOfPlant;
			if (num <= 0f)
			{
				return num;
			}
			float num2 = 0.5f;
			if ((float)this.map.listerThings.ThingsInGroup(ThingRequestGroup.Plant).Count > wholeMapNumDesiredPlants / 2f && !plantDef.plant.cavePlant)
			{
				num2 = (float)this.map.listerThings.ThingsOfDef(plantDef).Count / (float)this.map.listerThings.ThingsInGroup(ThingRequestGroup.Plant).Count / commonalityPctOfPlant;
				num *= WildPlantSpawner.GlobalPctSelectionWeightBias.Evaluate(num2);
			}
			if (plantDef.plant.GrowsInClusters && num2 < 1.1f)
			{
				float num3 = plantDef.plant.cavePlant ? this.CavePlantsCommonalitiesSum : this.map.Biome.PlantCommonalitiesSum;
				float x = commonalityOfPlant * plantDef.plant.wildClusterWeight / (num3 - commonalityOfPlant + commonalityOfPlant * plantDef.plant.wildClusterWeight);
				float num4 = 1f / (3.14159274f * (float)plantDef.plant.wildClusterRadius * (float)plantDef.plant.wildClusterRadius);
				num4 = GenMath.LerpDoubleClamped(commonalityPctOfPlant, 1f, 1f, num4, x);
				float f;
				if (distanceSqToNearbyClusters.TryGetValue(plantDef, out f))
				{
					float x2 = Mathf.Sqrt(f);
					num *= GenMath.LerpDoubleClamped((float)plantDef.plant.wildClusterRadius * 0.9f, (float)plantDef.plant.wildClusterRadius * 1.1f, plantDef.plant.wildClusterWeight, num4, x2);
				}
				else
				{
					num *= num4;
				}
			}
			if (plantDef.plant.wildEqualLocalDistribution)
			{
				float f2 = wholeMapNumDesiredPlants * commonalityPctOfPlant;
				float num5 = (float)Mathf.Max(this.map.Size.x, this.map.Size.z) / Mathf.Sqrt(f2) * 2f;
				if (plantDef.plant.GrowsInClusters)
				{
					num5 = Mathf.Max(num5, (float)plantDef.plant.wildClusterRadius * 1.6f);
				}
				num5 = Mathf.Max(num5, 7f);
				if (num5 <= 25f)
				{
					num *= this.LocalPlantProportionsWeightFactor(c, commonalityPctOfPlant, plantDensity, num5, plantDef);
				}
			}
			return num;
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x0015CA78 File Offset: 0x0015AC78
		private float LocalPlantProportionsWeightFactor(IntVec3 c, float commonalityPct, float plantDensity, float radiusToScan, ThingDef plantDef)
		{
			float numDesiredPlantsLocally = 0f;
			int numPlants = 0;
			int numPlantsThisDef = 0;
			RegionTraverser.BreadthFirstTraverse(c, this.map, (Region from, Region to) => c.InHorDistOf(to.extentsClose.ClosestCellTo(c), radiusToScan), delegate(Region reg)
			{
				numDesiredPlantsLocally += this.GetDesiredPlantsCountIn(reg, c, plantDensity);
				numPlants += reg.ListerThings.ThingsInGroup(ThingRequestGroup.Plant).Count;
				numPlantsThisDef += reg.ListerThings.ThingsOfDef(plantDef).Count;
				return false;
			}, 999999, RegionType.Set_Passable);
			if (numDesiredPlantsLocally * commonalityPct < 2f)
			{
				return 1f;
			}
			if ((float)numPlants <= numDesiredPlantsLocally * 0.5f)
			{
				return 1f;
			}
			float t = (float)numPlantsThisDef / (float)numPlants / commonalityPct;
			return Mathf.Lerp(7f, 1f, t);
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x0015CB4C File Offset: 0x0015AD4C
		private void CalculatePlantsWhichCanGrowAt(IntVec3 c, List<ThingDef> outPlants, bool cavePlants, float plantDensity)
		{
			outPlants.Clear();
			if (cavePlants)
			{
				for (int i = 0; i < WildPlantSpawner.allCavePlants.Count; i++)
				{
					if (WildPlantSpawner.allCavePlants[i].CanEverPlantAt_NewTemp(c, this.map, false))
					{
						outPlants.Add(WildPlantSpawner.allCavePlants[i]);
					}
				}
				return;
			}
			List<ThingDef> allWildPlants = this.map.Biome.AllWildPlants;
			for (int j = 0; j < allWildPlants.Count; j++)
			{
				ThingDef thingDef = allWildPlants[j];
				if (thingDef.CanEverPlantAt_NewTemp(c, this.map, false))
				{
					if (thingDef.plant.wildOrder != this.map.Biome.LowestWildAndCavePlantOrder)
					{
						float num = 7f;
						if (thingDef.plant.GrowsInClusters)
						{
							num = Math.Max(num, (float)thingDef.plant.wildClusterRadius * 1.5f);
						}
						if (!this.EnoughLowerOrderPlantsNearby(c, plantDensity, num, thingDef))
						{
							goto IL_D8;
						}
					}
					outPlants.Add(thingDef);
				}
				IL_D8:;
			}
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x0015CC44 File Offset: 0x0015AE44
		private bool EnoughLowerOrderPlantsNearby(IntVec3 c, float plantDensity, float radiusToScan, ThingDef plantDef)
		{
			float num = 0f;
			WildPlantSpawner.tmpPlantDefsLowerOrder.Clear();
			List<ThingDef> allWildPlants = this.map.Biome.AllWildPlants;
			for (int i = 0; i < allWildPlants.Count; i++)
			{
				if (allWildPlants[i].plant.wildOrder < plantDef.plant.wildOrder)
				{
					num += this.GetCommonalityPctOfPlant(allWildPlants[i]);
					WildPlantSpawner.tmpPlantDefsLowerOrder.Add(allWildPlants[i]);
				}
			}
			float numDesiredPlantsLocally = 0f;
			int numPlantsLowerOrder = 0;
			RegionTraverser.BreadthFirstTraverse(c, this.map, (Region from, Region to) => c.InHorDistOf(to.extentsClose.ClosestCellTo(c), radiusToScan), delegate(Region reg)
			{
				numDesiredPlantsLocally += this.GetDesiredPlantsCountIn(reg, c, plantDensity);
				for (int j = 0; j < WildPlantSpawner.tmpPlantDefsLowerOrder.Count; j++)
				{
					numPlantsLowerOrder += reg.ListerThings.ThingsOfDef(WildPlantSpawner.tmpPlantDefsLowerOrder[j]).Count;
				}
				return false;
			}, 999999, RegionType.Set_Passable);
			float num2 = numDesiredPlantsLocally * num;
			return num2 < 4f || (float)numPlantsLowerOrder / num2 >= 0.57f;
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x0015CD54 File Offset: 0x0015AF54
		private bool SaturatedAt(IntVec3 c, float plantDensity, bool cavePlants, float wholeMapNumDesiredPlants)
		{
			int num = GenRadial.NumCellsInRadius(20f);
			if (wholeMapNumDesiredPlants * ((float)num / (float)this.map.Area) <= 4f || !this.map.Biome.wildPlantsCareAboutLocalFertility)
			{
				return (float)this.map.listerThings.ThingsInGroup(ThingRequestGroup.Plant).Count >= wholeMapNumDesiredPlants;
			}
			float numDesiredPlantsLocally = 0f;
			int numPlants = 0;
			RegionTraverser.BreadthFirstTraverse(c, this.map, (Region from, Region to) => c.InHorDistOf(to.extentsClose.ClosestCellTo(c), 20f), delegate(Region reg)
			{
				numDesiredPlantsLocally += this.GetDesiredPlantsCountIn(reg, c, plantDensity);
				numPlants += reg.ListerThings.ThingsInGroup(ThingRequestGroup.Plant).Count;
				return false;
			}, 999999, RegionType.Set_Passable);
			return (float)numPlants >= numDesiredPlantsLocally;
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x0015CE28 File Offset: 0x0015B028
		private void CalculateDistancesToNearbyClusters(IntVec3 c)
		{
			WildPlantSpawner.nearbyClusters.Clear();
			WildPlantSpawner.nearbyClustersList.Clear();
			int num = GenRadial.NumCellsInRadius((float)(this.map.Biome.MaxWildAndCavePlantsClusterRadius * 2));
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = c + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.map))
				{
					List<Thing> list = this.map.thingGrid.ThingsListAtFast(intVec);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						if (thing.def.category == ThingCategory.Plant && thing.def.plant.GrowsInClusters)
						{
							float item = (float)intVec.DistanceToSquared(c);
							List<float> list2;
							if (!WildPlantSpawner.nearbyClusters.TryGetValue(thing.def, out list2))
							{
								list2 = SimplePool<List<float>>.Get();
								WildPlantSpawner.nearbyClusters.Add(thing.def, list2);
								WildPlantSpawner.nearbyClustersList.Add(new KeyValuePair<ThingDef, List<float>>(thing.def, list2));
							}
							list2.Add(item);
						}
					}
				}
			}
			WildPlantSpawner.distanceSqToNearbyClusters.Clear();
			for (int k = 0; k < WildPlantSpawner.nearbyClustersList.Count; k++)
			{
				List<float> value = WildPlantSpawner.nearbyClustersList[k].Value;
				value.Sort();
				WildPlantSpawner.distanceSqToNearbyClusters.Add(WildPlantSpawner.nearbyClustersList[k].Key, value[value.Count / 2]);
				value.Clear();
				SimplePool<List<float>>.Return(value);
			}
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x0015CFC8 File Offset: 0x0015B1C8
		private bool CanRegrowAt(IntVec3 c)
		{
			return c.GetTemperature(this.map) > 0f && (!c.Roofed(this.map) || this.GoodRoofForCavePlant(c));
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x0015CFF8 File Offset: 0x0015B1F8
		private bool GoodRoofForCavePlant(IntVec3 c)
		{
			RoofDef roof = c.GetRoof(this.map);
			return roof != null && roof.isNatural;
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x0015D01D File Offset: 0x0015B21D
		private float GetCommonalityOfPlant(ThingDef plant)
		{
			if (!plant.plant.cavePlant)
			{
				return this.map.Biome.CommonalityOfPlant(plant);
			}
			return plant.plant.cavePlantWeight;
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x0015D049 File Offset: 0x0015B249
		private float GetCommonalityPctOfPlant(ThingDef plant)
		{
			if (!plant.plant.cavePlant)
			{
				return this.map.Biome.CommonalityPctOfPlant(plant);
			}
			return this.GetCommonalityOfPlant(plant) / this.CavePlantsCommonalitiesSum;
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x0015D078 File Offset: 0x0015B278
		public float GetBaseDesiredPlantsCountAt(IntVec3 c)
		{
			float num = c.GetTerrain(this.map).fertility;
			if (this.GoodRoofForCavePlant(c))
			{
				num *= 0.5f;
			}
			return num;
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x0015D0A9 File Offset: 0x0015B2A9
		public float GetDesiredPlantsCountAt(IntVec3 c, IntVec3 forCell, float plantDensity)
		{
			return Mathf.Min(this.GetBaseDesiredPlantsCountAt(c) * plantDensity * forCell.GetTerrain(this.map).fertility, 1f);
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x0015D0D0 File Offset: 0x0015B2D0
		public float GetDesiredPlantsCountIn(Region reg, IntVec3 forCell, float plantDensity)
		{
			return Mathf.Min(reg.GetBaseDesiredPlantsCount(true) * plantDensity * forCell.GetTerrain(this.map).fertility, (float)reg.CellCount);
		}

		// Token: 0x040025BB RID: 9659
		private Map map;

		// Token: 0x040025BC RID: 9660
		private int cycleIndex;

		// Token: 0x040025BD RID: 9661
		private float calculatedWholeMapNumDesiredPlants;

		// Token: 0x040025BE RID: 9662
		private float calculatedWholeMapNumDesiredPlantsTmp;

		// Token: 0x040025BF RID: 9663
		private int calculatedWholeMapNumNonZeroFertilityCells;

		// Token: 0x040025C0 RID: 9664
		private int calculatedWholeMapNumNonZeroFertilityCellsTmp;

		// Token: 0x040025C1 RID: 9665
		private bool hasWholeMapNumDesiredPlantsCalculated;

		// Token: 0x040025C2 RID: 9666
		private float? cachedCavePlantsCommonalitiesSum;

		// Token: 0x040025C3 RID: 9667
		private static List<ThingDef> allCavePlants = new List<ThingDef>();

		// Token: 0x040025C4 RID: 9668
		private static List<ThingDef> tmpPossiblePlants = new List<ThingDef>();

		// Token: 0x040025C5 RID: 9669
		private static List<KeyValuePair<ThingDef, float>> tmpPossiblePlantsWithWeight = new List<KeyValuePair<ThingDef, float>>();

		// Token: 0x040025C6 RID: 9670
		private static Dictionary<ThingDef, float> distanceSqToNearbyClusters = new Dictionary<ThingDef, float>();

		// Token: 0x040025C7 RID: 9671
		private static Dictionary<ThingDef, List<float>> nearbyClusters = new Dictionary<ThingDef, List<float>>();

		// Token: 0x040025C8 RID: 9672
		private static List<KeyValuePair<ThingDef, List<float>>> nearbyClustersList = new List<KeyValuePair<ThingDef, List<float>>>();

		// Token: 0x040025C9 RID: 9673
		private const float CavePlantsDensityFactor = 0.5f;

		// Token: 0x040025CA RID: 9674
		private const int PlantSaturationScanRadius = 20;

		// Token: 0x040025CB RID: 9675
		private const float MapFractionCheckPerTick = 0.0001f;

		// Token: 0x040025CC RID: 9676
		private const float ChanceToRegrow = 0.012f;

		// Token: 0x040025CD RID: 9677
		private const float CavePlantChanceToRegrow = 0.0001f;

		// Token: 0x040025CE RID: 9678
		private const float BaseLowerOrderScanRadius = 7f;

		// Token: 0x040025CF RID: 9679
		private const float LowerOrderScanRadiusWildClusterRadiusFactor = 1.5f;

		// Token: 0x040025D0 RID: 9680
		private const float MinDesiredLowerOrderPlantsToConsiderSkipping = 4f;

		// Token: 0x040025D1 RID: 9681
		private const float MinLowerOrderPlantsPct = 0.57f;

		// Token: 0x040025D2 RID: 9682
		private const float LocalPlantProportionsMaxScanRadius = 25f;

		// Token: 0x040025D3 RID: 9683
		private const float MaxLocalProportionsBias = 7f;

		// Token: 0x040025D4 RID: 9684
		private const float CavePlantRegrowDays = 130f;

		// Token: 0x040025D5 RID: 9685
		private static readonly SimpleCurve GlobalPctSelectionWeightBias = new SimpleCurve
		{
			{
				new CurvePoint(0f, 3f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(1.5f, 0.25f),
				true
			},
			{
				new CurvePoint(3f, 0.02f),
				true
			}
		};

		// Token: 0x040025D6 RID: 9686
		private static List<ThingDef> tmpPlantDefsLowerOrder = new List<ThingDef>();
	}
}
