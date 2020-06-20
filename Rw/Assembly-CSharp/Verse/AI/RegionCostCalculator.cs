using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x0200057A RID: 1402
	public class RegionCostCalculator
	{
		// Token: 0x060027B4 RID: 10164 RVA: 0x000E8BA4 File Offset: 0x000E6DA4
		public RegionCostCalculator(Map map)
		{
			this.map = map;
			this.preciseRegionLinkDistancesDistanceGetter = new Func<int, int, float>(this.PreciseRegionLinkDistancesDistanceGetter);
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x000E8C18 File Offset: 0x000E6E18
		public void Init(CellRect destination, HashSet<Region> destRegions, TraverseParms parms, int moveTicksCardinal, int moveTicksDiagonal, ByteGrid avoidGrid, Area allowedArea, bool drafted)
		{
			this.regionGrid = this.map.regionGrid.DirectGrid;
			this.traverseParms = parms;
			this.destinationCell = destination.CenterCell;
			this.moveTicksCardinal = moveTicksCardinal;
			this.moveTicksDiagonal = moveTicksDiagonal;
			this.avoidGrid = avoidGrid;
			this.allowedArea = allowedArea;
			this.drafted = drafted;
			this.regionMinLink.Clear();
			this.distances.Clear();
			this.linkTargetCells.Clear();
			this.queue.Clear();
			this.minPathCosts.Clear();
			foreach (Region region in destRegions)
			{
				int minPathCost = this.RegionMedianPathCost(region);
				for (int i = 0; i < region.links.Count; i++)
				{
					RegionLink regionLink = region.links[i];
					if (regionLink.GetOtherRegion(region).Allows(this.traverseParms, false))
					{
						int num = this.RegionLinkDistance(this.destinationCell, regionLink, minPathCost);
						int num2;
						if (this.distances.TryGetValue(regionLink, out num2))
						{
							if (num < num2)
							{
								this.linkTargetCells[regionLink] = this.GetLinkTargetCell(this.destinationCell, regionLink);
							}
							num = Math.Min(num2, num);
						}
						else
						{
							this.linkTargetCells[regionLink] = this.GetLinkTargetCell(this.destinationCell, regionLink);
						}
						this.distances[regionLink] = num;
					}
				}
				this.GetPreciseRegionLinkDistances(region, destination, this.preciseRegionLinkDistances);
				for (int j = 0; j < this.preciseRegionLinkDistances.Count; j++)
				{
					Pair<RegionLink, int> pair = this.preciseRegionLinkDistances[j];
					RegionLink first = pair.First;
					int num3 = this.distances[first];
					int num4;
					if (pair.Second > num3)
					{
						this.distances[first] = pair.Second;
						num4 = pair.Second;
					}
					else
					{
						num4 = num3;
					}
					this.queue.Push(new RegionCostCalculator.RegionLinkQueueEntry(region, first, num4, num4));
				}
			}
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x000E8E54 File Offset: 0x000E7054
		public int GetRegionDistance(Region region, out RegionLink minLink)
		{
			if (this.regionMinLink.TryGetValue(region.id, out minLink))
			{
				return this.distances[minLink];
			}
			while (this.queue.Count != 0)
			{
				RegionCostCalculator.RegionLinkQueueEntry regionLinkQueueEntry = this.queue.Pop();
				int num = this.distances[regionLinkQueueEntry.Link];
				if (regionLinkQueueEntry.Cost == num)
				{
					Region otherRegion = regionLinkQueueEntry.Link.GetOtherRegion(regionLinkQueueEntry.From);
					if (otherRegion != null && otherRegion.valid)
					{
						int num2 = 0;
						if (otherRegion.door != null)
						{
							num2 = PathFinder.GetBuildingCost(otherRegion.door, this.traverseParms, this.traverseParms.pawn);
							if (num2 == 2147483647)
							{
								continue;
							}
							num2 += this.OctileDistance(1, 0);
						}
						int minPathCost = this.RegionMedianPathCost(otherRegion);
						for (int i = 0; i < otherRegion.links.Count; i++)
						{
							RegionLink regionLink = otherRegion.links[i];
							if (regionLink != regionLinkQueueEntry.Link && regionLink.GetOtherRegion(otherRegion).type.Passable())
							{
								int num3 = (otherRegion.door != null) ? num2 : this.RegionLinkDistance(regionLinkQueueEntry.Link, regionLink, minPathCost);
								num3 = Math.Max(num3, 1);
								int num4 = num + num3;
								int estimatedPathCost = this.MinimumRegionLinkDistance(this.destinationCell, regionLink) + num4;
								int num5;
								if (this.distances.TryGetValue(regionLink, out num5))
								{
									if (num4 < num5)
									{
										this.distances[regionLink] = num4;
										this.queue.Push(new RegionCostCalculator.RegionLinkQueueEntry(otherRegion, regionLink, num4, estimatedPathCost));
									}
								}
								else
								{
									this.distances.Add(regionLink, num4);
									this.queue.Push(new RegionCostCalculator.RegionLinkQueueEntry(otherRegion, regionLink, num4, estimatedPathCost));
								}
							}
						}
						if (!this.regionMinLink.ContainsKey(otherRegion.id))
						{
							this.regionMinLink.Add(otherRegion.id, regionLinkQueueEntry.Link);
							if (otherRegion == region)
							{
								minLink = regionLinkQueueEntry.Link;
								return regionLinkQueueEntry.Cost;
							}
						}
					}
				}
			}
			return 10000;
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x000E906C File Offset: 0x000E726C
		public int GetRegionBestDistances(Region region, out RegionLink bestLink, out RegionLink secondBestLink, out int secondBestCost)
		{
			int regionDistance = this.GetRegionDistance(region, out bestLink);
			secondBestLink = null;
			secondBestCost = int.MaxValue;
			for (int i = 0; i < region.links.Count; i++)
			{
				RegionLink regionLink = region.links[i];
				int num;
				if (regionLink != bestLink && regionLink.GetOtherRegion(region).type.Passable() && this.distances.TryGetValue(regionLink, out num) && num < secondBestCost)
				{
					secondBestCost = num;
					secondBestLink = regionLink;
				}
			}
			return regionDistance;
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x000E90E8 File Offset: 0x000E72E8
		public int RegionMedianPathCost(Region region)
		{
			int result;
			if (this.minPathCosts.TryGetValue(region, out result))
			{
				return result;
			}
			bool ignoreAllowedAreaCost = this.allowedArea != null && region.OverlapWith(this.allowedArea) > AreaOverlap.None;
			CellIndices cellIndices = this.map.cellIndices;
			Rand.PushState();
			Rand.Seed = cellIndices.CellToIndex(region.extentsClose.CenterCell) * (region.links.Count + 1);
			for (int i = 0; i < 11; i++)
			{
				RegionCostCalculator.pathCostSamples[i] = this.GetCellCostFast(cellIndices.CellToIndex(region.RandomCell), ignoreAllowedAreaCost);
			}
			Rand.PopState();
			Array.Sort<int>(RegionCostCalculator.pathCostSamples);
			return this.minPathCosts[region] = RegionCostCalculator.pathCostSamples[4];
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x000E91A8 File Offset: 0x000E73A8
		private int GetCellCostFast(int index, bool ignoreAllowedAreaCost = false)
		{
			int num = this.map.pathGrid.pathGrid[index];
			if (this.avoidGrid != null)
			{
				num += (int)(this.avoidGrid[index] * 8);
			}
			if (this.allowedArea != null && !ignoreAllowedAreaCost && !this.allowedArea[index])
			{
				num += 600;
			}
			if (this.drafted)
			{
				num += this.map.terrainGrid.topGrid[index].extraDraftedPerceivedPathCost;
			}
			else
			{
				num += this.map.terrainGrid.topGrid[index].extraNonDraftedPerceivedPathCost;
			}
			return num;
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x000E9244 File Offset: 0x000E7444
		private int RegionLinkDistance(RegionLink a, RegionLink b, int minPathCost)
		{
			IntVec3 a2 = this.linkTargetCells.ContainsKey(a) ? this.linkTargetCells[a] : RegionCostCalculator.RegionLinkCenter(a);
			IntVec3 b2 = this.linkTargetCells.ContainsKey(b) ? this.linkTargetCells[b] : RegionCostCalculator.RegionLinkCenter(b);
			IntVec3 intVec = a2 - b2;
			int num = Math.Abs(intVec.x);
			int num2 = Math.Abs(intVec.z);
			return this.OctileDistance(num, num2) + minPathCost * Math.Max(num, num2) + minPathCost * Math.Min(num, num2);
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x000E92D0 File Offset: 0x000E74D0
		public int RegionLinkDistance(IntVec3 cell, RegionLink link, int minPathCost)
		{
			IntVec3 linkTargetCell = this.GetLinkTargetCell(cell, link);
			IntVec3 intVec = cell - linkTargetCell;
			int num = Math.Abs(intVec.x);
			int num2 = Math.Abs(intVec.z);
			return this.OctileDistance(num, num2) + minPathCost * Math.Max(num, num2) + minPathCost * Math.Min(num, num2);
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x000E9320 File Offset: 0x000E7520
		private static int SpanCenterX(EdgeSpan e)
		{
			return e.root.x + ((e.dir == SpanDirection.East) ? (e.length / 2) : 0);
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x000E9342 File Offset: 0x000E7542
		private static int SpanCenterZ(EdgeSpan e)
		{
			return e.root.z + ((e.dir == SpanDirection.North) ? (e.length / 2) : 0);
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x000E9363 File Offset: 0x000E7563
		private static IntVec3 RegionLinkCenter(RegionLink link)
		{
			return new IntVec3(RegionCostCalculator.SpanCenterX(link.span), 0, RegionCostCalculator.SpanCenterZ(link.span));
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x000E9384 File Offset: 0x000E7584
		private int MinimumRegionLinkDistance(IntVec3 cell, RegionLink link)
		{
			IntVec3 intVec = cell - RegionCostCalculator.LinkClosestCell(cell, link);
			return this.OctileDistance(Math.Abs(intVec.x), Math.Abs(intVec.z));
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x000E93BB File Offset: 0x000E75BB
		private int OctileDistance(int dx, int dz)
		{
			return GenMath.OctileDistance(dx, dz, this.moveTicksCardinal, this.moveTicksDiagonal);
		}

		// Token: 0x060027C1 RID: 10177 RVA: 0x000E93D0 File Offset: 0x000E75D0
		private IntVec3 GetLinkTargetCell(IntVec3 cell, RegionLink link)
		{
			return RegionCostCalculator.LinkClosestCell(cell, link);
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x000E93DC File Offset: 0x000E75DC
		private static IntVec3 LinkClosestCell(IntVec3 cell, RegionLink link)
		{
			EdgeSpan span = link.span;
			int num = 0;
			int num2 = 0;
			if (span.dir == SpanDirection.North)
			{
				num2 = span.length - 1;
			}
			else
			{
				num = span.length - 1;
			}
			IntVec3 root = span.root;
			return new IntVec3(Mathf.Clamp(cell.x, root.x, root.x + num), 0, Mathf.Clamp(cell.z, root.z, root.z + num2));
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x000E9450 File Offset: 0x000E7650
		private void GetPreciseRegionLinkDistances(Region region, CellRect destination, List<Pair<RegionLink, int>> outDistances)
		{
			outDistances.Clear();
			RegionCostCalculator.tmpCellIndices.Clear();
			if (destination.Width == 1 && destination.Height == 1)
			{
				RegionCostCalculator.tmpCellIndices.Add(this.map.cellIndices.CellToIndex(destination.CenterCell));
			}
			else
			{
				foreach (IntVec3 c in destination)
				{
					if (c.InBounds(this.map))
					{
						RegionCostCalculator.tmpCellIndices.Add(this.map.cellIndices.CellToIndex(c));
					}
				}
			}
			Dijkstra<int>.Run(RegionCostCalculator.tmpCellIndices, (int x) => this.PreciseRegionLinkDistancesNeighborsGetter(x, region), this.preciseRegionLinkDistancesDistanceGetter, RegionCostCalculator.tmpDistances, null);
			for (int i = 0; i < region.links.Count; i++)
			{
				RegionLink regionLink = region.links[i];
				if (regionLink.GetOtherRegion(region).Allows(this.traverseParms, false))
				{
					float num;
					if (!RegionCostCalculator.tmpDistances.TryGetValue(this.map.cellIndices.CellToIndex(this.linkTargetCells[regionLink]), out num))
					{
						Log.ErrorOnce("Dijkstra couldn't reach one of the cells even though they are in the same region. There is most likely something wrong with the neighbor nodes getter.", 1938471531, false);
						num = 100f;
					}
					outDistances.Add(new Pair<RegionLink, int>(regionLink, (int)num));
				}
			}
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x000E95E0 File Offset: 0x000E77E0
		private IEnumerable<int> PreciseRegionLinkDistancesNeighborsGetter(int node, Region region)
		{
			if (this.regionGrid[node] == null || this.regionGrid[node] != region)
			{
				return null;
			}
			return this.PathableNeighborIndices(node);
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x000E9600 File Offset: 0x000E7800
		private float PreciseRegionLinkDistancesDistanceGetter(int a, int b)
		{
			return (float)(this.GetCellCostFast(b, false) + (this.AreCellsDiagonal(a, b) ? this.moveTicksDiagonal : this.moveTicksCardinal));
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x000E9624 File Offset: 0x000E7824
		private bool AreCellsDiagonal(int a, int b)
		{
			int x = this.map.Size.x;
			return a % x != b % x && a / x != b / x;
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x000E9658 File Offset: 0x000E7858
		private List<int> PathableNeighborIndices(int index)
		{
			RegionCostCalculator.tmpPathableNeighborIndices.Clear();
			PathGrid pathGrid = this.map.pathGrid;
			int x = this.map.Size.x;
			bool flag = index % x > 0;
			bool flag2 = index % x < x - 1;
			bool flag3 = index >= x;
			bool flag4 = index / x < this.map.Size.z - 1;
			if (flag3 && pathGrid.WalkableFast(index - x))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index - x);
			}
			if (flag2 && pathGrid.WalkableFast(index + 1))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index + 1);
			}
			if (flag && pathGrid.WalkableFast(index - 1))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index - 1);
			}
			if (flag4 && pathGrid.WalkableFast(index + x))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index + x);
			}
			bool flag5 = !flag || PathFinder.BlocksDiagonalMovement(index - 1, this.map);
			bool flag6 = !flag2 || PathFinder.BlocksDiagonalMovement(index + 1, this.map);
			if (flag3 && !PathFinder.BlocksDiagonalMovement(index - x, this.map))
			{
				if (!flag6 && pathGrid.WalkableFast(index - x + 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index - x + 1);
				}
				if (!flag5 && pathGrid.WalkableFast(index - x - 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index - x - 1);
				}
			}
			if (flag4 && !PathFinder.BlocksDiagonalMovement(index + x, this.map))
			{
				if (!flag6 && pathGrid.WalkableFast(index + x + 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index + x + 1);
				}
				if (!flag5 && pathGrid.WalkableFast(index + x - 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index + x - 1);
				}
			}
			return RegionCostCalculator.tmpPathableNeighborIndices;
		}

		// Token: 0x040017B1 RID: 6065
		private Map map;

		// Token: 0x040017B2 RID: 6066
		private Region[] regionGrid;

		// Token: 0x040017B3 RID: 6067
		private TraverseParms traverseParms;

		// Token: 0x040017B4 RID: 6068
		private IntVec3 destinationCell;

		// Token: 0x040017B5 RID: 6069
		private int moveTicksCardinal;

		// Token: 0x040017B6 RID: 6070
		private int moveTicksDiagonal;

		// Token: 0x040017B7 RID: 6071
		private ByteGrid avoidGrid;

		// Token: 0x040017B8 RID: 6072
		private Area allowedArea;

		// Token: 0x040017B9 RID: 6073
		private bool drafted;

		// Token: 0x040017BA RID: 6074
		private Func<int, int, float> preciseRegionLinkDistancesDistanceGetter;

		// Token: 0x040017BB RID: 6075
		private Dictionary<int, RegionLink> regionMinLink = new Dictionary<int, RegionLink>();

		// Token: 0x040017BC RID: 6076
		private Dictionary<RegionLink, int> distances = new Dictionary<RegionLink, int>();

		// Token: 0x040017BD RID: 6077
		private FastPriorityQueue<RegionCostCalculator.RegionLinkQueueEntry> queue = new FastPriorityQueue<RegionCostCalculator.RegionLinkQueueEntry>(new RegionCostCalculator.DistanceComparer());

		// Token: 0x040017BE RID: 6078
		private Dictionary<Region, int> minPathCosts = new Dictionary<Region, int>();

		// Token: 0x040017BF RID: 6079
		private List<Pair<RegionLink, int>> preciseRegionLinkDistances = new List<Pair<RegionLink, int>>();

		// Token: 0x040017C0 RID: 6080
		private Dictionary<RegionLink, IntVec3> linkTargetCells = new Dictionary<RegionLink, IntVec3>();

		// Token: 0x040017C1 RID: 6081
		private const int SampleCount = 11;

		// Token: 0x040017C2 RID: 6082
		private static int[] pathCostSamples = new int[11];

		// Token: 0x040017C3 RID: 6083
		private static List<int> tmpCellIndices = new List<int>();

		// Token: 0x040017C4 RID: 6084
		private static Dictionary<int, float> tmpDistances = new Dictionary<int, float>();

		// Token: 0x040017C5 RID: 6085
		private static List<int> tmpPathableNeighborIndices = new List<int>();

		// Token: 0x02001767 RID: 5991
		private struct RegionLinkQueueEntry
		{
			// Token: 0x17001558 RID: 5464
			// (get) Token: 0x060087DC RID: 34780 RVA: 0x002BB74D File Offset: 0x002B994D
			public Region From
			{
				get
				{
					return this.from;
				}
			}

			// Token: 0x17001559 RID: 5465
			// (get) Token: 0x060087DD RID: 34781 RVA: 0x002BB755 File Offset: 0x002B9955
			public RegionLink Link
			{
				get
				{
					return this.link;
				}
			}

			// Token: 0x1700155A RID: 5466
			// (get) Token: 0x060087DE RID: 34782 RVA: 0x002BB75D File Offset: 0x002B995D
			public int Cost
			{
				get
				{
					return this.cost;
				}
			}

			// Token: 0x1700155B RID: 5467
			// (get) Token: 0x060087DF RID: 34783 RVA: 0x002BB765 File Offset: 0x002B9965
			public int EstimatedPathCost
			{
				get
				{
					return this.estimatedPathCost;
				}
			}

			// Token: 0x060087E0 RID: 34784 RVA: 0x002BB76D File Offset: 0x002B996D
			public RegionLinkQueueEntry(Region from, RegionLink link, int cost, int estimatedPathCost)
			{
				this.from = from;
				this.link = link;
				this.cost = cost;
				this.estimatedPathCost = estimatedPathCost;
			}

			// Token: 0x04005951 RID: 22865
			private Region from;

			// Token: 0x04005952 RID: 22866
			private RegionLink link;

			// Token: 0x04005953 RID: 22867
			private int cost;

			// Token: 0x04005954 RID: 22868
			private int estimatedPathCost;
		}

		// Token: 0x02001768 RID: 5992
		private class DistanceComparer : IComparer<RegionCostCalculator.RegionLinkQueueEntry>
		{
			// Token: 0x060087E1 RID: 34785 RVA: 0x002BB78C File Offset: 0x002B998C
			public int Compare(RegionCostCalculator.RegionLinkQueueEntry a, RegionCostCalculator.RegionLinkQueueEntry b)
			{
				return a.EstimatedPathCost.CompareTo(b.EstimatedPathCost);
			}
		}
	}
}
