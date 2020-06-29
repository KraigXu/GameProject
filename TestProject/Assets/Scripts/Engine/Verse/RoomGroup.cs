using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	
	public class RoomGroup
	{
		
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x00049825 File Offset: 0x00047A25
		public List<Room> Rooms
		{
			get
			{
				return this.rooms;
			}
		}

		
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x0004982D File Offset: 0x00047A2D
		public Map Map
		{
			get
			{
				if (!this.rooms.Any<Room>())
				{
					return null;
				}
				return this.rooms[0].Map;
			}
		}

		
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x0004984F File Offset: 0x00047A4F
		public int RoomCount
		{
			get
			{
				return this.rooms.Count;
			}
		}

		
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x0004985C File Offset: 0x00047A5C
		public RoomGroupTempTracker TempTracker
		{
			get
			{
				return this.tempTracker;
			}
		}

		
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x00049864 File Offset: 0x00047A64
		// (set) Token: 0x06000CF0 RID: 3312 RVA: 0x00049871 File Offset: 0x00047A71
		public float Temperature
		{
			get
			{
				return this.tempTracker.Temperature;
			}
			set
			{
				this.tempTracker.Temperature = value;
			}
		}

		
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x0004987F File Offset: 0x00047A7F
		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.AnyRoomTouchesMapEdge || this.OpenRoofCount >= Mathf.CeilToInt((float)this.CellCount * 0.25f);
			}
		}

		
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x000498A8 File Offset: 0x00047AA8
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int i = 0; i < this.rooms.Count; i = num + 1)
				{
					foreach (IntVec3 intVec in this.rooms[i].Cells)
					{
						yield return intVec;
					}
					IEnumerator<IntVec3> enumerator = null;
					num = i;
				}
				yield break;
				yield break;
			}
		}

		
		// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x000498B8 File Offset: 0x00047AB8
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					for (int i = 0; i < this.rooms.Count; i++)
					{
						this.cachedCellCount += this.rooms[i].CellCount;
					}
				}
				return this.cachedCellCount;
			}
		}

		
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x0004990F File Offset: 0x00047B0F
		public IEnumerable<Region> Regions
		{
			get
			{
				int num;
				for (int i = 0; i < this.rooms.Count; i = num + 1)
				{
					foreach (Region region in this.rooms[i].Regions)
					{
						yield return region;
					}
					List<Region>.Enumerator enumerator = default(List<Region>.Enumerator);
					num = i;
				}
				yield break;
				yield break;
			}
		}

		
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x00049920 File Offset: 0x00047B20
		public int RegionCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.rooms.Count; i++)
				{
					num += this.rooms[i].RegionCount;
				}
				return num;
			}
		}

		
		// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x0004995C File Offset: 0x00047B5C
		public int OpenRoofCount
		{
			get
			{
				if (this.cachedOpenRoofCount == -1)
				{
					this.cachedOpenRoofCount = 0;
					for (int i = 0; i < this.rooms.Count; i++)
					{
						this.cachedOpenRoofCount += this.rooms[i].OpenRoofCount;
					}
				}
				return this.cachedOpenRoofCount;
			}
		}

		
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x000499B4 File Offset: 0x00047BB4
		public bool AnyRoomTouchesMapEdge
		{
			get
			{
				for (int i = 0; i < this.rooms.Count; i++)
				{
					if (this.rooms[i].TouchesMapEdge)
					{
						return true;
					}
				}
				return false;
			}
		}

		
		public static RoomGroup MakeNew(Map map)
		{
			RoomGroup roomGroup = new RoomGroup();
			roomGroup.ID = RoomGroup.nextRoomGroupID;
			roomGroup.tempTracker = new RoomGroupTempTracker(roomGroup, map);
			RoomGroup.nextRoomGroupID++;
			return roomGroup;
		}

		
		public void AddRoom(Room room)
		{
			if (this.rooms.Contains(room))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to add the same room twice to RoomGroup. room=",
					room,
					", roomGroup=",
					this
				}), false);
				return;
			}
			this.rooms.Add(room);
		}

		
		public void RemoveRoom(Room room)
		{
			if (!this.rooms.Contains(room))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to remove room from RoomGroup but this room is not here. room=",
					room,
					", roomGroup=",
					this
				}), false);
				return;
			}
			this.rooms.Remove(room);
		}

		
		public bool PushHeat(float energy)
		{
			if (this.UsesOutdoorTemperature)
			{
				return false;
			}
			this.Temperature += energy / (float)this.CellCount;
			return true;
		}

		
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.tempTracker.RoofChanged();
		}

		
		public void Notify_RoomGroupShapeChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			this.tempTracker.RoomChanged();
		}

		
		public string DebugString()
		{
			return string.Concat(new object[]
			{
				"RoomGroup ID=",
				this.ID,
				"\n  first cell=",
				this.Cells.FirstOrDefault<IntVec3>(),
				"\n  RoomCount=",
				this.RoomCount,
				"\n  RegionCount=",
				this.RegionCount,
				"\n  CellCount=",
				this.CellCount,
				"\n  OpenRoofCount=",
				this.OpenRoofCount,
				"\n  ",
				this.tempTracker.DebugString()
			});
		}

		
		internal void DebugDraw()
		{
			int num = Gen.HashCombineInt(this.GetHashCode(), 1948571531);
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)num * 0.01f);
			}
			this.tempTracker.DebugDraw();
		}

		
		public int ID = -1;

		
		private List<Room> rooms = new List<Room>();

		
		private RoomGroupTempTracker tempTracker;

		
		private int cachedOpenRoofCount = -1;

		
		private int cachedCellCount = -1;

		
		private static int nextRoomGroupID;

		
		private const float UseOutdoorTemperatureUnroofedFraction = 0.25f;
	}
}
