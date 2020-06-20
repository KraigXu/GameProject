using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x0200057B RID: 1403
	public class RegionCostCalculatorWrapper
	{
		// Token: 0x060027C9 RID: 10185 RVA: 0x000E982C File Offset: 0x000E7A2C
		public RegionCostCalculatorWrapper(Map map)
		{
			this.map = map;
			this.regionCostCalculator = new RegionCostCalculator(map);
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x000E9854 File Offset: 0x000E7A54
		public void Init(CellRect end, TraverseParms traverseParms, int moveTicksCardinal, int moveTicksDiagonal, ByteGrid avoidGrid, Area allowedArea, bool drafted, List<int> disallowedCorners)
		{
			this.moveTicksCardinal = moveTicksCardinal;
			this.moveTicksDiagonal = moveTicksDiagonal;
			this.endCell = end.CenterCell;
			this.cachedRegion = null;
			this.cachedBestLink = null;
			this.cachedSecondBestLink = null;
			this.cachedBestLinkCost = 0;
			this.cachedSecondBestLinkCost = 0;
			this.cachedRegionIsDestination = false;
			this.regionGrid = this.map.regionGrid.DirectGrid;
			this.destRegions.Clear();
			if (end.Width == 1 && end.Height == 1)
			{
				Region region = this.endCell.GetRegion(this.map, RegionType.Set_Passable);
				if (region != null)
				{
					this.destRegions.Add(region);
				}
			}
			else
			{
				foreach (IntVec3 intVec in end)
				{
					if (intVec.InBounds(this.map) && !disallowedCorners.Contains(this.map.cellIndices.CellToIndex(intVec)))
					{
						Region region2 = intVec.GetRegion(this.map, RegionType.Set_Passable);
						if (region2 != null && region2.Allows(traverseParms, true))
						{
							this.destRegions.Add(region2);
						}
					}
				}
			}
			if (this.destRegions.Count == 0)
			{
				Log.Error("Couldn't find any destination regions. This shouldn't ever happen because we've checked reachability.", false);
			}
			this.regionCostCalculator.Init(end, this.destRegions, traverseParms, moveTicksCardinal, moveTicksDiagonal, avoidGrid, allowedArea, drafted);
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x000E99C4 File Offset: 0x000E7BC4
		public int GetPathCostFromDestToRegion(int cellIndex)
		{
			Region region = this.regionGrid[cellIndex];
			IntVec3 cell = this.map.cellIndices.IndexToCell(cellIndex);
			if (region != this.cachedRegion)
			{
				this.cachedRegionIsDestination = this.destRegions.Contains(region);
				if (this.cachedRegionIsDestination)
				{
					return this.OctileDistanceToEnd(cell);
				}
				this.cachedBestLinkCost = this.regionCostCalculator.GetRegionBestDistances(region, out this.cachedBestLink, out this.cachedSecondBestLink, out this.cachedSecondBestLinkCost);
				this.cachedRegion = region;
			}
			else if (this.cachedRegionIsDestination)
			{
				return this.OctileDistanceToEnd(cell);
			}
			if (this.cachedBestLink != null)
			{
				int num = this.regionCostCalculator.RegionLinkDistance(cell, this.cachedBestLink, 1);
				int num3;
				if (this.cachedSecondBestLink != null)
				{
					int num2 = this.regionCostCalculator.RegionLinkDistance(cell, this.cachedSecondBestLink, 1);
					num3 = Mathf.Min(this.cachedSecondBestLinkCost + num2, this.cachedBestLinkCost + num);
				}
				else
				{
					num3 = this.cachedBestLinkCost + num;
				}
				return num3 + this.OctileDistanceToEndEps(cell);
			}
			return 10000;
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x000E9AC0 File Offset: 0x000E7CC0
		private int OctileDistanceToEnd(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, this.moveTicksCardinal, this.moveTicksDiagonal);
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x000E9B10 File Offset: 0x000E7D10
		private int OctileDistanceToEndEps(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, 2, 3);
		}

		// Token: 0x040017C6 RID: 6086
		private Map map;

		// Token: 0x040017C7 RID: 6087
		private IntVec3 endCell;

		// Token: 0x040017C8 RID: 6088
		private HashSet<Region> destRegions = new HashSet<Region>();

		// Token: 0x040017C9 RID: 6089
		private int moveTicksCardinal;

		// Token: 0x040017CA RID: 6090
		private int moveTicksDiagonal;

		// Token: 0x040017CB RID: 6091
		private RegionCostCalculator regionCostCalculator;

		// Token: 0x040017CC RID: 6092
		private Region cachedRegion;

		// Token: 0x040017CD RID: 6093
		private RegionLink cachedBestLink;

		// Token: 0x040017CE RID: 6094
		private RegionLink cachedSecondBestLink;

		// Token: 0x040017CF RID: 6095
		private int cachedBestLinkCost;

		// Token: 0x040017D0 RID: 6096
		private int cachedSecondBestLinkCost;

		// Token: 0x040017D1 RID: 6097
		private bool cachedRegionIsDestination;

		// Token: 0x040017D2 RID: 6098
		private Region[] regionGrid;
	}
}
