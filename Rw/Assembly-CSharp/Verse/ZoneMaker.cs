using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001EB RID: 491
	public static class ZoneMaker
	{
		// Token: 0x06000DE1 RID: 3553 RVA: 0x0004F290 File Offset: 0x0004D490
		public static Zone MakeZoneWithCells(Zone z, IEnumerable<IntVec3> cells)
		{
			if (cells != null)
			{
				foreach (IntVec3 c in cells)
				{
					z.AddCell(c);
				}
			}
			return z;
		}
	}
}
