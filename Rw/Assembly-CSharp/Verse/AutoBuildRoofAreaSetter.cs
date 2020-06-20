using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001CC RID: 460
	public class AutoBuildRoofAreaSetter
	{
		// Token: 0x06000D12 RID: 3346 RVA: 0x0004A498 File Offset: 0x00048698
		public AutoBuildRoofAreaSetter(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0004A4D4 File Offset: 0x000486D4
		public void TryGenerateAreaOnImpassable(IntVec3 c)
		{
			if (!c.Roofed(this.map) && c.Impassable(this.map) && RoofCollapseUtility.WithinRangeOfRoofHolder(c, this.map, false))
			{
				bool flag = false;
				for (int i = 0; i < 9; i++)
				{
					Room room = (c + GenRadial.RadialPattern[i]).GetRoom(this.map, RegionType.Set_Passable);
					if (room != null && !room.TouchesMapEdge)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					this.map.areaManager.BuildRoof[c] = true;
					MoteMaker.PlaceTempRoof(c, this.map);
				}
			}
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0004A56E File Offset: 0x0004876E
		public void TryGenerateAreaFor(Room room)
		{
			this.queuedGenerateRooms.Add(room);
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0004A57C File Offset: 0x0004877C
		public void AutoBuildRoofAreaSetterTick_First()
		{
			this.ResolveQueuedGenerateRoofs();
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0004A584 File Offset: 0x00048784
		public void ResolveQueuedGenerateRoofs()
		{
			for (int i = 0; i < this.queuedGenerateRooms.Count; i++)
			{
				this.TryGenerateAreaNow(this.queuedGenerateRooms[i]);
			}
			this.queuedGenerateRooms.Clear();
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0004A5C4 File Offset: 0x000487C4
		private void TryGenerateAreaNow(Room room)
		{
			if (room.Dereferenced || room.TouchesMapEdge)
			{
				return;
			}
			if (room.RegionCount > 26 || room.CellCount > 320)
			{
				return;
			}
			if (room.RegionType == RegionType.Portal)
			{
				return;
			}
			bool flag = false;
			foreach (IntVec3 c in room.BorderCells)
			{
				Thing roofHolderOrImpassable = c.GetRoofHolderOrImpassable(this.map);
				if (roofHolderOrImpassable != null)
				{
					if (roofHolderOrImpassable.Faction != null && roofHolderOrImpassable.Faction != Faction.OfPlayer)
					{
						return;
					}
					if (roofHolderOrImpassable.def.building != null && !roofHolderOrImpassable.def.building.allowAutoroof)
					{
						return;
					}
					if (roofHolderOrImpassable.Faction == Faction.OfPlayer)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			this.innerCells.Clear();
			foreach (IntVec3 intVec in room.Cells)
			{
				if (!this.innerCells.Contains(intVec))
				{
					this.innerCells.Add(intVec);
				}
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c2 = intVec + GenAdj.AdjacentCells[i];
					if (c2.InBounds(this.map))
					{
						Thing roofHolderOrImpassable2 = c2.GetRoofHolderOrImpassable(this.map);
						if (roofHolderOrImpassable2 != null && (roofHolderOrImpassable2.def.size.x > 1 || roofHolderOrImpassable2.def.size.z > 1))
						{
							CellRect cellRect = roofHolderOrImpassable2.OccupiedRect();
							cellRect.ClipInsideMap(this.map);
							for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
							{
								for (int k = cellRect.minX; k <= cellRect.maxX; k++)
								{
									IntVec3 item = new IntVec3(k, 0, j);
									if (!this.innerCells.Contains(item))
									{
										this.innerCells.Add(item);
									}
								}
							}
						}
					}
				}
			}
			this.cellsToRoof.Clear();
			foreach (IntVec3 a in this.innerCells)
			{
				for (int l = 0; l < 9; l++)
				{
					IntVec3 intVec2 = a + GenAdj.AdjacentCellsAndInside[l];
					if (intVec2.InBounds(this.map) && (l == 8 || intVec2.GetRoofHolderOrImpassable(this.map) != null) && !this.cellsToRoof.Contains(intVec2))
					{
						this.cellsToRoof.Add(intVec2);
					}
				}
			}
			this.justRoofedCells.Clear();
			foreach (IntVec3 intVec3 in this.cellsToRoof)
			{
				if (this.map.roofGrid.RoofAt(intVec3) == null && !this.justRoofedCells.Contains(intVec3) && !this.map.areaManager.NoRoof[intVec3] && RoofCollapseUtility.WithinRangeOfRoofHolder(intVec3, this.map, true))
				{
					this.map.areaManager.BuildRoof[intVec3] = true;
					this.justRoofedCells.Add(intVec3);
				}
			}
		}

		// Token: 0x04000A28 RID: 2600
		private Map map;

		// Token: 0x04000A29 RID: 2601
		private List<Room> queuedGenerateRooms = new List<Room>();

		// Token: 0x04000A2A RID: 2602
		private HashSet<IntVec3> cellsToRoof = new HashSet<IntVec3>();

		// Token: 0x04000A2B RID: 2603
		private HashSet<IntVec3> innerCells = new HashSet<IntVec3>();

		// Token: 0x04000A2C RID: 2604
		private List<IntVec3> justRoofedCells = new List<IntVec3>();
	}
}
