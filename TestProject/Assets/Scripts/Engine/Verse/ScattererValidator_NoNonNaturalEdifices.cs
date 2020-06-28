using System;

namespace Verse
{
	// Token: 0x020001A9 RID: 425
	public class ScattererValidator_NoNonNaturalEdifices : ScattererValidator
	{
		// Token: 0x06000BE6 RID: 3046 RVA: 0x00043980 File Offset: 0x00041B80
		public override bool Allows(IntVec3 c, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(c, this.radius);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					if (new IntVec3(j, 0, i).GetEdifice(map) != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04000982 RID: 2434
		public int radius = 1;
	}
}
