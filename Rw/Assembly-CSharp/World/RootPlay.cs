using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World
{
    public class RootPlay : Root
    {
        protected override void Start()
        {
            //Log.ResetMessageCount();
            //base.Start();
            //try
            //{
            //	this.musicManagerPlay = new MusicManagerPlay();
            //	FileInfo autostart = Root.checkedAutostartSaveFile ? null : SaveGameFilesUtility.GetAutostartSaveFile();
            //	Root.checkedAutostartSaveFile = true;
            //	if (autostart != null)
            //	{
            //		LongEventHandler.QueueLongEvent(delegate
            //		{
            //			SavedGameLoaderNow.LoadGameFromSaveFileNow(Path.GetFileNameWithoutExtension(autostart.Name));
            //		}, "LoadingLongEvent", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileLoadingGame), true);
            //	}
            //	else if (Find.GameInitData != null && !Find.GameInitData.gameToLoad.NullOrEmpty())
            //	{
            //		LongEventHandler.QueueLongEvent(delegate
            //		{
            //			SavedGameLoaderNow.LoadGameFromSaveFileNow(Find.GameInitData.gameToLoad);
            //		}, "LoadingLongEvent", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileLoadingGame), true);
            //	}
            //	else
            //	{
            //		LongEventHandler.QueueLongEvent(delegate
            //		{
            //			if (Current.Game == null)
            //			{
            //				Root_Play.SetupForQuickTestPlay();
            //			}
            //			Current.Game.InitNewGame();
            //		}, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true);
            //	}
            //	LongEventHandler.QueueLongEvent(delegate
            //	{
            //		ScreenFader.SetColor(Color.black);
            //		ScreenFader.StartFade(Color.clear, 0.5f);
            //	}, null, false, null, true);
            //}
            //catch (Exception arg)
            //{
            //	Log.Error("Critical error in root Start(): " + arg, false);
            //}
        }

        protected override void Update()
        {
            base.Update();

            Current.Game.UpdatePlay();


            // tr
            //if (LongEventHandler.ShouldWaitForEvent || this.destroyed)
            //{
            //    return;
            //}
            //try
            //{
            //    ShipCountdown.ShipCountdownUpdate();
            //    TargetHighlighter.TargetHighlighterUpdate();
            //    Current.Game.UpdatePlay();
            //    this.musicManagerPlay.MusicUpdate();
            //}
            //catch (Exception arg)
            //{
            //    Log.Error("Root level exception in Update(): " + arg, false);
            //}
        }


    }
}
