using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000573 RID: 1395
	public sealed class PathGrid
	{
		// Token: 0x06002767 RID: 10087 RVA: 0x000E6C62 File Offset: 0x000E4E62
		public PathGrid(Map map)
		{
			this.map = map;
			this.ResetPathGrid();
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000E6C77 File Offset: 0x000E4E77
		public void ResetPathGrid()
		{
			this.pathGrid = new int[this.map.cellIndices.NumGridCells];
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000E6C94 File Offset: 0x000E4E94
		public bool Walkable(IntVec3 loc)
		{
			return loc.InBounds(this.map) && this.pathGrid[this.map.cellIndices.CellToIndex(loc)] < 10000;
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000E6CC5 File Offset: 0x000E4EC5
		public bool WalkableFast(IntVec3 loc)
		{
			return this.pathGrid[this.map.cellIndices.CellToIndex(loc)] < 10000;
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x000E6CE6 File Offset: 0x000E4EE6
		public bool WalkableFast(int x, int z)
		{
			return this.pathGrid[this.map.cellIndices.CellToIndex(x, z)] < 10000;
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000E6D08 File Offset: 0x000E4F08
		public bool WalkableFast(int index)
		{
			return this.pathGrid[index] < 10000;
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000E6D19 File Offset: 0x000E4F19
		public int PerceivedPathCostAt(IntVec3 loc)
		{
			return this.pathGrid[this.map.cellIndices.CellToIndex(loc)];
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000E6D34 File Offset: 0x000E4F34
		public void RecalculatePerceivedPathCostUnderThing(Thing t)
		{
			if (t.def.size == IntVec2.One)
			{
				this.RecalculatePerceivedPathCostAt(t.Position);
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.RecalculatePerceivedPathCostAt(c);
				}
			}
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000E6DAC File Offset: 0x000E4FAC
		public void RecalculatePerceivedPathCostAt(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				return;
			}
			bool flag = this.WalkableFast(c);
			this.pathGrid[this.map.cellIndices.CellToIndex(c)] = this.CalculatedCostAt(c, true, IntVec3.Invalid);
			if (this.WalkableFast(c) != flag)
			{
				this.map.reachability.ClearCache();
				this.map.regionDirtyer.Notify_WalkabilityChanged(c);
			}
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x000E6E20 File Offset: 0x000E5020
		public void RecalculateAllPerceivedPathCosts()
		{
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.RecalculatePerceivedPathCostAt(c);
			}
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x000E6E74 File Offset: 0x000E5074
		public int CalculatedCostAt(IntVec3 c, bool perceivedStatic, IntVec3 prevCell)
		{
			bool flag = false;
			TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(c);
			if (terrainDef == null || terrainDef.passability == Traversability.Impassable)
			{
				return 10000;
			}
			int num = terrainDef.pathCost;
			List<Thing> list = this.map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.passability == Traversability.Impassable)
				{
					return 10000;
				}
				if (!PathGrid.IsPathCostIgnoreRepeater(thing.def) || !prevCell.IsValid || !this.ContainsPathCostIgnoreRepeater(prevCell))
				{
					int pathCost = thing.def.pathCost;
					if (pathCost > num)
					{
						num = pathCost;
					}
				}
				if (thing is Building_Door && prevCell.IsValid)
				{
					Building edifice = prevCell.GetEdifice(this.map);
					if (edifice != null && edifice is Building_Door)
					{
						flag = true;
					}
				}
			}
			int num2 = SnowUtility.MovementTicksAddOn(this.map.snowGrid.GetCategory(c));
			if (num2 > num)
			{
				num = num2;
			}
			if (flag)
			{
				num += 45;
			}
			if (perceivedStatic)
			{
				for (int j = 0; j < 9; j++)
				{
					IntVec3 intVec = GenAdj.AdjacentCellsAndInside[j];
					IntVec3 c2 = c + intVec;
					if (c2.InBounds(this.map))
					{
						Fire fire = null;
						list = this.map.thingGrid.ThingsListAtFast(c2);
						for (int k = 0; k < list.Count; k++)
						{
							fire = (list[k] as Fire);
							if (fire != null)
							{
								break;
							}
						}
						if (fire != null && fire.parent == null)
						{
							if (intVec.x == 0 && intVec.z == 0)
							{
								num += 1000;
							}
							else
							{
								num += 150;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x000E7040 File Offset: 0x000E5240
		private bool ContainsPathCostIgnoreRepeater(IntVec3 c)
		{
			List<Thing> list = this.map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (PathGrid.IsPathCostIgnoreRepeater(list[i].def))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x000E7086 File Offset: 0x000E5286
		private static bool IsPathCostIgnoreRepeater(ThingDef def)
		{
			return def.pathCost >= 25 && def.pathCostIgnoreRepeat;
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x000E709C File Offset: 0x000E529C
		[DebugOutput]
		public static void ThingPathCostsIgnoreRepeaters()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("===============PATH COST IGNORE REPEATERS==============");
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (PathGrid.IsPathCostIgnoreRepeater(thingDef) && thingDef.passability != Traversability.Impassable)
				{
					stringBuilder.AppendLine(thingDef.defName + " " + thingDef.pathCost);
				}
			}
			stringBuilder.AppendLine("===============NON-PATH COST IGNORE REPEATERS that are buildings with >0 pathCost ==============");
			foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
			{
				if (!PathGrid.IsPathCostIgnoreRepeater(thingDef2) && thingDef2.passability != Traversability.Impassable && thingDef2.category == ThingCategory.Building && thingDef2.pathCost > 0)
				{
					stringBuilder.AppendLine(thingDef2.defName + " " + thingDef2.pathCost);
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04001782 RID: 6018
		private Map map;

		// Token: 0x04001783 RID: 6019
		public int[] pathGrid;

		// Token: 0x04001784 RID: 6020
		public const int ImpassableCost = 10000;
	}
}
