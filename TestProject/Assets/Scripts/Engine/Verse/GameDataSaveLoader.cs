using System;
using System.IO;
using RimWorld;
using Verse.Profile;

namespace Verse
{
	// Token: 0x020002C0 RID: 704
	public static class GameDataSaveLoader
	{
		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x0007420D File Offset: 0x0007240D
		public static bool CurrentGameStateIsValuable
		{
			get
			{
				return Find.TickManager.TicksGame > GameDataSaveLoader.lastSaveTick + 60;
			}
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x00074224 File Offset: 0x00072424
		public static void SaveScenario(Scenario scen, string absFilePath)
		{
			try
			{
				scen.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "savedscenario", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<Scenario>(ref scen, "scenario", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving world: " + ex.ToString(), false);
			}
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x00074294 File Offset: 0x00072494
		public static bool TryLoadScenario(string absPath, ScenarioCategory category, out Scenario scen)
		{
			scen = null;
			try
			{
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Scenario, true);
					Scribe_Deep.Look<Scenario>(ref scen, "scenario", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				scen.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
				scen.Category = category;
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading scenario: " + ex.ToString(), false);
				scen = null;
				Scribe.ForceStop();
			}
			return scen != null;
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x00074340 File Offset: 0x00072540
		public static void SaveGame(string fileName)
		{
			try
			{
				SafeSaver.Save(GenFilePaths.FilePathForSavedGame(fileName), "savegame", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Game game = Current.Game;
					Scribe_Deep.Look<Game>(ref game, "game", Array.Empty<object>());
				}, Find.GameInfo.permadeathMode);
				GameDataSaveLoader.lastSaveTick = Find.TickManager.TicksGame;
			}
			catch (Exception arg)
			{
				Log.Error("Exception while saving game: " + arg, false);
			}
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x000743BC File Offset: 0x000725BC
		public static void CheckVersionAndLoadGame(string saveFileName)
		{
			PreLoadUtility.CheckVersionAndLoad(GenFilePaths.FilePathForSavedGame(saveFileName), ScribeMetaHeaderUtility.ScribeHeaderMode.Map, delegate
			{
				GameDataSaveLoader.LoadGame(saveFileName);
			});
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x000743F3 File Offset: 0x000725F3
		public static void LoadGame(string saveFileName)
		{
			LongEventHandler.QueueLongEvent(delegate
			{
				MemoryUtility.ClearAllMapsAndWorld();
				Current.Game = new Game();
				Current.Game.InitData = new GameInitData();
				Current.Game.InitData.gameToLoad = saveFileName;
			}, "Play", "LoadingLongEvent", true, null, true);
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x0007441E File Offset: 0x0007261E
		public static void LoadGame(FileInfo saveFile)
		{
			GameDataSaveLoader.LoadGame(Path.GetFileNameWithoutExtension(saveFile.Name));
		}

		// Token: 0x04000D70 RID: 3440
		private static int lastSaveTick = -9999;

		// Token: 0x04000D71 RID: 3441
		public const string SavedScenarioParentNodeName = "savedscenario";

		// Token: 0x04000D72 RID: 3442
		public const string SavedWorldParentNodeName = "savedworld";

		// Token: 0x04000D73 RID: 3443
		public const string SavedGameParentNodeName = "savegame";

		// Token: 0x04000D74 RID: 3444
		public const string GameNodeName = "game";

		// Token: 0x04000D75 RID: 3445
		public const string WorldNodeName = "world";

		// Token: 0x04000D76 RID: 3446
		public const string ScenarioNodeName = "scenario";

		// Token: 0x04000D77 RID: 3447
		public const string AutosavePrefix = "Autosave";

		// Token: 0x04000D78 RID: 3448
		public const string AutostartSaveName = "autostart";
	}
}
