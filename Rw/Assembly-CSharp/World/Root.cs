using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World
{
    public abstract class Root : MonoBehaviour
    {

		protected virtual void Start()
        {

        }


		protected virtual void Update()
		{
			try
			{
				//ResolutionUtility.Update();
				//RealTime.Update();
				//bool flag;
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
