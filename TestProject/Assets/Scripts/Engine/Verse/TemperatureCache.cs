using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001DB RID: 475
	public sealed class TemperatureCache : IExposable
	{
		// Token: 0x06000D76 RID: 3446 RVA: 0x0004CDC4 File Offset: 0x0004AFC4
		public TemperatureCache(Map map)
		{
			this.map = map;
			this.tempCache = new CachedTempInfo[map.cellIndices.NumGridCells];
			this.temperatureSaveLoad = new TemperatureSaveLoad(map);
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0004CE18 File Offset: 0x0004B018
		public void ResetTemperatureCache()
		{
			int numGridCells = this.map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				this.tempCache[i].Reset();
			}
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0004CE53 File Offset: 0x0004B053
		public void ExposeData()
		{
			this.temperatureSaveLoad.DoExposeWork();
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0004CE60 File Offset: 0x0004B060
		public void ResetCachedCellInfo(IntVec3 c)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)].Reset();
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0004CE83 File Offset: 0x0004B083
		private void SetCachedCellInfo(IntVec3 c, CachedTempInfo info)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)] = info;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0004CEA4 File Offset: 0x0004B0A4
		public void TryCacheRegionTempInfo(IntVec3 c, Region reg)
		{
			Room room = reg.Room;
			if (room != null)
			{
				RoomGroup group = room.Group;
				this.SetCachedCellInfo(c, new CachedTempInfo(group.ID, group.CellCount, group.Temperature));
			}
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0004CEE0 File Offset: 0x0004B0E0
		public bool TryGetAverageCachedRoomGroupTemp(RoomGroup r, out float result)
		{
			CellIndices cellIndices = this.map.cellIndices;
			foreach (IntVec3 c in r.Cells)
			{
				CachedTempInfo cachedTempInfo = this.map.temperatureCache.tempCache[cellIndices.CellToIndex(c)];
				if (cachedTempInfo.numCells > 0 && !this.processedRoomGroupIDs.Contains(cachedTempInfo.roomGroupID))
				{
					this.relevantTempInfoList.Add(cachedTempInfo);
					this.processedRoomGroupIDs.Add(cachedTempInfo.roomGroupID);
				}
			}
			int num = 0;
			float num2 = 0f;
			foreach (CachedTempInfo cachedTempInfo2 in this.relevantTempInfoList)
			{
				num += cachedTempInfo2.numCells;
				num2 += cachedTempInfo2.temperature * (float)cachedTempInfo2.numCells;
			}
			result = num2 / (float)num;
			bool result2 = !this.relevantTempInfoList.NullOrEmpty<CachedTempInfo>();
			this.processedRoomGroupIDs.Clear();
			this.relevantTempInfoList.Clear();
			return result2;
		}

		// Token: 0x04000A4A RID: 2634
		private Map map;

		// Token: 0x04000A4B RID: 2635
		internal TemperatureSaveLoad temperatureSaveLoad;

		// Token: 0x04000A4C RID: 2636
		public CachedTempInfo[] tempCache;

		// Token: 0x04000A4D RID: 2637
		private HashSet<int> processedRoomGroupIDs = new HashSet<int>();

		// Token: 0x04000A4E RID: 2638
		private List<CachedTempInfo> relevantTempInfoList = new List<CachedTempInfo>();
	}
}
