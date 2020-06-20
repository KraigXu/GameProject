using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003FC RID: 1020
	public static class ColorExtension
	{
		// Token: 0x06001E4B RID: 7755 RVA: 0x000BC940 File Offset: 0x000BAB40
		public static Color ToOpaque(this Color c)
		{
			c.a = 1f;
			return c;
		}
	}
}
