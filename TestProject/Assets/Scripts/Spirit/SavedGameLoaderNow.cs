using System;
using System.Linq;

namespace Spirit
{
	// Token: 0x020001AE RID: 430
	public class SavedGameLoaderNow
	{
		// Token: 0x06000BFE RID: 3070 RVA: 0x0004414C File Offset: 0x0004234C
		public static void LoadGameFromSaveFileNow(string fileName)
		{
			string str = (from mod in LoadedModManager.RunningMods
						  select mod.PackageIdPlayerFacing).ToLineList("  - ", false);
			Log.Message("Loading game from file " + fileName + " with mods:\n" + str, false);
			DeepProfiler.Start("Loading game from file " + fileName);
			Current.Game = new Game();
			DeepProfiler.Start("InitLoading (read file)");
			Scribe.loader.InitLoading(GenFilePaths.FilePathForSavedGame(fileName));
			DeepProfiler.End();
			try
			{
				ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Map, true);
				if (!Scribe.EnterNode("game"))
				{
					Log.Error("Could not find game XML node.", false);
					Scribe.ForceStop();
					return;
				}
				Current.Game = new Game();
				Current.Game.LoadGame();
			}
			catch (Exception)
			{
				Scribe.ForceStop();
				throw;
			}
			PermadeathModeUtility.CheckUpdatePermadeathModeUniqueNameOnGameLoad(fileName);
			DeepProfiler.End();
		}
	}
}
