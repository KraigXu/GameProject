using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200016C RID: 364
	public sealed class DeepResourceGrid : ICellBoolGiver, IExposable
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000A32 RID: 2610 RVA: 0x00017A00 File Offset: 0x00015C00
		public Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x00037278 File Offset: 0x00035478
		public DeepResourceGrid(Map map)
		{
			this.map = map;
			this.defGrid = new ushort[map.cellIndices.NumGridCells];
			this.countGrid = new ushort[map.cellIndices.NumGridCells];
			this.drawer = new CellBoolDrawer(this, map.Size.x, map.Size.z, 3640, 1f);
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x000372EC File Offset: 0x000354EC
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => this.defGrid[this.map.cellIndices.CellToIndex(c)], delegate(IntVec3 c, ushort val)
			{
				this.defGrid[this.map.cellIndices.CellToIndex(c)] = val;
			}, "defGrid");
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => this.countGrid[this.map.cellIndices.CellToIndex(c)], delegate(IntVec3 c, ushort val)
			{
				this.countGrid[this.map.cellIndices.CellToIndex(c)] = val;
			}, "countGrid");
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x00037349 File Offset: 0x00035549
		public ThingDef ThingDefAt(IntVec3 c)
		{
			return DefDatabase<ThingDef>.GetByShortHash(this.defGrid[this.map.cellIndices.CellToIndex(c)]);
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x00037368 File Offset: 0x00035568
		public int CountAt(IntVec3 c)
		{
			return (int)this.countGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x00037384 File Offset: 0x00035584
		public void SetAt(IntVec3 c, ThingDef def, int count)
		{
			if (count == 0)
			{
				def = null;
			}
			ushort num;
			if (def == null)
			{
				num = 0;
			}
			else
			{
				num = def.shortHash;
			}
			ushort num2 = (ushort)count;
			if (count > 65535)
			{
				Log.Error("Cannot store count " + count + " in DeepResourceGrid: out of ushort range.", false);
				num2 = ushort.MaxValue;
			}
			if (count < 0)
			{
				Log.Error("Cannot store count " + count + " in DeepResourceGrid: out of ushort range.", false);
				num2 = 0;
			}
			int num3 = this.map.cellIndices.CellToIndex(c);
			if (this.defGrid[num3] == num && this.countGrid[num3] == num2)
			{
				return;
			}
			this.defGrid[num3] = num;
			this.countGrid[num3] = num2;
			this.drawer.SetDirty();
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00037438 File Offset: 0x00035638
		public void DeepResourceGridUpdate()
		{
			this.drawer.CellBoolDrawerUpdate();
			if (DebugViewSettings.drawDeepResources)
			{
				this.MarkForDraw();
			}
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x00037452 File Offset: 0x00035652
		public void MarkForDraw()
		{
			if (this.map == Find.CurrentMap)
			{
				this.drawer.MarkForDraw();
			}
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0003746C File Offset: 0x0003566C
		public bool GetCellBool(int index)
		{
			return this.CountAt(this.map.cellIndices.IndexToCell(index)) > 0;
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00037488 File Offset: 0x00035688
		public Color GetCellExtraColor(int index)
		{
			IntVec3 c = this.map.cellIndices.IndexToCell(index);
			float num = (float)this.CountAt(c);
			ThingDef thingDef = this.ThingDefAt(c);
			return DebugMatsSpectrum.Mat(Mathf.RoundToInt(num / (float)thingDef.deepCountPerCell / 2f * 100f) % 100, true).color;
		}

		// Token: 0x04000838 RID: 2104
		private Map map;

		// Token: 0x04000839 RID: 2105
		private CellBoolDrawer drawer;

		// Token: 0x0400083A RID: 2106
		private ushort[] defGrid;

		// Token: 0x0400083B RID: 2107
		private ushort[] countGrid;
	}
}
