using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200016F RID: 367
	public sealed class ExitMapGrid : ICellBoolGiver
	{
		// Token: 0x170001F8 RID: 504
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

		// Token: 0x170001F9 RID: 505
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

		// Token: 0x170001FA RID: 506
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

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x00037765 File Offset: 0x00035965
		public Color Color
		{
			get
			{
				return new Color(0.35f, 1f, 0.35f, 0.12f);
			}
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x00037780 File Offset: 0x00035980
		public ExitMapGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00037796 File Offset: 0x00035996
		public bool GetCellBool(int index)
		{
			return this.Grid[index] && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00017A00 File Offset: 0x00015C00
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x000377BC File Offset: 0x000359BC
		public bool IsExitCell(IntVec3 c)
		{
			return this.MapUsesExitGrid && this.Grid[c];
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x000377D4 File Offset: 0x000359D4
		public void ExitMapGridUpdate()
		{
			if (!this.MapUsesExitGrid)
			{
				return;
			}
			this.Drawer.MarkForDraw();
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x000377F5 File Offset: 0x000359F5
		public void Notify_LOSBlockerSpawned()
		{
			this.dirty = true;
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x000377F5 File Offset: 0x000359F5
		public void Notify_LOSBlockerDespawned()
		{
			this.dirty = true;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00037800 File Offset: 0x00035A00
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

		// Token: 0x06000A53 RID: 2643 RVA: 0x000378D4 File Offset: 0x00035AD4
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

		// Token: 0x0400083E RID: 2110
		private Map map;

		// Token: 0x0400083F RID: 2111
		private bool dirty = true;

		// Token: 0x04000840 RID: 2112
		private BoolGrid exitMapGrid;

		// Token: 0x04000841 RID: 2113
		private CellBoolDrawer drawerInt;

		// Token: 0x04000842 RID: 2114
		private const int MaxDistToEdge = 2;
	}
}
