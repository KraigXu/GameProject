using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001BD RID: 445
	public sealed class RegionGrid
	{
		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x00047648 File Offset: 0x00045848
		public Region[] DirectGrid
		{
			get
			{
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get the region grid but RegionAndRoomUpdater is disabled. The result may be incorrect.", false);
				}
				this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				return this.regionGrid;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x0004769A File Offset: 0x0004589A
		public IEnumerable<Region> AllRegions_NoRebuild_InvalidAllowed
		{
			get
			{
				RegionGrid.allRegionsYielded.Clear();
				try
				{
					int count = this.map.cellIndices.NumGridCells;
					int num;
					for (int i = 0; i < count; i = num + 1)
					{
						if (this.regionGrid[i] != null && !RegionGrid.allRegionsYielded.Contains(this.regionGrid[i]))
						{
							yield return this.regionGrid[i];
							RegionGrid.allRegionsYielded.Add(this.regionGrid[i]);
						}
						num = i;
					}
				}
				finally
				{
					RegionGrid.allRegionsYielded.Clear();
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x000476AA File Offset: 0x000458AA
		public IEnumerable<Region> AllRegions
		{
			get
			{
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get all valid regions but RegionAndRoomUpdater is disabled. The result may be incorrect.", false);
				}
				this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				RegionGrid.allRegionsYielded.Clear();
				try
				{
					int count = this.map.cellIndices.NumGridCells;
					int num;
					for (int i = 0; i < count; i = num + 1)
					{
						if (this.regionGrid[i] != null && this.regionGrid[i].valid && !RegionGrid.allRegionsYielded.Contains(this.regionGrid[i]))
						{
							yield return this.regionGrid[i];
							RegionGrid.allRegionsYielded.Add(this.regionGrid[i]);
						}
						num = i;
					}
				}
				finally
				{
					RegionGrid.allRegionsYielded.Clear();
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x000476BA File Offset: 0x000458BA
		public RegionGrid(Map map)
		{
			this.map = map;
			this.regionGrid = new Region[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x000476F8 File Offset: 0x000458F8
		public Region GetValidRegionAt(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error("Tried to get valid region out of bounds at " + c, false);
				return null;
			}
			if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
			{
				Log.Warning("Trying to get valid region at " + c + " but RegionAndRoomUpdater is disabled. The result may be incorrect.", false);
			}
			this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
			Region region = this.regionGrid[this.map.cellIndices.CellToIndex(c)];
			if (region != null && region.valid)
			{
				return region;
			}
			return null;
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x000477A4 File Offset: 0x000459A4
		public Region GetValidRegionAt_NoRebuild(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error("Tried to get valid region out of bounds at " + c, false);
				return null;
			}
			Region region = this.regionGrid[this.map.cellIndices.CellToIndex(c)];
			if (region != null && region.valid)
			{
				return region;
			}
			return null;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x000477FE File Offset: 0x000459FE
		public Region GetRegionAt_NoRebuild_InvalidAllowed(IntVec3 c)
		{
			return this.regionGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00047818 File Offset: 0x00045A18
		public void SetRegionAt(IntVec3 c, Region reg)
		{
			this.regionGrid[this.map.cellIndices.CellToIndex(c)] = reg;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00047834 File Offset: 0x00045A34
		public void UpdateClean()
		{
			for (int i = 0; i < 16; i++)
			{
				if (this.curCleanIndex >= this.regionGrid.Length)
				{
					this.curCleanIndex = 0;
				}
				Region region = this.regionGrid[this.curCleanIndex];
				if (region != null && !region.valid)
				{
					this.regionGrid[this.curCleanIndex] = null;
				}
				this.curCleanIndex++;
			}
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x0004789C File Offset: 0x00045A9C
		public void DebugDraw()
		{
			if (this.map != Find.CurrentMap)
			{
				return;
			}
			if (DebugViewSettings.drawRegionTraversal)
			{
				CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
				currentViewRect.ClipInsideMap(this.map);
				foreach (IntVec3 c in currentViewRect)
				{
					Region validRegionAt = this.GetValidRegionAt(c);
					if (validRegionAt != null && !this.drawnRegions.Contains(validRegionAt))
					{
						validRegionAt.DebugDraw();
						this.drawnRegions.Add(validRegionAt);
					}
				}
				this.drawnRegions.Clear();
			}
			IntVec3 intVec = UI.MouseCell();
			if (intVec.InBounds(this.map))
			{
				if (DebugViewSettings.drawRooms)
				{
					Room room = intVec.GetRoom(this.map, RegionType.Set_All);
					if (room != null)
					{
						room.DebugDraw();
					}
				}
				if (DebugViewSettings.drawRoomGroups)
				{
					RoomGroup roomGroup = intVec.GetRoomGroup(this.map);
					if (roomGroup != null)
					{
						roomGroup.DebugDraw();
					}
				}
				if (DebugViewSettings.drawRegions || DebugViewSettings.drawRegionLinks || DebugViewSettings.drawRegionThings)
				{
					Region regionAt_NoRebuild_InvalidAllowed = this.GetRegionAt_NoRebuild_InvalidAllowed(intVec);
					if (regionAt_NoRebuild_InvalidAllowed != null)
					{
						regionAt_NoRebuild_InvalidAllowed.DebugDrawMouseover();
					}
				}
			}
		}

		// Token: 0x040009DA RID: 2522
		private Map map;

		// Token: 0x040009DB RID: 2523
		private Region[] regionGrid;

		// Token: 0x040009DC RID: 2524
		private int curCleanIndex;

		// Token: 0x040009DD RID: 2525
		public List<Room> allRooms = new List<Room>();

		// Token: 0x040009DE RID: 2526
		public static HashSet<Region> allRegionsYielded = new HashSet<Region>();

		// Token: 0x040009DF RID: 2527
		private const int CleanSquaresPerFrame = 16;

		// Token: 0x040009E0 RID: 2528
		public HashSet<Region> drawnRegions = new HashSet<Region>();
	}
}
