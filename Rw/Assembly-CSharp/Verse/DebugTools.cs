using System;

namespace Verse
{
	// Token: 0x0200034D RID: 845
	public static class DebugTools
	{
		// Token: 0x060019E5 RID: 6629 RVA: 0x0009EED7 File Offset: 0x0009D0D7
		public static void DebugToolsOnGUI()
		{
			if (DebugTools.curTool != null)
			{
				DebugTools.curTool.DebugToolOnGUI();
			}
		}

		// Token: 0x04000F0E RID: 3854
		public static DebugTool curTool;
	}
}
