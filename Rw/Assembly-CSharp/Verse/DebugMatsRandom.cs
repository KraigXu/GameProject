using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000159 RID: 345
	[StaticConstructorOnStartup]
	public static class DebugMatsRandom
	{
		// Token: 0x060009A9 RID: 2473 RVA: 0x000349D8 File Offset: 0x00032BD8
		static DebugMatsRandom()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsRandom.mats[i] = SolidColorMaterials.SimpleSolidColorMaterial(new Color(Rand.Value, Rand.Value, Rand.Value, 0.25f), false);
			}
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x00034A24 File Offset: 0x00032C24
		public static Material Mat(int ind)
		{
			ind %= 100;
			if (ind < 0)
			{
				ind *= -1;
			}
			return DebugMatsRandom.mats[ind];
		}

		// Token: 0x040007F2 RID: 2034
		private static readonly Material[] mats = new Material[100];

		// Token: 0x040007F3 RID: 2035
		public const int MaterialCount = 100;

		// Token: 0x040007F4 RID: 2036
		private const float Opacity = 0.25f;
	}
}
