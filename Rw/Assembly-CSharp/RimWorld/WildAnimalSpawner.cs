using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AB9 RID: 2745
	public class WildAnimalSpawner
	{
		// Token: 0x17000B82 RID: 2946
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

		// Token: 0x17000B83 RID: 2947
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

		// Token: 0x17000B84 RID: 2948
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

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x060040FC RID: 16636 RVA: 0x0015C0A8 File Offset: 0x0015A2A8
		public bool AnimalEcosystemFull
		{
			get
			{
				return this.CurrentTotalAnimalWeight >= this.DesiredTotalAnimalWeight;
			}
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x0015C0BB File Offset: 0x0015A2BB
		public WildAnimalSpawner(Map map)
		{
			this.map = map;
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x0015C0CC File Offset: 0x0015A2CC
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

		// Token: 0x060040FF RID: 16639 RVA: 0x0015C14C File Offset: 0x0015A34C
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

		// Token: 0x06004100 RID: 16640 RVA: 0x0015C1F4 File Offset: 0x0015A3F4
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

		// Token: 0x040025B8 RID: 9656
		private Map map;

		// Token: 0x040025B9 RID: 9657
		private const int AnimalCheckInterval = 1213;

		// Token: 0x040025BA RID: 9658
		private const float BaseAnimalSpawnChancePerInterval = 0.0269555561f;
	}
}
