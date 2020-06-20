using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200000A RID: 10
	public static class ColorIntUtility
	{
		// Token: 0x06000094 RID: 148 RVA: 0x000045B2 File Offset: 0x000027B2
		public static ColorInt AsColorInt(this Color32 col)
		{
			return new ColorInt(col);
		}
	}
}
