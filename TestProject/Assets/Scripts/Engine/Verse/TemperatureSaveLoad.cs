using System;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001DC RID: 476
	public class TemperatureSaveLoad
	{
		// Token: 0x06000D7D RID: 3453 RVA: 0x0004D01C File Offset: 0x0004B21C
		public TemperatureSaveLoad(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0004D02C File Offset: 0x0004B22C
		public void DoExposeWork()
		{
			byte[] arr = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				int num = Mathf.RoundToInt(this.map.mapTemperature.OutdoorTemp);
				ushort num2 = this.TempFloatToShort((float)num);
				ushort[] tempGrid = new ushort[this.map.cellIndices.NumGridCells];
				for (int i = 0; i < this.map.cellIndices.NumGridCells; i++)
				{
					tempGrid[i] = num2;
				}
				foreach (Region region in this.map.regionGrid.AllRegions_NoRebuild_InvalidAllowed)
				{
					if (region.Room != null)
					{
						ushort num3 = this.TempFloatToShort(region.Room.Temperature);
						foreach (IntVec3 c2 in region.Cells)
						{
							tempGrid[this.map.cellIndices.CellToIndex(c2)] = num3;
						}
					}
				}
				arr = MapSerializeUtility.SerializeUshort(this.map, (IntVec3 c) => tempGrid[this.map.cellIndices.CellToIndex(c)]);
			}
			DataExposeUtility.ByteArray(ref arr, "temperatures");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.tempGrid = new ushort[this.map.cellIndices.NumGridCells];
				MapSerializeUtility.LoadUshort(arr, this.map, delegate(IntVec3 c, ushort val)
				{
					this.tempGrid[this.map.cellIndices.CellToIndex(c)] = val;
				});
			}
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0004D1D4 File Offset: 0x0004B3D4
		public void ApplyLoadedDataToRegions()
		{
			if (this.tempGrid != null)
			{
				CellIndices cellIndices = this.map.cellIndices;
				foreach (Region region in this.map.regionGrid.AllRegions_NoRebuild_InvalidAllowed)
				{
					if (region.Room != null)
					{
						region.Room.Group.Temperature = this.TempShortToFloat(this.tempGrid[cellIndices.CellToIndex(region.Cells.First<IntVec3>())]);
					}
				}
				this.tempGrid = null;
			}
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0004D278 File Offset: 0x0004B478
		private ushort TempFloatToShort(float temp)
		{
			temp = Mathf.Clamp(temp, -273.15f, 1000f);
			temp *= 16f;
			return (ushort)((int)temp + 32768);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0004D29E File Offset: 0x0004B49E
		private float TempShortToFloat(ushort temp)
		{
			return ((float)temp - 32768f) / 16f;
		}

		// Token: 0x04000A4F RID: 2639
		private Map map;

		// Token: 0x04000A50 RID: 2640
		private ushort[] tempGrid;
	}
}
