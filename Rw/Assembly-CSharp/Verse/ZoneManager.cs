using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001EC RID: 492
	public sealed class ZoneManager : IExposable
	{
		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000DE2 RID: 3554 RVA: 0x0004F2DC File Offset: 0x0004D4DC
		public List<Zone> AllZones
		{
			get
			{
				return this.allZones;
			}
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0004F2E4 File Offset: 0x0004D4E4
		public ZoneManager(Map map)
		{
			this.map = map;
			this.zoneGrid = new Zone[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0004F314 File Offset: 0x0004D514
		public void ExposeData()
		{
			Scribe_Collections.Look<Zone>(ref this.allZones, "allZones", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateZoneManagerLinks();
				this.RebuildZoneGrid();
			}
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0004F340 File Offset: 0x0004D540
		private void UpdateZoneManagerLinks()
		{
			for (int i = 0; i < this.allZones.Count; i++)
			{
				this.allZones[i].zoneManager = this;
			}
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0004F378 File Offset: 0x0004D578
		private void RebuildZoneGrid()
		{
			CellIndices cellIndices = this.map.cellIndices;
			this.zoneGrid = new Zone[cellIndices.NumGridCells];
			foreach (Zone zone in this.allZones)
			{
				foreach (IntVec3 c in zone)
				{
					this.zoneGrid[cellIndices.CellToIndex(c)] = zone;
				}
			}
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0004F424 File Offset: 0x0004D624
		public void RegisterZone(Zone newZone)
		{
			this.allZones.Add(newZone);
			newZone.PostRegister();
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0004F438 File Offset: 0x0004D638
		public void DeregisterZone(Zone oldZone)
		{
			this.allZones.Remove(oldZone);
			oldZone.PostDeregister();
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0004F44D File Offset: 0x0004D64D
		internal void AddZoneGridCell(Zone zone, IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = zone;
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0004F468 File Offset: 0x0004D668
		internal void ClearZoneGridCell(IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = null;
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0004F483 File Offset: 0x0004D683
		public Zone ZoneAt(IntVec3 c)
		{
			return this.zoneGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0004F4A0 File Offset: 0x0004D6A0
		public string NewZoneName(string nameBase)
		{
			for (int i = 1; i <= 1000; i++)
			{
				string cand = nameBase + " " + i;
				if (!this.allZones.Any((Zone z) => z.label == cand))
				{
					return cand;
				}
			}
			Log.Error("Ran out of zone names.", false);
			return "Zone X";
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0004F50C File Offset: 0x0004D70C
		internal void Notify_NoZoneOverlapThingSpawned(Thing thing)
		{
			CellRect cellRect = thing.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					Zone zone = this.ZoneAt(c);
					if (zone != null)
					{
						zone.RemoveCell(c);
						zone.CheckContiguous();
					}
				}
			}
		}

		// Token: 0x04000A97 RID: 2711
		public Map map;

		// Token: 0x04000A98 RID: 2712
		private List<Zone> allZones = new List<Zone>();

		// Token: 0x04000A99 RID: 2713
		private Zone[] zoneGrid;
	}
}
