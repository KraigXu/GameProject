using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000CB RID: 203
	public class FastPawnCapacityDefComparer : IEqualityComparer<PawnCapacityDef>
	{
		// Token: 0x060005B2 RID: 1458 RVA: 0x0001BED2 File Offset: 0x0001A0D2
		public bool Equals(PawnCapacityDef x, PawnCapacityDef y)
		{
			return x == y;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001BED8 File Offset: 0x0001A0D8
		public int GetHashCode(PawnCapacityDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04000475 RID: 1141
		public static readonly FastPawnCapacityDefComparer Instance = new FastPawnCapacityDefComparer();
	}
}
