using System;

namespace Verse
{
	
	public static class DebugTools
	{
		
		public static void DebugToolsOnGUI()
		{
			if (DebugTools.curTool != null)
			{
				DebugTools.curTool.DebugToolOnGUI();
			}
		}

		
		public static DebugTool curTool;
	}
}
