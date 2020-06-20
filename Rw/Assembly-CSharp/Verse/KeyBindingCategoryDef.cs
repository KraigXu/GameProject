using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000C0 RID: 192
	public class KeyBindingCategoryDef : Def
	{
		// Token: 0x06000595 RID: 1429 RVA: 0x0001B906 File Offset: 0x00019B06
		public static KeyBindingCategoryDef Named(string defName)
		{
			return DefDatabase<KeyBindingCategoryDef>.GetNamed(defName, true);
		}

		// Token: 0x04000424 RID: 1060
		public bool isGameUniversal;

		// Token: 0x04000425 RID: 1061
		public List<KeyBindingCategoryDef> checkForConflicts = new List<KeyBindingCategoryDef>();

		// Token: 0x04000426 RID: 1062
		public bool selfConflicting = true;
	}
}
