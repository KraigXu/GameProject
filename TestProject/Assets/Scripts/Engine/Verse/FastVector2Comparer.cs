using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002B2 RID: 690
	public class FastVector2Comparer : IEqualityComparer<Vector2>
	{
		// Token: 0x060013AF RID: 5039 RVA: 0x0007117C File Offset: 0x0006F37C
		public bool Equals(Vector2 x, Vector2 y)
		{
			return x == y;
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x00071185 File Offset: 0x0006F385
		public int GetHashCode(Vector2 obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04000D37 RID: 3383
		public static readonly FastVector2Comparer Instance = new FastVector2Comparer();
	}
}
