using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000461 RID: 1121
	public static class MatsFromSpectrum
	{
		// Token: 0x06002144 RID: 8516 RVA: 0x000CC04F File Offset: 0x000CA24F
		public static Material Get(Color[] spectrum, float val)
		{
			return MatsFromSpectrum.Get(spectrum, val, ShaderDatabase.MetaOverlay);
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x000CC05D File Offset: 0x000CA25D
		public static Material Get(Color[] spectrum, float val, Shader shader)
		{
			return SolidColorMaterials.NewSolidColorMaterial(ColorsFromSpectrum.Get(spectrum, val), shader);
		}
	}
}
