using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000157 RID: 343
	public static class DebugSolidColorMats
	{
		// Token: 0x060009A4 RID: 2468 RVA: 0x00034808 File Offset: 0x00032A08
		public static Material MaterialOf(Color col)
		{
			Material material;
			if (DebugSolidColorMats.colorMatDict.TryGetValue(col, out material))
			{
				return material;
			}
			material = SolidColorMaterials.SimpleSolidColorMaterial(col, false);
			DebugSolidColorMats.colorMatDict.Add(col, material);
			return material;
		}

		// Token: 0x040007ED RID: 2029
		private static Dictionary<Color, Material> colorMatDict = new Dictionary<Color, Material>();
	}
}
