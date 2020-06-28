using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000174 RID: 372
	public sealed class ThingGrid
	{
		// Token: 0x06000AA9 RID: 2729 RVA: 0x00038BF0 File Offset: 0x00036DF0
		public ThingGrid(Map map)
		{
			this.map = map;
			CellIndices cellIndices = map.cellIndices;
			this.thingGrid = new List<Thing>[cellIndices.NumGridCells];
			for (int i = 0; i < cellIndices.NumGridCells; i++)
			{
				this.thingGrid[i] = new List<Thing>(4);
			}
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x00038C44 File Offset: 0x00036E44
		public void Register(Thing t)
		{
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				this.RegisterInCell(t, t.Position);
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					this.RegisterInCell(t, new IntVec3(j, 0, i));
				}
			}
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00038CC8 File Offset: 0x00036EC8
		private void RegisterInCell(Thing t, IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Warning(string.Concat(new object[]
				{
					t,
					" tried to register out of bounds at ",
					c,
					". Destroying."
				}), false);
				t.Destroy(DestroyMode.Vanish);
				return;
			}
			this.thingGrid[this.map.cellIndices.CellToIndex(c)].Add(t);
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x00038D38 File Offset: 0x00036F38
		public void Deregister(Thing t, bool doEvenIfDespawned = false)
		{
			if (!t.Spawned && !doEvenIfDespawned)
			{
				return;
			}
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				this.DeregisterInCell(t, t.Position);
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					this.DeregisterInCell(t, new IntVec3(j, 0, i));
				}
			}
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00038DC8 File Offset: 0x00036FC8
		private void DeregisterInCell(Thing t, IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error(t + " tried to de-register out of bounds at " + c, false);
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (this.thingGrid[num].Contains(t))
			{
				this.thingGrid[num].Remove(t);
			}
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x00038E2C File Offset: 0x0003702C
		public IEnumerable<Thing> ThingsAt(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				yield break;
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			int num;
			for (int i = 0; i < list.Count; i = num + 1)
			{
				yield return list[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x00038E44 File Offset: 0x00037044
		public List<Thing> ThingsListAt(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.ErrorOnce("Got ThingsListAt out of bounds: " + c, 495287, false);
				return ThingGrid.EmptyThingList;
			}
			return this.thingGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00038E98 File Offset: 0x00037098
		public List<Thing> ThingsListAtFast(IntVec3 c)
		{
			return this.thingGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00038EB2 File Offset: 0x000370B2
		public List<Thing> ThingsListAtFast(int index)
		{
			return this.thingGrid[index];
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00038EBC File Offset: 0x000370BC
		public bool CellContains(IntVec3 c, ThingCategory cat)
		{
			return this.ThingAt(c, cat) != null;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00038ECC File Offset: 0x000370CC
		public Thing ThingAt(IntVec3 c, ThingCategory cat)
		{
			if (!c.InBounds(this.map))
			{
				return null;
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == cat)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00038F30 File Offset: 0x00037130
		public bool CellContains(IntVec3 c, ThingDef def)
		{
			return this.ThingAt(c, def) != null;
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x00038F40 File Offset: 0x00037140
		public Thing ThingAt(IntVec3 c, ThingDef def)
		{
			if (!c.InBounds(this.map))
			{
				return null;
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def == def)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00038FA0 File Offset: 0x000371A0
		public T ThingAt<T>(IntVec3 c) where T : Thing
		{
			if (!c.InBounds(this.map))
			{
				return default(T);
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			for (int i = 0; i < list.Count; i++)
			{
				T t = list[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x0400084F RID: 2127
		private Map map;

		// Token: 0x04000850 RID: 2128
		private List<Thing>[] thingGrid;

		// Token: 0x04000851 RID: 2129
		private static readonly List<Thing> EmptyThingList = new List<Thing>();
	}
}
