using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class WildAnimalSpawner
	{
		
		// (get) Token: 0x060040F9 RID: 16633 RVA: 0x0015BF50 File Offset: 0x0015A150
		private float DesiredAnimalDensity
		{
			get
			{
				float num = this.map.Biome.animalDensity;
				float num2 = 0f;
				float num3 = 0f;
				foreach (PawnKindDef pawnKindDef in this.map.Biome.AllWildAnimals)
				{
					float num4 = this.map.Biome.CommonalityOfAnimal(pawnKindDef);
					num3 += num4;
					if (this.map.mapTemperature.SeasonAcceptableFor(pawnKindDef.race))
					{
						num2 += num4;
					}
				}
				num *= num2 / num3;
				num *= this.map.gameConditionManager.AggregateAnimalDensityFactor(this.map);
				return num;
			}
		}

		
		// (get) Token: 0x060040FA RID: 16634 RVA: 0x0015C018 File Offset: 0x0015A218
		private float DesiredTotalAnimalWeight
		{
			get
			{
				float desiredAnimalDensity = this.DesiredAnimalDensity;
				if (desiredAnimalDensity == 0f)
				{
					return 0f;
				}
				float num = 10000f / desiredAnimalDensity;
				return (float)this.map.Area / num;
			}
		}

		
		// (get) Token: 0x060040FB RID: 16635 RVA: 0x0015C050 File Offset: 0x0015A250
		private float CurrentTotalAnimalWeight
		{
			get
			{
				float num = 0f;
				List<Pawn> allPawnsSpawned = this.map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].Faction == null)
					{
						num += allPawnsSpawned[i].kindDef.ecoSystemWeight;
					}
				}
				return num;
			}
		}

		
		// (get) Token: 0x060040FC RID: 16636 RVA: 0x0015C0A8 File Offset: 0x0015A2A8
		public bool AnimalEcosystemFull
		{
			get
			{
				return this.CurrentTotalAnimalWeight >= this.DesiredTotalAnimalWeight;
			}
		}

		
		public WildAnimalSpawner(Map map)
		{
			this.map = map;
		}

		
		public void WildAnimalSpawnerTick()
		{
			if (Find.TickManager.TicksGame % 1213 == 0 && !this.AnimalEcosystemFull && Rand.Chance(0.0269555561f * this.DesiredAnimalDensity))
			{
				TraverseParms traverseParms = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false);
				IntVec3 loc;
				if (RCellFinder.TryFindRandomPawnEntryCell(out loc, this.map, CellFinder.EdgeRoadChance_Animal, true, (IntVec3 cell) => this.map.reachability.CanReachMapEdge(cell, traverseParms)))
				{
					this.SpawnRandomWildAnimalAt(loc);
				}
			}
		}

		
		public bool SpawnRandomWildAnimalAt(IntVec3 loc)
		{
			PawnKindDef pawnKindDef = (from a in this.map.Biome.AllWildAnimals
			where this.map.mapTemperature.SeasonAcceptableFor(a.race)
			select a).RandomElementByWeight((PawnKindDef def) => this.map.Biome.CommonalityOfAnimal(def) / def.wildGroupSize.Average);
			if (pawnKindDef == null)
			{
				Log.Error("No spawnable animals right now.", false);
				return false;
			}
			int randomInRange = pawnKindDef.wildGroupSize.RandomInRange;
			int radius = Mathf.CeilToInt(Mathf.Sqrt((float)pawnKindDef.wildGroupSize.max));
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 loc2 = CellFinder.RandomClosewalkCellNear(loc, this.map, radius, null);
				GenSpawn.Spawn(PawnGenerator.GeneratePawn(pawnKindDef, null), loc2, this.map, WipeMode.Vanish);
			}
			return true;
		}

		
		public string DebugString()
		{
			return string.Concat(new object[]
			{
				"DesiredTotalAnimalWeight: ",
				this.DesiredTotalAnimalWeight,
				"\nCurrentTotalAnimalWeight: ",
				this.CurrentTotalAnimalWeight,
				"\nDesiredAnimalDensity: ",
				this.DesiredAnimalDensity
			});
		}

		
		private Map map;

		
		private const int AnimalCheckInterval = 1213;

		
		private const float BaseAnimalSpawnChancePerInterval = 0.0269555561f;
	}
}
