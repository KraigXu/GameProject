using System;
using UnityEngine.SceneManagement;

namespace Verse
{
	// Token: 0x0200011F RID: 287
	public static class QuickStarter
	{
		// Token: 0x06000816 RID: 2070 RVA: 0x00025648 File Offset: 0x00023848
		public static bool CheckQuickStart()
		{
			if (GenCommandLine.CommandLineArgPassed("quicktest") && !QuickStarter.quickStarted && GenScene.InEntryScene)
			{
				QuickStarter.quickStarted = true;
				SceneManager.LoadScene("Play");
				return true;
			}
			return false;
		}

		// Token: 0x0400072C RID: 1836
		private static bool quickStarted;
	}
}
