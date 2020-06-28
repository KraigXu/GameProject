using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020001BB RID: 443
	public class RegionAndRoomUpdater
	{
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000C5B RID: 3163 RVA: 0x00046632 File Offset: 0x00044832
		// (set) Token: 0x06000C5C RID: 3164 RVA: 0x0004663A File Offset: 0x0004483A
		public bool Enabled
		{
			get
			{
				return this.enabledInt;
			}
			set
			{
				this.enabledInt = value;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x00046643 File Offset: 0x00044843
		public bool AnythingToRebuild
		{
			get
			{
				return this.map.regionDirtyer.AnyDirty || !this.initialized;
			}
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x00046664 File Offset: 0x00044864
		public RegionAndRoomUpdater(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x000466E8 File Offset: 0x000448E8
		public void RebuildAllRegionsAndRooms()
		{
			if (!this.Enabled)
			{
				Log.Warning("Called RebuildAllRegionsAndRooms() but RegionAndRoomUpdater is disabled. Regions won't be rebuilt.", false);
			}
			this.map.temperatureCache.ResetTemperatureCache();
			this.map.regionDirtyer.SetAllDirty();
			this.TryRebuildDirtyRegionsAndRooms();
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x00046724 File Offset: 0x00044924
		public void TryRebuildDirtyRegionsAndRooms()
		{
			if (this.working || !this.Enabled)
			{
				return;
			}
			this.working = true;
			if (!this.initialized)
			{
				this.RebuildAllRegionsAndRooms();
			}
			if (!this.map.regionDirtyer.AnyDirty)
			{
				this.working = false;
				return;
			}
			try
			{
				this.RegenerateNewRegionsFromDirtyCells();
				this.CreateOrUpdateRooms();
			}
			catch (Exception arg)
			{
				Log.Error("Exception while rebuilding dirty regions: " + arg, false);
			}
			this.newRegions.Clear();
			this.map.regionDirtyer.SetAllClean();
			this.initialized = true;
			this.working = false;
			if (DebugSettings.detectRegionListersBugs)
			{
				Autotests_RegionListers.CheckBugs(this.map);
			}
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x000467E0 File Offset: 0x000449E0
		private void RegenerateNewRegionsFromDirtyCells()
		{
			this.newRegions.Clear();
			List<IntVec3> dirtyCells = this.map.regionDirtyer.DirtyCells;
			for (int i = 0; i < dirtyCells.Count; i++)
			{
				IntVec3 intVec = dirtyCells[i];
				if (intVec.GetRegion(this.map, RegionType.Set_All) == null)
				{
					Region region = this.map.regionMaker.TryGenerateRegionFrom(intVec);
					if (region != null)
					{
						this.newRegions.Add(region);
					}
				}
			}
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00046854 File Offset: 0x00044A54
		private void CreateOrUpdateRooms()
		{
			this.newRooms.Clear();
			this.reusedOldRooms.Clear();
			this.newRoomGroups.Clear();
			this.reusedOldRoomGroups.Clear();
			int numRegionGroups = this.CombineNewRegionsIntoContiguousGroups();
			this.CreateOrAttachToExistingRooms(numRegionGroups);
			int numRoomGroups = this.CombineNewAndReusedRoomsIntoContiguousGroups();
			this.CreateOrAttachToExistingRoomGroups(numRoomGroups);
			this.NotifyAffectedRoomsAndRoomGroupsAndUpdateTemperature();
			this.newRooms.Clear();
			this.reusedOldRooms.Clear();
			this.newRoomGroups.Clear();
			this.reusedOldRoomGroups.Clear();
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x000468DC File Offset: 0x00044ADC
		private int CombineNewRegionsIntoContiguousGroups()
		{
			int num = 0;
			for (int i = 0; i < this.newRegions.Count; i++)
			{
				if (this.newRegions[i].newRegionGroupIndex < 0)
				{
					RegionTraverser.FloodAndSetNewRegionIndex(this.newRegions[i], num);
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x0004692C File Offset: 0x00044B2C
		private void CreateOrAttachToExistingRooms(int numRegionGroups)
		{
			for (int i = 0; i < numRegionGroups; i++)
			{
				this.currentRegionGroup.Clear();
				for (int j = 0; j < this.newRegions.Count; j++)
				{
					if (this.newRegions[j].newRegionGroupIndex == i)
					{
						this.currentRegionGroup.Add(this.newRegions[j]);
					}
				}
				if (!this.currentRegionGroup[0].type.AllowsMultipleRegionsPerRoom())
				{
					if (this.currentRegionGroup.Count != 1)
					{
						Log.Error("Region type doesn't allow multiple regions per room but there are >1 regions in this group.", false);
					}
					Room room = Room.MakeNew(this.map);
					this.currentRegionGroup[0].Room = room;
					this.newRooms.Add(room);
				}
				else
				{
					bool flag;
					Room room2 = this.FindCurrentRegionGroupNeighborWithMostRegions(out flag);
					if (room2 == null)
					{
						Room item = RegionTraverser.FloodAndSetRooms(this.currentRegionGroup[0], this.map, null);
						this.newRooms.Add(item);
					}
					else if (!flag)
					{
						for (int k = 0; k < this.currentRegionGroup.Count; k++)
						{
							this.currentRegionGroup[k].Room = room2;
						}
						this.reusedOldRooms.Add(room2);
					}
					else
					{
						RegionTraverser.FloodAndSetRooms(this.currentRegionGroup[0], this.map, room2);
						this.reusedOldRooms.Add(room2);
					}
				}
			}
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00046A94 File Offset: 0x00044C94
		private int CombineNewAndReusedRoomsIntoContiguousGroups()
		{
			int num = 0;
			foreach (Room room in this.reusedOldRooms)
			{
				room.newOrReusedRoomGroupIndex = -1;
			}
			foreach (Room room2 in this.reusedOldRooms.Concat(this.newRooms))
			{
				if (room2.newOrReusedRoomGroupIndex < 0)
				{
					this.tmpRoomStack.Clear();
					this.tmpRoomStack.Push(room2);
					room2.newOrReusedRoomGroupIndex = num;
					while (this.tmpRoomStack.Count != 0)
					{
						Room room3 = this.tmpRoomStack.Pop();
						foreach (Room room4 in room3.Neighbors)
						{
							if (room4.newOrReusedRoomGroupIndex < 0 && this.ShouldBeInTheSameRoomGroup(room3, room4))
							{
								room4.newOrReusedRoomGroupIndex = num;
								this.tmpRoomStack.Push(room4);
							}
						}
					}
					this.tmpRoomStack.Clear();
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x00046BEC File Offset: 0x00044DEC
		private void CreateOrAttachToExistingRoomGroups(int numRoomGroups)
		{
			for (int i = 0; i < numRoomGroups; i++)
			{
				this.currentRoomGroup.Clear();
				foreach (Room room in this.reusedOldRooms)
				{
					if (room.newOrReusedRoomGroupIndex == i)
					{
						this.currentRoomGroup.Add(room);
					}
				}
				for (int j = 0; j < this.newRooms.Count; j++)
				{
					if (this.newRooms[j].newOrReusedRoomGroupIndex == i)
					{
						this.currentRoomGroup.Add(this.newRooms[j]);
					}
				}
				bool flag;
				RoomGroup roomGroup = this.FindCurrentRoomGroupNeighborWithMostRegions(out flag);
				if (roomGroup == null)
				{
					RoomGroup roomGroup2 = RoomGroup.MakeNew(this.map);
					this.FloodAndSetRoomGroups(this.currentRoomGroup[0], roomGroup2);
					this.newRoomGroups.Add(roomGroup2);
				}
				else if (!flag)
				{
					for (int k = 0; k < this.currentRoomGroup.Count; k++)
					{
						this.currentRoomGroup[k].Group = roomGroup;
					}
					this.reusedOldRoomGroups.Add(roomGroup);
				}
				else
				{
					this.FloodAndSetRoomGroups(this.currentRoomGroup[0], roomGroup);
					this.reusedOldRoomGroups.Add(roomGroup);
				}
			}
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x00046D50 File Offset: 0x00044F50
		private void FloodAndSetRoomGroups(Room start, RoomGroup roomGroup)
		{
			this.tmpRoomStack.Clear();
			this.tmpRoomStack.Push(start);
			this.tmpVisitedRooms.Clear();
			this.tmpVisitedRooms.Add(start);
			while (this.tmpRoomStack.Count != 0)
			{
				Room room = this.tmpRoomStack.Pop();
				room.Group = roomGroup;
				foreach (Room room2 in room.Neighbors)
				{
					if (!this.tmpVisitedRooms.Contains(room2) && this.ShouldBeInTheSameRoomGroup(room, room2))
					{
						this.tmpRoomStack.Push(room2);
						this.tmpVisitedRooms.Add(room2);
					}
				}
			}
			this.tmpVisitedRooms.Clear();
			this.tmpRoomStack.Clear();
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x00046E34 File Offset: 0x00045034
		private void NotifyAffectedRoomsAndRoomGroupsAndUpdateTemperature()
		{
			foreach (Room room in this.reusedOldRooms)
			{
				room.Notify_RoomShapeOrContainedBedsChanged();
			}
			for (int i = 0; i < this.newRooms.Count; i++)
			{
				this.newRooms[i].Notify_RoomShapeOrContainedBedsChanged();
			}
			foreach (RoomGroup roomGroup in this.reusedOldRoomGroups)
			{
				roomGroup.Notify_RoomGroupShapeChanged();
			}
			for (int j = 0; j < this.newRoomGroups.Count; j++)
			{
				RoomGroup roomGroup2 = this.newRoomGroups[j];
				roomGroup2.Notify_RoomGroupShapeChanged();
				float temperature;
				if (this.map.temperatureCache.TryGetAverageCachedRoomGroupTemp(roomGroup2, out temperature))
				{
					roomGroup2.Temperature = temperature;
				}
			}
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x00046F34 File Offset: 0x00045134
		private Room FindCurrentRegionGroupNeighborWithMostRegions(out bool multipleOldNeighborRooms)
		{
			multipleOldNeighborRooms = false;
			Room room = null;
			for (int i = 0; i < this.currentRegionGroup.Count; i++)
			{
				foreach (Region region in this.currentRegionGroup[i].NeighborsOfSameType)
				{
					if (region.Room != null && !this.reusedOldRooms.Contains(region.Room))
					{
						if (room == null)
						{
							room = region.Room;
						}
						else if (region.Room != room)
						{
							multipleOldNeighborRooms = true;
							if (region.Room.RegionCount > room.RegionCount)
							{
								room = region.Room;
							}
						}
					}
				}
			}
			return room;
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x00046FF4 File Offset: 0x000451F4
		private RoomGroup FindCurrentRoomGroupNeighborWithMostRegions(out bool multipleOldNeighborRoomGroups)
		{
			multipleOldNeighborRoomGroups = false;
			RoomGroup roomGroup = null;
			for (int i = 0; i < this.currentRoomGroup.Count; i++)
			{
				foreach (Room room in this.currentRoomGroup[i].Neighbors)
				{
					if (room.Group != null && this.ShouldBeInTheSameRoomGroup(this.currentRoomGroup[i], room) && !this.reusedOldRoomGroups.Contains(room.Group))
					{
						if (roomGroup == null)
						{
							roomGroup = room.Group;
						}
						else if (room.Group != roomGroup)
						{
							multipleOldNeighborRoomGroups = true;
							if (room.Group.RegionCount > roomGroup.RegionCount)
							{
								roomGroup = room.Group;
							}
						}
					}
				}
			}
			return roomGroup;
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x000470D0 File Offset: 0x000452D0
		private bool ShouldBeInTheSameRoomGroup(Room a, Room b)
		{
			RegionType regionType = a.RegionType;
			RegionType regionType2 = b.RegionType;
			return (regionType == RegionType.Normal || regionType == RegionType.ImpassableFreeAirExchange) && (regionType2 == RegionType.Normal || regionType2 == RegionType.ImpassableFreeAirExchange);
		}

		// Token: 0x040009CA RID: 2506
		private Map map;

		// Token: 0x040009CB RID: 2507
		private List<Region> newRegions = new List<Region>();

		// Token: 0x040009CC RID: 2508
		private List<Room> newRooms = new List<Room>();

		// Token: 0x040009CD RID: 2509
		private HashSet<Room> reusedOldRooms = new HashSet<Room>();

		// Token: 0x040009CE RID: 2510
		private List<RoomGroup> newRoomGroups = new List<RoomGroup>();

		// Token: 0x040009CF RID: 2511
		private HashSet<RoomGroup> reusedOldRoomGroups = new HashSet<RoomGroup>();

		// Token: 0x040009D0 RID: 2512
		private List<Region> currentRegionGroup = new List<Region>();

		// Token: 0x040009D1 RID: 2513
		private List<Room> currentRoomGroup = new List<Room>();

		// Token: 0x040009D2 RID: 2514
		private Stack<Room> tmpRoomStack = new Stack<Room>();

		// Token: 0x040009D3 RID: 2515
		private HashSet<Room> tmpVisitedRooms = new HashSet<Room>();

		// Token: 0x040009D4 RID: 2516
		private bool initialized;

		// Token: 0x040009D5 RID: 2517
		private bool working;

		// Token: 0x040009D6 RID: 2518
		private bool enabledInt = true;
	}
}
