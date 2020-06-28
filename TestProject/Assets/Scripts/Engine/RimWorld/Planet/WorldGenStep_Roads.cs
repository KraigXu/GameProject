using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001217 RID: 4631
	public class WorldGenStep_Roads : WorldGenStep
	{
		// Token: 0x170011EE RID: 4590
		// (get) Token: 0x06006B1B RID: 27419 RVA: 0x00255E2F File Offset: 0x0025402F
		public override int SeedPart
		{
			get
			{
				return 1538475135;
			}
		}

		// Token: 0x06006B1C RID: 27420 RVA: 0x00255E36 File Offset: 0x00254036
		public override void GenerateFresh(string seed)
		{
			this.GenerateRoadEndpoints();
			Rand.PushState();
			Rand.Seed = GenText.StableStringHash(seed);
			this.GenerateRoadNetwork();
			Rand.PopState();
		}

		// Token: 0x06006B1D RID: 27421 RVA: 0x00255E59 File Offset: 0x00254059
		public override void GenerateWithoutWorldData(string seed)
		{
			Rand.PushState();
			Rand.Seed = GenText.StableStringHash(seed);
			this.GenerateRoadNetwork();
			Rand.PopState();
		}

		// Token: 0x06006B1E RID: 27422 RVA: 0x00255E78 File Offset: 0x00254078
		private void GenerateRoadEndpoints()
		{
			List<int> list = (from wo in Find.WorldObjects.AllWorldObjects
			where Rand.Value > 0.05f
			select wo.Tile).ToList<int>();
			int num = GenMath.RoundRandom((float)Find.WorldGrid.TilesCount / 100000f * WorldGenStep_Roads.ExtraRoadNodesPer100kTiles.RandomInRange);
			for (int i = 0; i < num; i++)
			{
				list.Add(TileFinder.RandomSettlementTileFor(null, false, null));
			}
			List<int> list2 = new List<int>();
			for (int j = 0; j < list.Count; j++)
			{
				int num2 = Mathf.Max(0, WorldGenStep_Roads.RoadDistanceFromSettlement.RandomInRange);
				int num3 = list[j];
				for (int k = 0; k < num2; k++)
				{
					Find.WorldGrid.GetTileNeighbors(num3, list2);
					num3 = list2.RandomElement<int>();
				}
				if (Find.WorldReachability.CanReach(list[j], num3))
				{
					list[j] = num3;
				}
			}
			list = list.Distinct<int>().ToList<int>();
			Find.World.genData.roadNodes = list;
		}

		// Token: 0x06006B1F RID: 27423 RVA: 0x00255FC0 File Offset: 0x002541C0
		private void GenerateRoadNetwork()
		{
			Find.WorldPathGrid.RecalculateAllPerceivedPathCosts(new int?(0));
			List<WorldGenStep_Roads.Link> linkProspective = this.GenerateProspectiveLinks(Find.World.genData.roadNodes);
			List<WorldGenStep_Roads.Link> linkFinal = this.GenerateFinalLinks(linkProspective, Find.World.genData.roadNodes.Count);
			this.DrawLinksOnWorld(linkFinal, Find.World.genData.roadNodes);
		}

		// Token: 0x06006B20 RID: 27424 RVA: 0x00256028 File Offset: 0x00254228
		private List<WorldGenStep_Roads.Link> GenerateProspectiveLinks(List<int> indexToTile)
		{
			WorldGenStep_Roads.<>c__DisplayClass14_0 <>c__DisplayClass14_ = new WorldGenStep_Roads.<>c__DisplayClass14_0();
			<>c__DisplayClass14_.tileToIndexLookup = new Dictionary<int, int>();
			for (int i = 0; i < indexToTile.Count; i++)
			{
				<>c__DisplayClass14_.tileToIndexLookup[indexToTile[i]] = i;
			}
			<>c__DisplayClass14_.linkProspective = new List<WorldGenStep_Roads.Link>();
			List<int> list = new List<int>();
			int srcIndex;
			int srcIndex2;
			for (srcIndex = 0; srcIndex < indexToTile.Count; srcIndex = srcIndex2)
			{
				int srcTile = indexToTile[srcIndex];
				list.Clear();
				list.Add(srcTile);
				int found = 0;
				Find.WorldPathFinder.FloodPathsWithCost(list, (int src, int dst) => Caravan_PathFollower.CostToMove(3300, src, dst, null, true, null, null), null, delegate(int tile, float distance)
				{
					int found;
					if (tile != srcTile && <>c__DisplayClass14_.tileToIndexLookup.ContainsKey(tile))
					{
						found++;
						found = found;
						<>c__DisplayClass14_.linkProspective.Add(new WorldGenStep_Roads.Link
						{
							distance = distance,
							indexA = srcIndex,
							indexB = <>c__DisplayClass14_.tileToIndexLookup[tile]
						});
					}
					return found >= 8;
				});
				srcIndex2 = srcIndex + 1;
			}
			<>c__DisplayClass14_.linkProspective.Sort((WorldGenStep_Roads.Link lhs, WorldGenStep_Roads.Link rhs) => lhs.distance.CompareTo(rhs.distance));
			return <>c__DisplayClass14_.linkProspective;
		}

		// Token: 0x06006B21 RID: 27425 RVA: 0x00256164 File Offset: 0x00254364
		private List<WorldGenStep_Roads.Link> GenerateFinalLinks(List<WorldGenStep_Roads.Link> linkProspective, int endpointCount)
		{
			List<WorldGenStep_Roads.Connectedness> list = new List<WorldGenStep_Roads.Connectedness>();
			for (int i = 0; i < endpointCount; i++)
			{
				list.Add(new WorldGenStep_Roads.Connectedness());
			}
			List<WorldGenStep_Roads.Link> list2 = new List<WorldGenStep_Roads.Link>();
			for (int j = 0; j < linkProspective.Count; j++)
			{
				WorldGenStep_Roads.Link prospective = linkProspective[j];
				if (list[prospective.indexA].Group() != list[prospective.indexB].Group() || (Rand.Value <= 0.015f && !list2.Any((WorldGenStep_Roads.Link link) => link.indexB == prospective.indexA && link.indexA == prospective.indexB)))
				{
					if (Rand.Value > 0.1f)
					{
						list2.Add(prospective);
					}
					if (list[prospective.indexA].Group() != list[prospective.indexB].Group())
					{
						WorldGenStep_Roads.Connectedness parent = new WorldGenStep_Roads.Connectedness();
						list[prospective.indexA].Group().parent = parent;
						list[prospective.indexB].Group().parent = parent;
					}
				}
			}
			return list2;
		}

		// Token: 0x06006B22 RID: 27426 RVA: 0x002562A4 File Offset: 0x002544A4
		private void DrawLinksOnWorld(List<WorldGenStep_Roads.Link> linkFinal, List<int> indexToTile)
		{
			foreach (WorldGenStep_Roads.Link link in linkFinal)
			{
				WorldPath worldPath = Find.WorldPathFinder.FindPath(indexToTile[link.indexA], indexToTile[link.indexB], null, null);
				List<int> nodesReversed = worldPath.NodesReversed;
				RoadDef roadDef = (from rd in DefDatabase<RoadDef>.AllDefsListForReading
				where !rd.ancientOnly
				select rd).RandomElementWithFallback(null);
				for (int i = 0; i < nodesReversed.Count - 1; i++)
				{
					Find.WorldGrid.OverlayRoad(nodesReversed[i], nodesReversed[i + 1], roadDef);
				}
				worldPath.ReleaseToPool();
			}
		}

		// Token: 0x040042E4 RID: 17124
		private static readonly FloatRange ExtraRoadNodesPer100kTiles = new FloatRange(30f, 50f);

		// Token: 0x040042E5 RID: 17125
		private static readonly IntRange RoadDistanceFromSettlement = new IntRange(-4, 4);

		// Token: 0x040042E6 RID: 17126
		private const float ChanceExtraNonSpanningTreeLink = 0.015f;

		// Token: 0x040042E7 RID: 17127
		private const float ChanceHideSpanningTreeLink = 0.1f;

		// Token: 0x040042E8 RID: 17128
		private const float ChanceWorldObjectReclusive = 0.05f;

		// Token: 0x040042E9 RID: 17129
		private const int PotentialSpanningTreeLinksPerSettlement = 8;

		// Token: 0x02001FA5 RID: 8101
		private struct Link
		{
			// Token: 0x04007686 RID: 30342
			public float distance;

			// Token: 0x04007687 RID: 30343
			public int indexA;

			// Token: 0x04007688 RID: 30344
			public int indexB;
		}

		// Token: 0x02001FA6 RID: 8102
		private class Connectedness
		{
			// Token: 0x0600AE41 RID: 44609 RVA: 0x003253E6 File Offset: 0x003235E6
			public WorldGenStep_Roads.Connectedness Group()
			{
				if (this.parent == null)
				{
					return this;
				}
				return this.parent.Group();
			}

			// Token: 0x04007689 RID: 30345
			public WorldGenStep_Roads.Connectedness parent;
		}
	}
}
