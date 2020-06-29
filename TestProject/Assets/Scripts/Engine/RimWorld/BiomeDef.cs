using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class BiomeDef : Def
	{
		
		
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

		
		
		public float PlantCommonalitiesSum
		{
			get
			{
				this.CachePlantCommonalitiesIfShould();
				return this.cachedPlantCommonalitiesSum;
			}
		}

		
		
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

		
		public float CommonalityPctOfPlant(ThingDef plantDef)
		{
			return this.CommonalityOfPlant(plantDef) / this.PlantCommonalitiesSum;
		}

		
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

		
		public bool IsPackAnimalAllowed(ThingDef pawn)
		{
			return this.allowedPackAnimals.Contains(pawn);
		}

		
		public static BiomeDef Named(string defName)
		{
			return DefDatabase<BiomeDef>.GetNamed(defName, true);
		}

		
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

			//{
			//	
			//}
			//IEnumerator<string> enumerator = null;
			//if (Prefs.DevMode)
			//{
			//	List<BiomeAnimalRecord>.Enumerator enumerator2 = this.wildAnimals.GetEnumerator();
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

		
		public Type workerClass = typeof(BiomeWorker);

		
		public bool implemented = true;

		
		public bool canBuildBase = true;

		
		public bool canAutoChoose = true;

		
		public bool allowRoads = true;

		
		public bool allowRivers = true;

		
		public float animalDensity;

		
		public float plantDensity;

		
		public float diseaseMtbDays = 60f;

		
		public float settlementSelectionWeight = 1f;

		
		public bool impassable;

		
		public bool hasVirtualPlants = true;

		
		public float forageability;

		
		public ThingDef foragedFood;

		
		public bool wildPlantsCareAboutLocalFertility = true;

		
		public float wildPlantRegrowDays = 25f;

		
		public float movementDifficulty = 1f;

		
		public List<WeatherCommonalityRecord> baseWeatherCommonalities = new List<WeatherCommonalityRecord>();

		
		public List<TerrainThreshold> terrainsByFertility = new List<TerrainThreshold>();

		
		public List<SoundDef> soundsAmbient = new List<SoundDef>();

		
		public List<TerrainPatchMaker> terrainPatchMakers = new List<TerrainPatchMaker>();

		
		private List<BiomePlantRecord> wildPlants = new List<BiomePlantRecord>();

		
		private List<BiomeAnimalRecord> wildAnimals = new List<BiomeAnimalRecord>();

		
		private List<BiomeDiseaseRecord> diseases = new List<BiomeDiseaseRecord>();

		
		private List<ThingDef> allowedPackAnimals = new List<ThingDef>();

		
		public bool hasBedrock = true;

		
		public bool isExtremeBiome;

		
		[NoTranslate]
		public string texture;

		
		[Unsaved(false)]
		private Dictionary<PawnKindDef, float> cachedAnimalCommonalities;

		
		[Unsaved(false)]
		private Dictionary<ThingDef, float> cachedPlantCommonalities;

		
		[Unsaved(false)]
		private Dictionary<IncidentDef, float> cachedDiseaseCommonalities;

		
		[Unsaved(false)]
		private Material cachedMat;

		
		[Unsaved(false)]
		private List<ThingDef> cachedWildPlants;

		
		[Unsaved(false)]
		private int? cachedMaxWildPlantsClusterRadius;

		
		[Unsaved(false)]
		private float cachedPlantCommonalitiesSum;

		
		[Unsaved(false)]
		private float? cachedLowestWildPlantOrder;

		
		[Unsaved(false)]
		private BiomeWorker workerInt;
	}
}
