using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001C9 RID: 457
	public sealed class Room
	{
		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00048A69 File Offset: 0x00046C69
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

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x00048A86 File Offset: 0x00046C86
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

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x00048AA8 File Offset: 0x00046CA8
		public List<Region> Regions
		{
			get
			{
				return this.regions;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000CBC RID: 3260 RVA: 0x00048AB0 File Offset: 0x00046CB0
		public int RegionCount
		{
			get
			{
				return this.regions.Count;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x00048ABD File Offset: 0x00046CBD
		public bool IsHuge
		{
			get
			{
				return this.regions.Count > 60;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x00048ACE File Offset: 0x00046CCE
		public bool Dereferenced
		{
			get
			{
				return this.regions.Count == 0;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x00048ADE File Offset: 0x00046CDE
		public bool TouchesMapEdge
		{
			get
			{
				return this.numRegionsTouchingMapEdge > 0;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x00048AE9 File Offset: 0x00046CE9
		public float Temperature
		{
			get
			{
				return this.Group.Temperature;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x00048AF6 File Offset: 0x00046CF6
		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.Group.UsesOutdoorTemperature;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x00048B03 File Offset: 0x00046D03
		// (set) Token: 0x06000CC3 RID: 3267 RVA: 0x00048B0B File Offset: 0x00046D0B
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

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x00048B48 File Offset: 0x00046D48
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

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x00048B9F File Offset: 0x00046D9F
		public int OpenRoofCount
		{
			get
			{
				return this.OpenRoofCountStopAt(int.MaxValue);
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x00048BAC File Offset: 0x00046DAC
		public bool PsychologicallyOutdoors
		{
			get
			{
				return this.OpenRoofCountStopAt(300) >= 300 || (this.Group.AnyRoomTouchesMapEdge && (float)this.OpenRoofCount / (float)this.CellCount >= 0.5f);
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x00048BE8 File Offset: 0x00046DE8
		public bool OutdoorsForWork
		{
			get
			{
				return this.OpenRoofCountStopAt(101) > 100 || (float)this.OpenRoofCount > (float)this.CellCount * 0.25f;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x00048C10 File Offset: 0x00046E10
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

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x00048CCC File Offset: 0x00046ECC
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

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000CCA RID: 3274 RVA: 0x00048CDC File Offset: 0x00046EDC
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

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x00048CEC File Offset: 0x00046EEC
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

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000CCC RID: 3276 RVA: 0x00048CFC File Offset: 0x00046EFC
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

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00048D0C File Offset: 0x00046F0C
		public bool Fogged
		{
			get
			{
				return this.regions.Count != 0 && this.regions[0].AnyCell.Fogged(this.Map);
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x00048D39 File Offset: 0x00046F39
		public bool IsDoorway
		{
			get
			{
				return this.regions.Count == 1 && this.regions[0].IsDoorway;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x00048D5C File Offset: 0x00046F5C
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

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x00048DF3 File Offset: 0x00046FF3
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

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00048E09 File Offset: 0x00047009
		public static Room MakeNew(Map map)
		{
			Room room = new Room();
			room.mapIndex = (sbyte)map.Index;
			room.ID = Room.nextRoomID;
			Room.nextRoomID++;
			return room;
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00048E34 File Offset: 0x00047034
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

		// Token: 0x06000CD3 RID: 3283 RVA: 0x00048EC0 File Offset: 0x000470C0
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

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00048F68 File Offset: 0x00047168
		public void Notify_MyMapRemoved()
		{
			this.mapIndex = -1;
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00048F74 File Offset: 0x00047174
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

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00049031 File Offset: 0x00047231
		public void Notify_TerrainChanged()
		{
			this.statsAndRoleDirty = true;
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x00049031 File Offset: 0x00047231
		public void Notify_BedTypeChanged()
		{
			this.statsAndRoleDirty = true;
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0004903A File Offset: 0x0004723A
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.cachedOpenRoofState = null;
			this.Group.Notify_RoofChanged();
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00049058 File Offset: 0x00047258
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

		// Token: 0x06000CDA RID: 3290 RVA: 0x00049190 File Offset: 0x00047390
		public bool ContainsCell(IntVec3 cell)
		{
			return this.Map != null && cell.GetRoom(this.Map, RegionType.Set_All) == this;
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x000491AC File Offset: 0x000473AC
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

		// Token: 0x06000CDC RID: 3292 RVA: 0x000491F0 File Offset: 0x000473F0
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

		// Token: 0x06000CDD RID: 3293 RVA: 0x00049208 File Offset: 0x00047408
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

		// Token: 0x06000CDE RID: 3294 RVA: 0x00049290 File Offset: 0x00047490
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

		// Token: 0x06000CDF RID: 3295 RVA: 0x000492F3 File Offset: 0x000474F3
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

		// Token: 0x06000CE0 RID: 3296 RVA: 0x0004931E File Offset: 0x0004751E
		public RoomStatScoreStage GetStatScoreStage(RoomStatDef stat)
		{
			return stat.GetScoreStage(this.GetStat(stat));
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00049330 File Offset: 0x00047530
		public void DrawFieldEdges()
		{
			Room.fields.Clear();
			Room.fields.AddRange(this.Cells);
			Color color = this.isPrisonCell ? Room.PrisonFieldColor : Room.NonPrisonFieldColor;
			color.a = Pulser.PulseBrightness(1f, 0.6f);
			GenDraw.DrawFieldEdges(Room.fields, color);
			Room.fields.Clear();
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00049398 File Offset: 0x00047598
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

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0004943C File Offset: 0x0004763C
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

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00049530 File Offset: 0x00047730
		internal void DebugDraw()
		{
			int hashCode = this.GetHashCode();
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)hashCode * 0.01f);
			}
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0004958C File Offset: 0x0004778C
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

		// Token: 0x06000CE6 RID: 3302 RVA: 0x000496A8 File Offset: 0x000478A8
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

		// Token: 0x06000CE7 RID: 3303 RVA: 0x00049732 File Offset: 0x00047932
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.ID, 1538478890);
		}

		// Token: 0x040009FD RID: 2557
		public sbyte mapIndex = -1;

		// Token: 0x040009FE RID: 2558
		private RoomGroup groupInt;

		// Token: 0x040009FF RID: 2559
		private List<Region> regions = new List<Region>();

		// Token: 0x04000A00 RID: 2560
		public int ID = -16161616;

		// Token: 0x04000A01 RID: 2561
		public int lastChangeTick = -1;

		// Token: 0x04000A02 RID: 2562
		private int numRegionsTouchingMapEdge;

		// Token: 0x04000A03 RID: 2563
		private int cachedOpenRoofCount = -1;

		// Token: 0x04000A04 RID: 2564
		private IEnumerator<IntVec3> cachedOpenRoofState;

		// Token: 0x04000A05 RID: 2565
		public bool isPrisonCell;

		// Token: 0x04000A06 RID: 2566
		private int cachedCellCount = -1;

		// Token: 0x04000A07 RID: 2567
		private bool statsAndRoleDirty = true;

		// Token: 0x04000A08 RID: 2568
		private DefMap<RoomStatDef, float> stats = new DefMap<RoomStatDef, float>();

		// Token: 0x04000A09 RID: 2569
		private RoomRoleDef role;

		// Token: 0x04000A0A RID: 2570
		public int newOrReusedRoomGroupIndex = -1;

		// Token: 0x04000A0B RID: 2571
		private static int nextRoomID;

		// Token: 0x04000A0C RID: 2572
		private const int RegionCountHuge = 60;

		// Token: 0x04000A0D RID: 2573
		private const int MaxRegionsToAssignRoomRole = 36;

		// Token: 0x04000A0E RID: 2574
		private static readonly Color PrisonFieldColor = new Color(1f, 0.7f, 0.2f);

		// Token: 0x04000A0F RID: 2575
		private static readonly Color NonPrisonFieldColor = new Color(0.3f, 0.3f, 1f);

		// Token: 0x04000A10 RID: 2576
		private HashSet<Room> uniqueNeighborsSet = new HashSet<Room>();

		// Token: 0x04000A11 RID: 2577
		private List<Room> uniqueNeighbors = new List<Room>();

		// Token: 0x04000A12 RID: 2578
		private HashSet<Thing> uniqueContainedThingsSet = new HashSet<Thing>();

		// Token: 0x04000A13 RID: 2579
		private List<Thing> uniqueContainedThings = new List<Thing>();

		// Token: 0x04000A14 RID: 2580
		private HashSet<Thing> uniqueContainedThingsOfDef = new HashSet<Thing>();

		// Token: 0x04000A15 RID: 2581
		private static List<IntVec3> fields = new List<IntVec3>();
	}
}
