using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Spirit
{
    public abstract class Root : MonoBehaviour
    {

		private static bool globalInitDone;

		private static bool prefsApplied;

		protected static bool checkedAutostartSaveFile;

		protected bool destroyed;

		//public SoundRoot soundRoot;

		//public UIRoot uiRoot;


		protected virtual void Start()
        {
			//try
			//{
			//	CultureInfoUtility.EnsureEnglish();
			//	Current.Notify_LoadedSceneChanged();
			//	Root.CheckGlobalInit();
			//	Action action = delegate
			//	{
			//		DeepProfiler.Start("Misc Init (InitializingInterface)");
			//		try
			//		{
			//			this.soundRoot = new SoundRoot();
			//			if (GenScene.InPlayScene)
			//			{
			//				this.uiRoot = new UIRoot_Play();
			//			}
			//			else if (GenScene.InEntryScene)
			//			{
			//				this.uiRoot = new UIRoot_Entry();
			//			}
			//			this.uiRoot.Init();
			//			Messages.Notify_LoadedLevelChanged();
			//			if (Current.SubcameraDriver != null)
			//			{
			//				Current.SubcameraDriver.Init();
			//			}
			//		}
			//		finally
			//		{
			//			DeepProfiler.End();
			//		}
			//	};
			//	if (!PlayDataLoader.Loaded)
			//	{
			//		Application.runInBackground = true;
			//		LongEventHandler.QueueLongEvent(delegate
			//		{
			//			PlayDataLoader.LoadAllPlayData(false);
			//		}, null, true, null, true);
			//		LongEventHandler.QueueLongEvent(action, "InitializingInterface", false, null, true);
			//	}
			//	else
			//	{
			//		action();
			//	}
			//}
			//catch (Exception arg)
			//{
			//	Log.Error("Critical error in root Start(): " + arg, false);
			//}
		}
		protected virtual void Update()
		{
			try
			{
				//ResolutionUtility.Update();
				RealTime.Update();
				bool flag;
				//LongEventHandler.LongEventsUpdate(out flag);
				//if (flag)
				//{
				//	this.destroyed = true;
				//}
				//else if (!LongEventHandler.ShouldWaitForEvent)
				//{
				//	Rand.EnsureStateStackEmpty();
				//	Widgets.EnsureMousePositionStackEmpty();
				//	SteamManager.Update();
				//	PortraitsCache.PortraitsCacheUpdate();
				//	AttackTargetsCache.AttackTargetsCacheStaticUpdate();
				//	Pawn_MeleeVerbs.PawnMeleeVerbsStaticUpdate();
				//	Storyteller.StorytellerStaticUpdate();
				//	CaravanInventoryUtility.CaravanInventoryUtilityStaticUpdate();
				//	this.uiRoot.UIRootUpdate();
				//	if (Time.frameCount > 3 && !Root.prefsApplied)
				//	{
				//		Root.prefsApplied = true;
				//		Prefs.Apply();
				//	}
				//	this.soundRoot.Update();
				//}
			}
			catch (Exception arg)
			{
				//Log.Error("Root level exception in Update(): " + arg, false);
			}
		}

	}
}
