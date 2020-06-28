using System;
using System.IO;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000124 RID: 292
	public class Root_Play : Root
	{
		// Token: 0x06000831 RID: 2097 RVA: 0x00025C20 File Offset: 0x00023E20
		public override void Start()
		{
			Log.ResetMessageCount();
			base.Start();
			try
			{
				this.musicManagerPlay = new MusicManagerPlay();
				FileInfo autostart = Root.checkedAutostartSaveFile ? null : SaveGameFilesUtility.GetAutostartSaveFile();
				Root.checkedAutostartSaveFile = true;
				if (autostart != null)
				{
					LongEventHandler.QueueLongEvent(delegate
					{
						SavedGameLoaderNow.LoadGameFromSaveFileNow(Path.GetFileNameWithoutExtension(autostart.Name));
					}, "LoadingLongEvent", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileLoadingGame), true);
				}
				else if (Find.GameInitData != null && !Find.GameInitData.gameToLoad.NullOrEmpty())
				{
					LongEventHandler.QueueLongEvent(delegate
					{
						SavedGameLoaderNow.LoadGameFromSaveFileNow(Find.GameInitData.gameToLoad);
					}, "LoadingLongEvent", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileLoadingGame), true);
				}
				else
				{
					LongEventHandler.QueueLongEvent(delegate
					{
						if (Current.Game == null)
						{
							Root_Play.SetupForQuickTestPlay();
						}
						Current.Game.InitNewGame();
					}, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true);
				}
				LongEventHandler.QueueLongEvent(delegate
				{
					ScreenFader.SetColor(Color.black);
					ScreenFader.StartFade(Color.clear, 0.5f);
				}, null, false, null, true);
			}
			catch (Exception arg)
			{
				Log.Error("Critical error in root Start(): " + arg, false);
			}
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00025D78 File Offset: 0x00023F78
		public override void Update()
		{
			base.Update();
			if (LongEventHandler.ShouldWaitForEvent || this.destroyed)
			{
				return;
			}
			try
			{
				ShipCountdown.ShipCountdownUpdate();
				TargetHighlighter.TargetHighlighterUpdate();
				Current.Game.UpdatePlay();
				this.musicManagerPlay.MusicUpdate();
			}
			catch (Exception arg)
			{
				Log.Error("Root level exception in Update(): " + arg, false);
			}
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00025DE4 File Offset: 0x00023FE4
		private static void SetupForQuickTestPlay()
		{
			Current.ProgramState = ProgramState.Entry;
			Current.Game = new Game();
			Current.Game.InitData = new GameInitData();
			Current.Game.Scenario = ScenarioDefOf.Crashlanded.scenario;
			Find.Scenario.PreConfigure();
			Current.Game.storyteller = new Storyteller(StorytellerDefOf.Cassandra, DifficultyDefOf.Rough);
			Current.Game.World = WorldGenerator.GenerateWorld(0.05f, GenText.RandomSeedString(), OverallRainfall.Normal, OverallTemperature.Normal, OverallPopulation.Normal);
			Find.GameInitData.ChooseRandomStartingTile();
			Find.GameInitData.mapSize = 150;
			Find.GameInitData.PrepForMapGen();
			Find.Scenario.PreMapGenerate();
		}

		// Token: 0x04000742 RID: 1858
		public MusicManagerPlay musicManagerPlay;
	}
}
