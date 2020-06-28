using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001A4 RID: 420
	public class GenStep_TerrainPatches : GenStep
	{
		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x00042EEB File Offset: 0x000410EB
		public override int SeedPart
		{
			get
			{
				return 1370184742;
			}
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00042EF4 File Offset: 0x000410F4
		public override void Generate(Map map, GenStepParams parms)
		{
			int num = Mathf.RoundToInt((float)map.Area / 10000f * this.patchesPer10kCellsRange.RandomInRange);
			for (int i = 0; i < num; i++)
			{
				float randomInRange = this.patchSizeRange.RandomInRange;
				IntVec3 a = CellFinder.RandomCell(map);
				foreach (IntVec3 b in GenRadial.RadialPatternInRadius(randomInRange / 2f))
				{
					IntVec3 c = a + b;
					if (c.InBounds(map))
					{
						map.terrainGrid.SetTerrain(c, this.terrainDef);
					}
				}
			}
		}

		// Token: 0x04000963 RID: 2403
		public TerrainDef terrainDef;

		// Token: 0x04000964 RID: 2404
		public FloatRange patchesPer10kCellsRange;

		// Token: 0x04000965 RID: 2405
		public FloatRange patchSizeRange;
	}
}
