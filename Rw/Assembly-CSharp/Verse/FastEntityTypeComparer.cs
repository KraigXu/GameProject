using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000176 RID: 374
	public class FastEntityTypeComparer : IEqualityComparer<ThingCategory>
	{
		// Token: 0x06000AC1 RID: 2753 RVA: 0x0001BED2 File Offset: 0x0001A0D2
		public bool Equals(ThingCategory x, ThingCategory y)
		{
			return x == y;
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0002D90A File Offset: 0x0002BB0A
		public int GetHashCode(ThingCategory obj)
		{
			return (int)obj;
		}

		// Token: 0x04000855 RID: 2133
		public static readonly FastEntityTypeComparer Instance = new FastEntityTypeComparer();
	}
}
