﻿using System;
using System.Collections.Generic;

namespace Verse
{
	
	public sealed class ZoneManager : IExposable
	{
		
		// (get) Token: 0x06000DE2 RID: 3554 RVA: 0x0004F2DC File Offset: 0x0004D4DC
		public List<Zone> AllZones
		{
			get
			{
				return this.allZones;
			}
		}

		
		public ZoneManager(Map map)
		{
			this.map = map;
			this.zoneGrid = new Zone[map.cellIndices.NumGridCells];
		}

		
		public void ExposeData()
		{
			Scribe_Collections.Look<Zone>(ref this.allZones, "allZones", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateZoneManagerLinks();
				this.RebuildZoneGrid();
			}
		}

		
		private void UpdateZoneManagerLinks()
		{
			for (int i = 0; i < this.allZones.Count; i++)
			{
				this.allZones[i].zoneManager = this;
			}
		}

		
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

		
		public void RegisterZone(Zone newZone)
		{
			this.allZones.Add(newZone);
			newZone.PostRegister();
		}

		
		public void DeregisterZone(Zone oldZone)
		{
			this.allZones.Remove(oldZone);
			oldZone.PostDeregister();
		}

		
		internal void AddZoneGridCell(Zone zone, IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = zone;
		}

		
		internal void ClearZoneGridCell(IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = null;
		}

		
		public Zone ZoneAt(IntVec3 c)
		{
			return this.zoneGrid[this.map.cellIndices.CellToIndex(c)];
		}

		
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

		
		public Map map;

		
		private List<Zone> allZones = new List<Zone>();

		
		private Zone[] zoneGrid;
	}
}
