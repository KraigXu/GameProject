using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008AF RID: 2223
	public class TerrainThreshold
	{
		// Token: 0x060035B5 RID: 13749 RVA: 0x0012419C File Offset: 0x0012239C
		public static TerrainDef TerrainAtValue(List<TerrainThreshold> threshes, float val)
		{
			for (int i = 0; i < threshes.Count; i++)
			{
				if (threshes[i].min <= val && threshes[i].max >= val)
				{
					return threshes[i].terrain;
				}
			}
			return null;
		}

		// Token: 0x04001D6A RID: 7530
		public TerrainDef terrain;

		// Token: 0x04001D6B RID: 7531
		public float min = -1000f;

		// Token: 0x04001D6C RID: 7532
		public float max = 1000f;
	}
}
