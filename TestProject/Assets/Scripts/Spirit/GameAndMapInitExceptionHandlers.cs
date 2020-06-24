using System;
using System.Linq;

namespace Spirit
{
	// Token: 0x020001AD RID: 429
	public static class GameAndMapInitExceptionHandlers
	{
		// Token: 0x06000BFB RID: 3067 RVA: 0x00043FFC File Offset: 0x000421FC
		public static void ErrorWhileLoadingAssets(Exception e)
		{
			string text = "ErrorWhileLoadingAssets".Translate();
			if (!ModsConfig.ActiveModsInLoadOrder.Any((ModMetaData x) => !x.Official))
			{
				if (ModsConfig.ActiveModsInLoadOrder.Any((ModMetaData x) => x.IsCoreMod))
				{
					goto IL_86;
				}
			}
			text += "\n\n" + "ErrorWhileLoadingAssets_ModsInfo".Translate();
		IL_86:
			DelayedErrorWindowRequest.Add(text, "ErrorWhileLoadingAssetsTitle".Translate());
			GenScene.GoToMainMenu();
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x000440A9 File Offset: 0x000422A9
		public static void ErrorWhileGeneratingMap(Exception e)
		{
			DelayedErrorWindowRequest.Add("ErrorWhileGeneratingMap".Translate(), "ErrorWhileGeneratingMapTitle".Translate());
			Scribe.ForceStop();
			GenScene.GoToMainMenu();
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x000440D8 File Offset: 0x000422D8
		public static void ErrorWhileLoadingGame(Exception e)
		{
			string text = "ErrorWhileLoadingMap".Translate();
			string value;
			string value2;
			if (!ScribeMetaHeaderUtility.LoadedModsMatchesActiveMods(out value, out value2))
			{
				text += "\n\n" + "ModsMismatchWarningText".Translate(value, value2);
			}
			DelayedErrorWindowRequest.Add(text, "ErrorWhileLoadingMapTitle".Translate());
			Scribe.ForceStop();
			GenScene.GoToMainMenu();
		}
	}
}
