using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public sealed class Room
	{
		
		
		public Map Map
		{
			get
			{
				if (this.mapIndex >= 0)
				{
					return Find.Maps[(int)this.mapIndex];
				}
				return null;
			}
		}

		
		
		public RegionType RegionType
		{
			get
			{
				if (!this.regions.Any<Region>())
				{
					return RegionType.None;
				}
				return this.regions[0].type;
			}
		}

		
		
		public List<Region> Regions
		{
			get
			{
				return this.regions;
			}
		}

		
		
		public int RegionCount
		{
			get
			{
				return this.regions.Count;
			}
		}

		
		
		public bool IsHuge
		{
			get
			{
				return this.regions.Count > 60;
			}
		}

		
		
		public bool Dereferenced
		{
			get
			{
				return this.regions.Count == 0;
			}
		}

		
		
		public bool TouchesMapEdge
		{
			get
			{
				return this.numRegionsTouchingMapEdge > 0;
			}
		}

		
		
		public float Temperature
		{
			get
			{
				return this.Group.Temperature;
			}
		}

		
		
		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.Group.UsesOutdoorTemperature;
			}
		}

		
		
		
		public RoomGroup Group
		{
			get
			{
				return this.groupInt;
			}
			set
			{
				if (value == this.groupInt)
				{
					return;
				}
				if (this.groupInt != null)
				{
					this.groupInt.RemoveRoom(this);
				}
				this.groupInt = value;
				if (this.groupInt != null)
				{
					this.groupInt.AddRoom(this);
				}
			}
		}

		
		
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					for (int i = 0; i < this.regions.Count; i++)
					{
						this.cachedCellCount += this.regions[i].CellCount;
					}
				}
				return this.cachedCellCount;
			}
		}

		
		
		public int OpenRoofCount
		{
			get
			{
				return this.OpenRoofCountStopAt(int.MaxValue);
			}
		}

		
		
		public bool PsychologicallyOutdoors
		{
			get
			{
				return this.OpenRoofCountStopAt(300) >= 300 || (this.Group.AnyRoomTouchesMapEdge && (float)this.OpenRoofCount / (float)this.CellCount >= 0.5f);
			}
		}

		
		
		public bool OutdoorsForWork
		{
			get
			{
				return this.OpenRoofCountStopAt(101) > 100 || (float)this.OpenRoofCount > (float)this.CellCount * 0.25f;
			}
		}

		
		
		public List<Room> Neighbors
		{
			get
			{
				this.uniqueNeighborsSet.Clear();
				this.uniqueNeighbors.Clear();
				for (int i = 0; i < this.regions.Count; i++)
				{
					foreach (Region region in this.regions[i].Neighbors)
					{
						if (this.uniqueNeighborsSet.Add(region.Room) && region.Room != this)
						{
							this.uniqueNeighbors.Add(region.Room);
						}
					}
				}
				this.uniqueNeighborsSet.Clear();
				return this.uniqueNeighbors;
			}
		}

		
		
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int i = 0; i < this.regions.Count; i = num + 1)
				{
					foreach (IntVec3 intVec in this.regions[i].Cells)
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

		
		
		public IEnumerable<IntVec3> BorderCells
		{
			get
			{
				foreach (IntVec3 c in this.Cells)
				{
					int num;
					for (int i = 0; i < 8; i = num)
					{
						IntVec3 intVec = c + GenAdj.AdjacentCells[i];
						Region region = (c + GenAdj.AdjacentCells[i]).GetRegion(this.Map, RegionType.Set_Passable);
						if (region == null || region.Room != this)
						{
							yield return intVec;
						}
						num = i + 1;
					}
				}
				IEnumerator<IntVec3> enumerator = null;
				yield break;
				yield break;
			}
		}

		
		
		public IEnumerable<Pawn> Owners
		{
			get
			{
				if (this.TouchesMapEdge)
				{
					yield break;
				}
				if (this.IsHuge)
				{
					yield break;
				}
				if (this.Role != RoomRoleDefOf.Bedroom && this.Role != RoomRoleDefOf.PrisonCell && this.Role != RoomRoleDefOf.Barracks && this.Role != RoomRoleDefOf.PrisonBarracks)
				{
					yield break;
				}
				Pawn pawn = null;
				Pawn secondOwner = null;
				foreach (Building_Bed building_Bed in this.ContainedBeds)
				{
					if (building_Bed.def.building.bed_humanlike)
					{
						for (int i = 0; i < building_Bed.OwnersForReading.Count; i++)
						{
							if (pawn == null)
							{
								pawn = building_Bed.OwnersForReading[i];
							}
							else
							{
								if (secondOwner != null)
								{
									yield break;
								}
								secondOwner = building_Bed.OwnersForReading[i];
							}
						}
					}
				}
				if (pawn != null)
				{
					if (secondOwner == null)
					{
						yield return pawn;
					}
					else if (LovePartnerRelationUtility.LovePartnerRelationExists(pawn, secondOwner))
					{
						yield return pawn;
						yield return secondOwner;
					}
				}
				yield break;
			}
		}

		
		
		public IEnumerable<Building_Bed> ContainedBeds
		{
			get
			{
				List<Thing> things = this.ContainedAndAdjacentThings;
				int num;
				for (int i = 0; i < things.Count; i = num + 1)
				{
					Building_Bed building_Bed = things[i] as Building_Bed;
					if (building_Bed != null)
					{
						yield return building_Bed;
					}
					num = i;
				}
				yield break;
			}
		}

		
		
		public bool Fogged
		{
			get
			{
				return this.regions.Count != 0 && this.regions[0].AnyCell.Fogged(this.Map);
			}
		}

		
		
		public bool IsDoorway
		{
			get
			{
				return this.regions.Count == 1 && this.regions[0].IsDoorway;
			}
		}

		
		
		public List<Thing> ContainedAndAdjacentThings
		{
			get
			{
				this.uniqueContainedThingsSet.Clear();
				this.uniqueContainedThings.Clear();
				for (int i = 0; i < this.regions.Count; i++)
				{
					List<Thing> allThings = this.regions[i].ListerThings.AllThings;
					if (allThings != null)
					{
						for (int j = 0; j < allThings.Count; j++)
						{
							Thing item = allThings[j];
							if (this.uniqueContainedThingsSet.Add(item))
							{
								this.uniqueContainedThings.Add(item);
							}
						}
					}
				}
				this.uniqueContainedThingsSet.Clear();
				return this.uniqueContainedThings;
			}
		}

		
		
		public RoomRoleDef Role
		{
			get
			{
				if (this.statsAndRoleDirty)
				{
					this.UpdateRoomStatsAndRole();
				}
				return this.role;
			}
		}

		
		public static Room MakeNew(Map map)
		{
			Room room = new Room();
			room.mapIndex = (sbyte)map.Index;
			room.ID = Room.nextRoomID;
			Room.nextRoomID++;
			return room;
		}

		
		public void AddRegion(Region r)
		{
			if (this.regions.Contains(r))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to add the same region twice to Room. region=",
					r,
					", room=",
					this
				}), false);
				return;
			}
			this.regions.Add(r);
			if (r.touchesMapEdge)
			{
				this.numRegionsTouchingMapEdge++;
			}
			if (this.regions.Count == 1)
			{
				this.Map.regionGrid.allRooms.Add(this);
			}
		}

		
		public void RemoveRegion(Region r)
		{
			if (!this.regions.Contains(r))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to remove region from Room but this region is not here. region=",
					r,
					", room=",
					this
				}), false);
				return;
			}
			this.regions.Remove(r);
			if (r.touchesMapEdge)
			{
				this.numRegionsTouchingMapEdge--;
			}
			if (this.regions.Count == 0)
			{
				this.Group = null;
				this.cachedOpenRoofCount = -1;
				this.cachedOpenRoofState = null;
				this.statsAndRoleDirty = true;
				this.Map.regionGrid.allRooms.Remove(this);
			}
		}

		
		public void Notify_MyMapRemoved()
		{
			this.mapIndex = -1;
		}

		
		public void Notify_ContainedThingSpawnedOrDespawned(Thing th)
		{
			if (th.def.category != ThingCategory.Mote && th.def.category != ThingCategory.Projectile && th.def.category != ThingCategory.Ethereal && th.def.category != ThingCategory.Pawn)
			{
				if (this.IsDoorway)
				{
					for (int i = 0; i < this.regions[0].links.Count; i++)
					{
						Region otherRegion = this.regions[0].links[i].GetOtherRegion(this.regions[0]);
						if (otherRegion != null && !otherRegion.IsDoorway)
						{
							otherRegion.Room.Notify_ContainedThingSpawnedOrDespawned(th);
						}
					}
				}
				this.statsAndRoleDirty = true;
			}
		}

		
		public void Notify_TerrainChanged()
		{
			this.statsAndRoleDirty = true;
		}

		
		public void Notify_BedTypeChanged()
		{
			this.statsAndRoleDirty = true;
		}

		
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.cachedOpenRoofState = null;
			this.Group.Notify_RoofChanged();
		}

		
		public void Notify_RoomShapeOrContainedBedsChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			this.cachedOpenRoofState = null;
			if (Current.ProgramState == ProgramState.Playing && !this.Fogged)
			{
				this.Map.autoBuildRoofAreaSetter.TryGenerateAreaFor(this);
			}
			this.isPrisonCell = false;
			if (Building_Bed.RoomCanBePrisonCell(this))
			{
				List<Thing> containedAndAdjacentThings = this.ContainedAndAdjacentThings;
				for (int i = 0; i < containedAndAdjacentThings.Count; i++)
				{
					Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
					if (building_Bed != null && building_Bed.ForPrisoners)
					{
						this.isPrisonCell = true;
						break;
					}
				}
			}
			List<Thing> list = this.Map.listerThings.ThingsOfDef(ThingDefOf.NutrientPasteDispenser);
			for (int j = 0; j < list.Count; j++)
			{
				list[j].Notify_ColorChanged();
			}
			if (Current.ProgramState == ProgramState.Playing && this.isPrisonCell)
			{
				foreach (Building_Bed building_Bed2 in this.ContainedBeds)
				{
					building_Bed2.ForPrisoners = true;
				}
			}
			this.lastChangeTick = Find.TickManager.TicksGame;
			this.statsAndRoleDirty = true;
			FacilitiesUtility.NotifyFacilitiesAboutChangedLOSBlockers(this.regions);
		}

		
		public bool ContainsCell(IntVec3 cell)
		{
			return this.Map != null && cell.GetRoom(this.Map, RegionType.Set_All) == this;
		}

		
		public bool ContainsThing(ThingDef def)
		{
			for (int i = 0; i < this.regions.Count; i++)
			{
				if (this.regions[i].ListerThings.ThingsOfDef(def).Any<Thing>())
				{
					return true;
				}
			}
			return false;
		}

		
		public IEnumerable<Thing> ContainedThings(ThingDef def)
		{
			this.uniqueContainedThingsOfDef.Clear();
			int num;
			for (int i = 0; i < this.regions.Count; i = num)
			{
				List<Thing> things = this.regions[i].ListerThings.ThingsOfDef(def);
				for (int j = 0; j < things.Count; j = num)
				{
					if (this.uniqueContainedThingsOfDef.Add(things[j]))
					{
						yield return things[j];
					}
					num = j + 1;
				}
				things = null;
				num = i + 1;
			}
			this.uniqueContainedThingsOfDef.Clear();
			yield break;
		}

		
		public int ThingCount(ThingDef def)
		{
			this.uniqueContainedThingsOfDef.Clear();
			int num = 0;
			for (int i = 0; i < this.regions.Count; i++)
			{
				List<Thing> list = this.regions[i].ListerThings.ThingsOfDef(def);
				for (int j = 0; j < list.Count; j++)
				{
					if (this.uniqueContainedThingsOfDef.Add(list[j]))
					{
						num += list[j].stackCount;
					}
				}
			}
			this.uniqueContainedThingsOfDef.Clear();
			return num;
		}

		
		public void DecrementMapIndex()
		{
			if (this.mapIndex <= 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to decrement map index for room ",
					this.ID,
					", but mapIndex=",
					this.mapIndex
				}), false);
				return;
			}
			this.mapIndex -= 1;
		}

		
		public float GetStat(RoomStatDef roomStat)
		{
			if (this.statsAndRoleDirty)
			{
				this.UpdateRoomStatsAndRole();
			}
			if (this.stats == null)
			{
				return roomStat.roomlessScore;
			}
			return this.stats[roomStat];
		}

		
		public RoomStatScoreStage GetStatScoreStage(RoomStatDef stat)
		{
			return stat.GetScoreStage(this.GetStat(stat));
		}

		
		public void DrawFieldEdges()
		{
			Room.fields.Clear();
			Room.fields.AddRange(this.Cells);
			Color color = this.isPrisonCell ? Room.PrisonFieldColor : Room.NonPrisonFieldColor;
			color.a = Pulser.PulseBrightness(1f, 0.6f);
			GenDraw.DrawFieldEdges(Room.fields, color);
			Room.fields.Clear();
		}

		
		public int OpenRoofCountStopAt(int threshold)
		{
			if (this.cachedOpenRoofCount == -1 && this.cachedOpenRoofState == null)
			{
				this.cachedOpenRoofCount = 0;
				this.cachedOpenRoofState = this.Cells.GetEnumerator();
			}
			if (this.cachedOpenRoofCount < threshold && this.cachedOpenRoofState != null)
			{
				RoofGrid roofGrid = this.Map.roofGrid;
				while (this.cachedOpenRoofCount < threshold && this.cachedOpenRoofState.MoveNext())
				{
					if (!roofGrid.Roofed(this.cachedOpenRoofState.Current))
					{
						this.cachedOpenRoofCount++;
					}
				}
				if (this.cachedOpenRoofCount < threshold)
				{
					this.cachedOpenRoofState = null;
				}
			}
			return this.cachedOpenRoofCount;
		}

		
		private void UpdateRoomStatsAndRole()
		{
			this.statsAndRoleDirty = false;
			if (!this.TouchesMapEdge && this.RegionType == RegionType.Normal && this.regions.Count <= 36)
			{
				if (this.stats == null)
				{
					this.stats = new DefMap<RoomStatDef, float>();
				}
				foreach (RoomStatDef roomStatDef in from x in DefDatabase<RoomStatDef>.AllDefs
				orderby x.updatePriority descending
				select x)
				{
					this.stats[roomStatDef] = roomStatDef.Worker.GetScore(this);
				}
				this.role = DefDatabase<RoomRoleDef>.AllDefs.MaxBy((RoomRoleDef x) => x.Worker.GetScore(this));
				return;
			}
			this.stats = null;
			this.role = RoomRoleDefOf.None;
		}

		
		internal void DebugDraw()
		{
			int hashCode = this.GetHashCode();
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)hashCode * 0.01f);
			}
		}

		
		internal string DebugString()
		{
			return string.Concat(new object[]
			{
				"Room ID=",
				this.ID,
				"\n  first cell=",
				this.Cells.FirstOrDefault<IntVec3>(),
				"\n  RegionCount=",
				this.RegionCount,
				"\n  RegionType=",
				this.RegionType,
				"\n  CellCount=",
				this.CellCount,
				"\n  OpenRoofCount=",
				this.OpenRoofCount,
				"\n  numRegionsTouchingMapEdge=",
				this.numRegionsTouchingMapEdge,
				"\n  lastChangeTick=",
				this.lastChangeTick,
				"\n  isPrisonCell=",
				this.isPrisonCell.ToString(),
				"\n  RoomGroup=",
				(this.Group != null) ? this.Group.ID.ToString() : "null"
			});
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Room(roomID=",
				this.ID,
				", first=",
				this.Cells.FirstOrDefault<IntVec3>().ToString(),
				", RegionsCount=",
				this.RegionCount.ToString(),
				", lastChangeTick=",
				this.lastChangeTick,
				")"
			});
		}

		
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.ID, 1538478890);
		}

		
		public sbyte mapIndex = -1;

		
		private RoomGroup groupInt;

		
		private List<Region> regions = new List<Region>();

		
		public int ID = -16161616;

		
		public int lastChangeTick = -1;

		
		private int numRegionsTouchingMapEdge;

		
		private int cachedOpenRoofCount = -1;

		
		private IEnumerator<IntVec3> cachedOpenRoofState;

		
		public bool isPrisonCell;

		
		private int cachedCellCount = -1;

		
		private bool statsAndRoleDirty = true;

		
		private DefMap<RoomStatDef, float> stats = new DefMap<RoomStatDef, float>();

		
		private RoomRoleDef role;

		
		public int newOrReusedRoomGroupIndex = -1;

		
		private static int nextRoomID;

		
		private const int RegionCountHuge = 60;

		
		private const int MaxRegionsToAssignRoomRole = 36;

		
		private static readonly Color PrisonFieldColor = new Color(1f, 0.7f, 0.2f);

		
		private static readonly Color NonPrisonFieldColor = new Color(0.3f, 0.3f, 1f);

		
		private HashSet<Room> uniqueNeighborsSet = new HashSet<Room>();

		
		private List<Room> uniqueNeighbors = new List<Room>();

		
		private HashSet<Thing> uniqueContainedThingsSet = new HashSet<Thing>();

		
		private List<Thing> uniqueContainedThings = new List<Thing>();

		
		private HashSet<Thing> uniqueContainedThingsOfDef = new HashSet<Thing>();

		
		private static List<IntVec3> fields = new List<IntVec3>();
	}
}
