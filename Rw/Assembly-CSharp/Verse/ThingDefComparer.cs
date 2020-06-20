using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000177 RID: 375
	public class ThingDefComparer : IEqualityComparer<ThingDef>
	{
		// Token: 0x06000AC5 RID: 2757 RVA: 0x0003925B File Offset: 0x0003745B
		public bool Equals(ThingDef x, ThingDef y)
		{
			return (x == null && y == null) || (x != null && y != null && x.shortHash == y.shortHash);
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0001BED8 File Offset: 0x0001A0D8
		public int GetHashCode(ThingDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04000856 RID: 2134
		public static readonly ThingDefComparer Instance = new ThingDefComparer();
	}
}
