using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	
	public sealed class ExitMapGrid : ICellBoolGiver
	{
		
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x00037674 File Offset: 0x00035874
		public bool MapUsesExitGrid
		{
			get
			{
				if (this.map.IsPlayerHome)
				{
					return false;
				}
				CaravansBattlefield caravansBattlefield = this.map.Parent as CaravansBattlefield;
				if (caravansBattlefield != null && caravansBattlefield.def.blockExitGridUntilBattleIsWon && !caravansBattlefield.WonBattle)
				{
					return false;
				}
				FormCaravanComp component = this.map.Parent.GetComponent<FormCaravanComp>();
				return component == null || !component.CanFormOrReformCaravanNow;
			}
		}

		
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x000376DC File Offset: 0x000358DC
		public CellBoolDrawer Drawer
		{
			get
			{
				if (!this.MapUsesExitGrid)
				{
					return null;
				}
				if (this.dirty)
				{
					this.Rebuild();
				}
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 3690, 0.33f);
				}
				return this.drawerInt;
			}
		}

		
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x00037745 File Offset: 0x00035945
		public BoolGrid Grid
		{
			get
			{
				if (!this.MapUsesExitGrid)
				{
					return null;
				}
				if (this.dirty)
				{
					this.Rebuild();
				}
				return this.exitMapGrid;
			}
		}

		
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x00037765 File Offset: 0x00035965
		public Color Color
		{
			get
			{
				return new Color(0.35f, 1f, 0.35f, 0.12f);
			}
		}

		
		public ExitMapGrid(Map map)
		{
			this.map = map;
		}

		
		public bool GetCellBool(int index)
		{
			return this.Grid[index] && !this.map.fogGrid.IsFogged(index);
		}

		
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		
		public bool IsExitCell(IntVec3 c)
		{
			return this.MapUsesExitGrid && this.Grid[c];
		}

		
		public void ExitMapGridUpdate()
		{
			if (!this.MapUsesExitGrid)
			{
				return;
			}
			this.Drawer.MarkForDraw();
			this.Drawer.CellBoolDrawerUpdate();
		}

		
		public void Notify_LOSBlockerSpawned()
		{
			this.dirty = true;
		}

		
		public void Notify_LOSBlockerDespawned()
		{
			this.dirty = true;
		}

		
		private void Rebuild()
		{
			this.dirty = false;
			if (this.exitMapGrid == null)
			{
				this.exitMapGrid = new BoolGrid(this.map);
			}
			else
			{
				this.exitMapGrid.Clear();
			}
			CellRect cellRect = CellRect.WholeMap(this.map);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					if (i > 1 && i < cellRect.maxZ - 2 + 1 && j > 1 && j < cellRect.maxX - 2 + 1)
					{
						j = cellRect.maxX - 2 + 1;
					}
					IntVec3 intVec = new IntVec3(j, 0, i);
					if (this.IsGoodExitCell(intVec))
					{
						this.exitMapGrid[intVec] = true;
					}
				}
			}
			if (this.drawerInt != null)
			{
				this.drawerInt.SetDirty();
			}
		}

		
		private bool IsGoodExitCell(IntVec3 cell)
		{
			if (!cell.CanBeSeenOver(this.map))
			{
				return false;
			}
			int num = GenRadial.NumCellsInRadius(2f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = cell + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.map) && intVec.OnEdge(this.map) && intVec.CanBeSeenOverFast(this.map) && GenSight.LineOfSight(cell, intVec, this.map, false, null, 0, 0))
				{
					return true;
				}
			}
			return false;
		}

		
		private Map map;

		
		private bool dirty = true;

		
		private BoolGrid exitMapGrid;

		
		private CellBoolDrawer drawerInt;

		
		private const int MaxDistToEdge = 2;
	}
}
