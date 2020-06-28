using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001D2 RID: 466
	public sealed class RoofGrid : IExposable, ICellBoolGiver
	{
		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000D32 RID: 3378 RVA: 0x0004B69C File Offset: 0x0004989C
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 3620, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000D33 RID: 3379 RVA: 0x0004B6ED File Offset: 0x000498ED
		public Color Color
		{
			get
			{
				return new Color(0.3f, 1f, 0.4f);
			}
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0004B703 File Offset: 0x00049903
		public RoofGrid(Map map)
		{
			this.map = map;
			this.roofGrid = new RoofDef[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0004B728 File Offset: 0x00049928
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, delegate(IntVec3 c)
			{
				if (this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null)
				{
					return this.roofGrid[this.map.cellIndices.CellToIndex(c)].shortHash;
				}
				return 0;
			}, delegate(IntVec3 c, ushort val)
			{
				this.SetRoof(c, DefDatabase<RoofDef>.GetByShortHash(val));
			}, "roofs");
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0004B752 File Offset: 0x00049952
		public bool GetCellBool(int index)
		{
			return this.roofGrid[index] != null && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0004B774 File Offset: 0x00049974
		public Color GetCellExtraColor(int index)
		{
			if (RoofDefOf.RoofRockThick != null && this.roofGrid[index] == RoofDefOf.RoofRockThick)
			{
				return Color.gray;
			}
			return Color.white;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0004B797 File Offset: 0x00049997
		public bool Roofed(int index)
		{
			return this.roofGrid[index] != null;
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x0004B7A4 File Offset: 0x000499A4
		public bool Roofed(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)] != null;
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0004B7C2 File Offset: 0x000499C2
		public bool Roofed(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null;
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0004B7DF File Offset: 0x000499DF
		public RoofDef RoofAt(int index)
		{
			return this.roofGrid[index];
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x0004B7E9 File Offset: 0x000499E9
		public RoofDef RoofAt(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0004B803 File Offset: 0x00049A03
		public RoofDef RoofAt(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)];
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0004B820 File Offset: 0x00049A20
		public void SetRoof(IntVec3 c, RoofDef def)
		{
			if (this.roofGrid[this.map.cellIndices.CellToIndex(c)] == def)
			{
				return;
			}
			this.roofGrid[this.map.cellIndices.CellToIndex(c)] = def;
			this.map.glowGrid.MarkGlowGridDirty(c);
			Region validRegionAt_NoRebuild = this.map.regionGrid.GetValidRegionAt_NoRebuild(c);
			if (validRegionAt_NoRebuild != null)
			{
				validRegionAt_NoRebuild.Room.Notify_RoofChanged();
			}
			if (this.drawerInt != null)
			{
				this.drawerInt.SetDirty();
			}
			this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Roofs);
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0004B8B9 File Offset: 0x00049AB9
		public void RoofGridUpdate()
		{
			if (Find.PlaySettings.showRoofOverlay)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x04000A36 RID: 2614
		private Map map;

		// Token: 0x04000A37 RID: 2615
		private RoofDef[] roofGrid;

		// Token: 0x04000A38 RID: 2616
		private CellBoolDrawer drawerInt;
	}
}
