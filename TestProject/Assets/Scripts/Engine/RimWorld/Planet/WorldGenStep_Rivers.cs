using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001216 RID: 4630
	public class WorldGenStep_Rivers : WorldGenStep
	{
		// Token: 0x170011ED RID: 4589
		// (get) Token: 0x06006B0D RID: 27405 RVA: 0x002558A0 File Offset: 0x00253AA0
		public override int SeedPart
		{
			get
			{
				return 605014749;
			}
		}

		// Token: 0x06006B0E RID: 27406 RVA: 0x002558A7 File Offset: 0x00253AA7
		public override void GenerateFresh(string seed)
		{
			this.GenerateRivers();
		}

		// Token: 0x06006B0F RID: 27407 RVA: 0x002558B0 File Offset: 0x00253AB0
		private void GenerateRivers()
		{
			Find.WorldPathGrid.RecalculateAllPerceivedPathCosts();
			List<int> coastalWaterTiles = this.GetCoastalWaterTiles();
			if (!coastalWaterTiles.Any<int>())
			{
				return;
			}
			List<int> neighbors = new List<int>();
			List<int>[] array = Find.WorldPathFinder.FloodPathsWithCostForTree(coastalWaterTiles, delegate(int st, int ed)
			{
				Tile tile = Find.WorldGrid[ed];
				Tile tile2 = Find.WorldGrid[st];
				Find.WorldGrid.GetTileNeighbors(ed, neighbors);
				int num = neighbors[0];
				for (int j = 0; j < neighbors.Count; j++)
				{
					if (WorldGenStep_Rivers.GetImpliedElevation(Find.WorldGrid[neighbors[j]]) < WorldGenStep_Rivers.GetImpliedElevation(Find.WorldGrid[num]))
					{
						num = neighbors[j];
					}
				}
				float num2 = 1f;
				if (num != st)
				{
					num2 = 2f;
				}
				return Mathf.RoundToInt(num2 * WorldGenStep_Rivers.ElevationChangeCost.Evaluate(WorldGenStep_Rivers.GetImpliedElevation(tile2) - WorldGenStep_Rivers.GetImpliedElevation(tile)));
			}, (int tid) => Find.WorldGrid[tid].WaterCovered, null);
			float[] flow = new float[array.Length];
			for (int i = 0; i < coastalWaterTiles.Count; i++)
			{
				this.AccumulateFlow(flow, array, coastalWaterTiles[i]);
				this.CreateRivers(flow, array, coastalWaterTiles[i]);
			}
		}

		// Token: 0x06006B10 RID: 27408 RVA: 0x00255960 File Offset: 0x00253B60
		private static float GetImpliedElevation(Tile tile)
		{
			float num = 0f;
			if (tile.hilliness == Hilliness.SmallHills)
			{
				num = 15f;
			}
			else if (tile.hilliness == Hilliness.LargeHills)
			{
				num = 250f;
			}
			else if (tile.hilliness == Hilliness.Mountainous)
			{
				num = 500f;
			}
			else if (tile.hilliness == Hilliness.Impassable)
			{
				num = 1000f;
			}
			return tile.elevation + num;
		}

		// Token: 0x06006B11 RID: 27409 RVA: 0x002559C0 File Offset: 0x00253BC0
		private List<int> GetCoastalWaterTiles()
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
			{
				if (Find.WorldGrid[i].biome == BiomeDefOf.Ocean)
				{
					Find.WorldGrid.GetTileNeighbors(i, list2);
					bool flag = false;
					for (int j = 0; j < list2.Count; j++)
					{
						if (Find.WorldGrid[list2[j]].biome != BiomeDefOf.Ocean)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						list.Add(i);
					}
				}
			}
			return list;
		}

		// Token: 0x06006B12 RID: 27410 RVA: 0x00255A5C File Offset: 0x00253C5C
		private void AccumulateFlow(float[] flow, List<int>[] riverPaths, int index)
		{
			Tile tile = Find.WorldGrid[index];
			flow[index] += tile.rainfall;
			if (riverPaths[index] != null)
			{
				for (int i = 0; i < riverPaths[index].Count; i++)
				{
					this.AccumulateFlow(flow, riverPaths, riverPaths[index][i]);
					flow[index] += flow[riverPaths[index][i]];
				}
			}
			flow[index] = Mathf.Max(0f, flow[index] - WorldGenStep_Rivers.CalculateTotalEvaporation(flow[index], tile.temperature));
		}

		// Token: 0x06006B13 RID: 27411 RVA: 0x00255AE4 File Offset: 0x00253CE4
		private void CreateRivers(float[] flow, List<int>[] riverPaths, int index)
		{
			List<int> list = new List<int>();
			Find.WorldGrid.GetTileNeighbors(index, list);
			for (int i = 0; i < list.Count; i++)
			{
				float targetFlow = flow[list[i]];
				RiverDef riverDef = (from rd in DefDatabase<RiverDef>.AllDefs
				where rd.spawnFlowThreshold > 0 && (float)rd.spawnFlowThreshold <= targetFlow
				select rd).MaxByWithFallback((RiverDef rd) => rd.spawnFlowThreshold, null);
				if (riverDef != null && Rand.Value < riverDef.spawnChance)
				{
					Find.WorldGrid.OverlayRiver(index, list[i], riverDef);
					this.ExtendRiver(flow, riverPaths, list[i], riverDef);
				}
			}
		}

		// Token: 0x06006B14 RID: 27412 RVA: 0x00255BA0 File Offset: 0x00253DA0
		private void ExtendRiver(float[] flow, List<int>[] riverPaths, int index, RiverDef incomingRiver)
		{
			if (riverPaths[index] == null)
			{
				return;
			}
			int bestOutput = riverPaths[index].MaxBy((int ni) => flow[ni]);
			RiverDef riverDef = incomingRiver;
			while (riverDef != null && (float)riverDef.degradeThreshold > flow[bestOutput])
			{
				riverDef = riverDef.degradeChild;
			}
			if (riverDef != null)
			{
				Find.WorldGrid.OverlayRiver(index, bestOutput, riverDef);
				this.ExtendRiver(flow, riverPaths, bestOutput, riverDef);
			}
			if (incomingRiver.branches != null)
			{
				IEnumerable<int> source = riverPaths[index];
				Func<int, bool> <>9__1;
				Func<int, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((int ni) => ni != bestOutput));
				}
				using (IEnumerator<int> enumerator = source.Where(predicate).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int alternateRiver = enumerator.Current;
						RiverDef.Branch branch2 = (from branch in incomingRiver.branches
						where (float)branch.minFlow <= flow[alternateRiver]
						select branch).MaxByWithFallback((RiverDef.Branch branch) => branch.minFlow, null);
						if (branch2 != null && Rand.Value < branch2.chance)
						{
							Find.WorldGrid.OverlayRiver(index, alternateRiver, branch2.child);
							this.ExtendRiver(flow, riverPaths, alternateRiver, branch2.child);
						}
					}
				}
			}
		}

		// Token: 0x06006B15 RID: 27413 RVA: 0x00255D38 File Offset: 0x00253F38
		public static float CalculateEvaporationConstant(float temperature)
		{
			return 0.61121f * Mathf.Exp((18.678f - temperature / 234.5f) * (temperature / (257.14f + temperature))) / (temperature + 273f);
		}

		// Token: 0x06006B16 RID: 27414 RVA: 0x00255D64 File Offset: 0x00253F64
		public static float CalculateRiverSurfaceArea(float flow)
		{
			return Mathf.Pow(flow, 0.5f);
		}

		// Token: 0x06006B17 RID: 27415 RVA: 0x00255D71 File Offset: 0x00253F71
		public static float CalculateEvaporativeArea(float flow)
		{
			return WorldGenStep_Rivers.CalculateRiverSurfaceArea(flow) + 0f;
		}

		// Token: 0x06006B18 RID: 27416 RVA: 0x00255D7F File Offset: 0x00253F7F
		public static float CalculateTotalEvaporation(float flow, float temperature)
		{
			return WorldGenStep_Rivers.CalculateEvaporationConstant(temperature) * WorldGenStep_Rivers.CalculateEvaporativeArea(flow) * 250f;
		}

		// Token: 0x040042DD RID: 17117
		private static readonly SimpleCurve ElevationChangeCost = new SimpleCurve
		{
			{
				new CurvePoint(-1000f, 50f),
				true
			},
			{
				new CurvePoint(-100f, 100f),
				true
			},
			{
				new CurvePoint(0f, 400f),
				true
			},
			{
				new CurvePoint(0f, 5000f),
				true
			},
			{
				new CurvePoint(100f, 50000f),
				true
			},
			{
				new CurvePoint(1000f, 50000f),
				true
			}
		};

		// Token: 0x040042DE RID: 17118
		private const float HillinessSmallHillsElevation = 15f;

		// Token: 0x040042DF RID: 17119
		private const float HillinessLargeHillsElevation = 250f;

		// Token: 0x040042E0 RID: 17120
		private const float HillinessMountainousElevation = 500f;

		// Token: 0x040042E1 RID: 17121
		private const float HillinessImpassableElevation = 1000f;

		// Token: 0x040042E2 RID: 17122
		private const float NonRiverEvaporation = 0f;

		// Token: 0x040042E3 RID: 17123
		private const float EvaporationMultiple = 250f;
	}
}
