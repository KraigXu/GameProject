using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000172 RID: 370
	public sealed class SnowGrid : IExposable
	{
		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000A83 RID: 2691 RVA: 0x000380C3 File Offset: 0x000362C3
		internal float[] DepthGridDirect_Unsafe
		{
			get
			{
				return this.depthGrid;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x000380CB File Offset: 0x000362CB
		public float TotalDepth
		{
			get
			{
				return (float)this.totalDepth;
			}
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x000380D4 File Offset: 0x000362D4
		public SnowGrid(Map map)
		{
			this.map = map;
			this.depthGrid = new float[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x000380F9 File Offset: 0x000362F9
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => SnowGrid.SnowFloatToShort(this.GetDepth(c)), delegate(IntVec3 c, ushort val)
			{
				this.depthGrid[this.map.cellIndices.CellToIndex(c)] = SnowGrid.SnowShortToFloat(val);
			}, "depthGrid");
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00038123 File Offset: 0x00036323
		private static ushort SnowFloatToShort(float depth)
		{
			depth = Mathf.Clamp(depth, 0f, 1f);
			depth *= 65535f;
			return (ushort)Mathf.RoundToInt(depth);
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00038147 File Offset: 0x00036347
		private static float SnowShortToFloat(ushort depth)
		{
			return (float)depth / 65535f;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00038154 File Offset: 0x00036354
		private bool CanHaveSnow(int ind)
		{
			Building building = this.map.edificeGrid[ind];
			if (building != null && !SnowGrid.CanCoexistWithSnow(building.def))
			{
				return false;
			}
			TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(ind);
			return terrainDef == null || terrainDef.holdSnow;
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x000381A5 File Offset: 0x000363A5
		public static bool CanCoexistWithSnow(ThingDef def)
		{
			return def.category != ThingCategory.Building || def.Fillage != FillCategory.Full;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x000381BC File Offset: 0x000363BC
		public void AddDepth(IntVec3 c, float depthToAdd)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			float num2 = this.depthGrid[num];
			if (num2 <= 0f && depthToAdd < 0f)
			{
				return;
			}
			if (num2 >= 0.999f && depthToAdd > 1f)
			{
				return;
			}
			if (!this.CanHaveSnow(num))
			{
				this.depthGrid[num] = 0f;
				return;
			}
			float num3 = num2 + depthToAdd;
			num3 = Mathf.Clamp(num3, 0f, 1f);
			float num4 = num3 - num2;
			this.totalDepth += (double)num4;
			if (Mathf.Abs(num4) > 0.0001f)
			{
				this.depthGrid[num] = num3;
				this.CheckVisualOrPathCostChange(c, num2, num3);
			}
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00038264 File Offset: 0x00036464
		public void SetDepth(IntVec3 c, float newDepth)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			if (!this.CanHaveSnow(num))
			{
				this.depthGrid[num] = 0f;
				return;
			}
			newDepth = Mathf.Clamp(newDepth, 0f, 1f);
			float num2 = this.depthGrid[num];
			this.depthGrid[num] = newDepth;
			float num3 = newDepth - num2;
			this.totalDepth += (double)num3;
			this.CheckVisualOrPathCostChange(c, num2, newDepth);
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x000382DC File Offset: 0x000364DC
		private void CheckVisualOrPathCostChange(IntVec3 c, float oldDepth, float newDepth)
		{
			if (!Mathf.Approximately(oldDepth, newDepth))
			{
				if (Mathf.Abs(oldDepth - newDepth) > 0.15f || Rand.Value < 0.0125f)
				{
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Snow, true, false);
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Things, true, false);
				}
				else if (newDepth == 0f)
				{
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Snow, true, false);
				}
				if (SnowUtility.GetSnowCategory(oldDepth) != SnowUtility.GetSnowCategory(newDepth))
				{
					this.map.pathGrid.RecalculatePerceivedPathCostAt(c);
				}
			}
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x00038377 File Offset: 0x00036577
		public float GetDepth(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				return 0f;
			}
			return this.depthGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x000383A5 File Offset: 0x000365A5
		public SnowCategory GetCategory(IntVec3 c)
		{
			return SnowUtility.GetSnowCategory(this.GetDepth(c));
		}

		// Token: 0x04000846 RID: 2118
		private Map map;

		// Token: 0x04000847 RID: 2119
		private float[] depthGrid;

		// Token: 0x04000848 RID: 2120
		private double totalDepth;

		// Token: 0x04000849 RID: 2121
		public const float MaxDepth = 1f;
	}
}
