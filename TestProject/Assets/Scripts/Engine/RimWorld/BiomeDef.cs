using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B6 RID: 2230
	public class BiomeDef : Def
	{
		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x060035C2 RID: 13762 RVA: 0x001242D5 File Offset: 0x001224D5
		public BiomeWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (BiomeWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x060035C3 RID: 13763 RVA: 0x001242FC File Offset: 0x001224FC
		public Material DrawMaterial
		{
			get
			{
				if (this.cachedMat == null)
				{
					if (this.texture.NullOrEmpty())
					{
						return null;
					}
					if (this == BiomeDefOf.Ocean || this == BiomeDefOf.Lake)
					{
						this.cachedMat = MaterialAllocator.Create(WorldMaterials.WorldOcean);
					}
					else if (!this.allowRoads && !this.allowRivers)
					{
						this.cachedMat = MaterialAllocator.Create(WorldMaterials.WorldIce);
					}
					else
					{
						this.cachedMat = MaterialAllocator.Create(WorldMaterials.WorldTerrain);
					}
					this.cachedMat.mainTexture = ContentFinder<Texture2D>.Get(this.texture, true);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x060035C4 RID: 13764 RVA: 0x00124398 File Offset: 0x00122598
		public List<ThingDef> AllWildPlants
		{
			get
			{
				if (this.cachedWildPlants == null)
				{
					this.cachedWildPlants = new List<ThingDef>();
					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
					{
						if (thingDef.category == ThingCategory.Plant && this.CommonalityOfPlant(thingDef) > 0f)
						{
							this.cachedWildPlants.Add(thingDef);
						}
					}
				}
				return this.cachedWildPlants;
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x060035C5 RID: 13765 RVA: 0x00124420 File Offset: 0x00122620
		public int MaxWildAndCavePlantsClusterRadius
		{
			get
			{
				if (this.cachedMaxWildPlantsClusterRadius == null)
				{
					this.cachedMaxWildPlantsClusterRadius = new int?(0);
					List<ThingDef> allWildPlants = this.AllWildPlants;
					for (int i = 0; i < allWildPlants.Count; i++)
					{
						if (allWildPlants[i].plant.GrowsInClusters)
						{
							this.cachedMaxWildPlantsClusterRadius = new int?(Mathf.Max(this.cachedMaxWildPlantsClusterRadius.Value, allWildPlants[i].plant.wildClusterRadius));
						}
					}
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].category == ThingCategory.Plant && allDefsListForReading[j].plant.cavePlant)
						{
							this.cachedMaxWildPlantsClusterRadius = new int?(Mathf.Max(this.cachedMaxWildPlantsClusterRadius.Value, allDefsListForReading[j].plant.wildClusterRadius));
						}
					}
				}
				return this.cachedMaxWildPlantsClusterRadius.Value;
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x060035C6 RID: 13766 RVA: 0x00124510 File Offset: 0x00122710
		public float LowestWildAndCavePlantOrder
		{
			get
			{
				if (this.cachedLowestWildPlantOrder == null)
				{
					this.cachedLowestWildPlantOrder = new float?((float)int.MaxValue);
					List<ThingDef> allWildPlants = this.AllWildPlants;
					for (int i = 0; i < allWildPlants.Count; i++)
					{
						this.cachedLowestWildPlantOrder = new float?(Mathf.Min(this.cachedLowestWildPlantOrder.Value, allWildPlants[i].plant.wildOrder));
					}
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].category == ThingCategory.Plant && allDefsListForReading[j].plant.cavePlant)
						{
							this.cachedLowestWildPlantOrder = new float?(Mathf.Min(this.cachedLowestWildPlantOrder.Value, allDefsListForReading[j].plant.wildOrder));
						}
					}
				}
				return this.cachedLowestWildPlantOrder.Value;
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x060035C7 RID: 13767 RVA: 0x001245F2 File Offset: 0x001227F2
		public IEnumerable<PawnKindDef> AllWildAnimals
		{
			get
			{
				foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
				{
					if (this.CommonalityOfAnimal(pawnKindDef) > 0f)
					{
						yield return pawnKindDef;
					}
				}
				IEnumerator<PawnKindDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x060035C8 RID: 13768 RVA: 0x00124602 File Offset: 0x00122802
		public float PlantCommonalitiesSum
		{
			get
			{
				this.CachePlantCommonalitiesIfShould();
				return this.cachedPlantCommonalitiesSum;
			}
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x060035C9 RID: 13769 RVA: 0x00124610 File Offset: 0x00122810
		public float TreeDensity
		{
			get
			{
				float num = 0f;
				float num2 = 0f;
				this.CachePlantCommonalitiesIfShould();
				foreach (KeyValuePair<ThingDef, float> keyValuePair in this.cachedPlantCommonalities)
				{
					num += keyValuePair.Value;
					if (keyValuePair.Key.plant.IsTree)
					{
						num2 += keyValuePair.Value;
					}
				}
				if (num == 0f)
				{
					return 0f;
				}
				return num2 / num * this.plantDensity;
			}
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x001246B0 File Offset: 0x001228B0
		public float CommonalityOfAnimal(PawnKindDef animalDef)
		{
			if (this.cachedAnimalCommonalities == null)
			{
				this.cachedAnimalCommonalities = new Dictionary<PawnKindDef, float>();
				for (int i = 0; i < this.wildAnimals.Count; i++)
				{
					this.cachedAnimalCommonalities.Add(this.wildAnimals[i].animal, this.wildAnimals[i].commonality);
				}
				foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
				{
					if (pawnKindDef.RaceProps.wildBiomes != null)
					{
						for (int j = 0; j < pawnKindDef.RaceProps.wildBiomes.Count; j++)
						{
							if (pawnKindDef.RaceProps.wildBiomes[j].biome == this)
							{
								this.cachedAnimalCommonalities.Add(pawnKindDef, pawnKindDef.RaceProps.wildBiomes[j].commonality);
							}
						}
					}
				}
			}
			float result;
			if (this.cachedAnimalCommonalities.TryGetValue(animalDef, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x001247D0 File Offset: 0x001229D0
		public float CommonalityOfPlant(ThingDef plantDef)
		{
			this.CachePlantCommonalitiesIfShould();
			float result;
			if (this.cachedPlantCommonalities.TryGetValue(plantDef, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x001247FA File Offset: 0x001229FA
		public float CommonalityPctOfPlant(ThingDef plantDef)
		{
			return this.CommonalityOfPlant(plantDef) / this.PlantCommonalitiesSum;
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x0012480C File Offset: 0x00122A0C
		public float CommonalityOfDisease(IncidentDef diseaseInc)
		{
			if (this.cachedDiseaseCommonalities == null)
			{
				this.cachedDiseaseCommonalities = new Dictionary<IncidentDef, float>();
				for (int i = 0; i < this.diseases.Count; i++)
				{
					this.cachedDiseaseCommonalities.Add(this.diseases[i].diseaseInc, this.diseases[i].commonality);
				}
				foreach (IncidentDef incidentDef in DefDatabase<IncidentDef>.AllDefs)
				{
					if (incidentDef.diseaseBiomeRecords != null)
					{
						for (int j = 0; j < incidentDef.diseaseBiomeRecords.Count; j++)
						{
							if (incidentDef.diseaseBiomeRecords[j].biome == this)
							{
								this.cachedDiseaseCommonalities.Add(incidentDef.diseaseBiomeRecords[j].diseaseInc, incidentDef.diseaseBiomeRecords[j].commonality);
							}
						}
					}
				}
			}
			float result;
			if (this.cachedDiseaseCommonalities.TryGetValue(diseaseInc, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x00124928 File Offset: 0x00122B28
		public bool IsPackAnimalAllowed(ThingDef pawn)
		{
			return this.allowedPackAnimals.Contains(pawn);
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x00124936 File Offset: 0x00122B36
		public static BiomeDef Named(string defName)
		{
			return DefDatabase<BiomeDef>.GetNamed(defName, true);
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x00124940 File Offset: 0x00122B40
		private void CachePlantCommonalitiesIfShould()
		{
			if (this.cachedPlantCommonalities == null)
			{
				this.cachedPlantCommonalities = new Dictionary<ThingDef, float>();
				for (int i = 0; i < this.wildPlants.Count; i++)
				{
					if (this.wildPlants[i].plant != null)
					{
						this.cachedPlantCommonalities.Add(this.wildPlants[i].plant, this.wildPlants[i].commonality);
					}
				}
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef.plant != null && thingDef.plant.wildBiomes != null)
					{
						for (int j = 0; j < thingDef.plant.wildBiomes.Count; j++)
						{
							if (thingDef.plant.wildBiomes[j].biome == this)
							{
								this.cachedPlantCommonalities.Add(thingDef, thingDef.plant.wildBiomes[j].commonality);
							}
						}
					}
				}
				this.cachedPlantCommonalitiesSum = this.cachedPlantCommonalities.Sum((KeyValuePair<ThingDef, float> x) => x.Value);
			}
		}


		public override IEnumerable<string> ConfigErrors()
		{
			//foreach (string text in this.<>n__0())
			//{
			//	yield return text;
			//}
			//IEnumerator<string> enumerator = null;
			//if (Prefs.DevMode)
			//{
			//	using (List<BiomeAnimalRecord>.Enumerator enumerator2 = this.wildAnimals.GetEnumerator())
			//	{
			//		while (enumerator2.MoveNext())
			//		{
			//			BiomeAnimalRecord wa = enumerator2.Current;
			//			if (this.wildAnimals.Count((BiomeAnimalRecord a) => a.animal == wa.animal) > 1)
			//			{
			//				yield return "Duplicate animal record: " + wa.animal.defName;
			//			}
			//		}
			//	}
			//	List<BiomeAnimalRecord>.Enumerator enumerator2 = default(List<BiomeAnimalRecord>.Enumerator);
			//}
			yield break;
			yield break;
		}

		// Token: 0x04001D7A RID: 7546
		public Type workerClass = typeof(BiomeWorker);

		// Token: 0x04001D7B RID: 7547
		public bool implemented = true;

		// Token: 0x04001D7C RID: 7548
		public bool canBuildBase = true;

		// Token: 0x04001D7D RID: 7549
		public bool canAutoChoose = true;

		// Token: 0x04001D7E RID: 7550
		public bool allowRoads = true;

		// Token: 0x04001D7F RID: 7551
		public bool allowRivers = true;

		// Token: 0x04001D80 RID: 7552
		public float animalDensity;

		// Token: 0x04001D81 RID: 7553
		public float plantDensity;

		// Token: 0x04001D82 RID: 7554
		public float diseaseMtbDays = 60f;

		// Token: 0x04001D83 RID: 7555
		public float settlementSelectionWeight = 1f;

		// Token: 0x04001D84 RID: 7556
		public bool impassable;

		// Token: 0x04001D85 RID: 7557
		public bool hasVirtualPlants = true;

		// Token: 0x04001D86 RID: 7558
		public float forageability;

		// Token: 0x04001D87 RID: 7559
		public ThingDef foragedFood;

		// Token: 0x04001D88 RID: 7560
		public bool wildPlantsCareAboutLocalFertility = true;

		// Token: 0x04001D89 RID: 7561
		public float wildPlantRegrowDays = 25f;

		// Token: 0x04001D8A RID: 7562
		public float movementDifficulty = 1f;

		// Token: 0x04001D8B RID: 7563
		public List<WeatherCommonalityRecord> baseWeatherCommonalities = new List<WeatherCommonalityRecord>();

		// Token: 0x04001D8C RID: 7564
		public List<TerrainThreshold> terrainsByFertility = new List<TerrainThreshold>();

		// Token: 0x04001D8D RID: 7565
		public List<SoundDef> soundsAmbient = new List<SoundDef>();

		// Token: 0x04001D8E RID: 7566
		public List<TerrainPatchMaker> terrainPatchMakers = new List<TerrainPatchMaker>();

		// Token: 0x04001D8F RID: 7567
		private List<BiomePlantRecord> wildPlants = new List<BiomePlantRecord>();

		// Token: 0x04001D90 RID: 7568
		private List<BiomeAnimalRecord> wildAnimals = new List<BiomeAnimalRecord>();

		// Token: 0x04001D91 RID: 7569
		private List<BiomeDiseaseRecord> diseases = new List<BiomeDiseaseRecord>();

		// Token: 0x04001D92 RID: 7570
		private List<ThingDef> allowedPackAnimals = new List<ThingDef>();

		// Token: 0x04001D93 RID: 7571
		public bool hasBedrock = true;

		// Token: 0x04001D94 RID: 7572
		public bool isExtremeBiome;

		// Token: 0x04001D95 RID: 7573
		[NoTranslate]
		public string texture;

		// Token: 0x04001D96 RID: 7574
		[Unsaved(false)]
		private Dictionary<PawnKindDef, float> cachedAnimalCommonalities;

		// Token: 0x04001D97 RID: 7575
		[Unsaved(false)]
		private Dictionary<ThingDef, float> cachedPlantCommonalities;

		// Token: 0x04001D98 RID: 7576
		[Unsaved(false)]
		private Dictionary<IncidentDef, float> cachedDiseaseCommonalities;

		// Token: 0x04001D99 RID: 7577
		[Unsaved(false)]
		private Material cachedMat;

		// Token: 0x04001D9A RID: 7578
		[Unsaved(false)]
		private List<ThingDef> cachedWildPlants;

		// Token: 0x04001D9B RID: 7579
		[Unsaved(false)]
		private int? cachedMaxWildPlantsClusterRadius;

		// Token: 0x04001D9C RID: 7580
		[Unsaved(false)]
		private float cachedPlantCommonalitiesSum;

		// Token: 0x04001D9D RID: 7581
		[Unsaved(false)]
		private float? cachedLowestWildPlantOrder;

		// Token: 0x04001D9E RID: 7582
		[Unsaved(false)]
		private BiomeWorker workerInt;
	}
}
