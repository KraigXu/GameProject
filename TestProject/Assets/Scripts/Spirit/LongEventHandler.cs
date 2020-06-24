using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RimWorld;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spirit
{
	// Token: 0x02000034 RID: 52
	public static class LongEventHandler
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000F67B File Offset: 0x0000D87B
		public static bool ShouldWaitForEvent
		{
			get
			{
				return LongEventHandler.AnyEventNowOrWaiting && ((LongEventHandler.currentEvent != null && !LongEventHandler.currentEvent.UseStandardWindow) || (Find.UIRoot == null || Find.WindowStack == null));
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0000F6AC File Offset: 0x0000D8AC
		public static bool AnyEventNowOrWaiting
		{
			get
			{
				return LongEventHandler.currentEvent != null || LongEventHandler.eventQueue.Count > 0;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000F6C4 File Offset: 0x0000D8C4
		public static bool AnyEventWhichDoesntUseStandardWindowNowOrWaiting
		{
			get
			{
				LongEventHandler.QueuedLongEvent queuedLongEvent = LongEventHandler.currentEvent;
				if (queuedLongEvent != null && !queuedLongEvent.UseStandardWindow)
				{
					return true;
				}
				return LongEventHandler.eventQueue.Any((LongEventHandler.QueuedLongEvent x) => !x.UseStandardWindow);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000F70D File Offset: 0x0000D90D
		public static bool ForcePause
		{
			get
			{
				return LongEventHandler.AnyEventNowOrWaiting;
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000F714 File Offset: 0x0000D914
		public static void QueueLongEvent(Action action, string textKey, bool doAsynchronously, Action<Exception> exceptionHandler, bool showExtraUIInfo = true)
		{
			LongEventHandler.QueuedLongEvent queuedLongEvent = new LongEventHandler.QueuedLongEvent();
			queuedLongEvent.eventAction = action;
			queuedLongEvent.eventTextKey = textKey;
			queuedLongEvent.doAsynchronously = doAsynchronously;
			queuedLongEvent.exceptionHandler = exceptionHandler;
			queuedLongEvent.canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting;
			queuedLongEvent.showExtraUIInfo = showExtraUIInfo;
			LongEventHandler.eventQueue.Enqueue(queuedLongEvent);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000F764 File Offset: 0x0000D964
		public static void QueueLongEvent(IEnumerable action, string textKey, Action<Exception> exceptionHandler = null, bool showExtraUIInfo = true)
		{
			LongEventHandler.QueuedLongEvent queuedLongEvent = new LongEventHandler.QueuedLongEvent();
			queuedLongEvent.eventActionEnumerator = action.GetEnumerator();
			queuedLongEvent.eventTextKey = textKey;
			queuedLongEvent.doAsynchronously = false;
			queuedLongEvent.exceptionHandler = exceptionHandler;
			queuedLongEvent.canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting;
			queuedLongEvent.showExtraUIInfo = showExtraUIInfo;
			LongEventHandler.eventQueue.Enqueue(queuedLongEvent);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000F7B8 File Offset: 0x0000D9B8
		public static void QueueLongEvent(Action preLoadLevelAction, string levelToLoad, string textKey, bool doAsynchronously, Action<Exception> exceptionHandler, bool showExtraUIInfo = true)
		{
			LongEventHandler.QueuedLongEvent queuedLongEvent = new LongEventHandler.QueuedLongEvent();
			queuedLongEvent.eventAction = preLoadLevelAction;
			queuedLongEvent.levelToLoad = levelToLoad;
			queuedLongEvent.eventTextKey = textKey;
			queuedLongEvent.doAsynchronously = doAsynchronously;
			queuedLongEvent.exceptionHandler = exceptionHandler;
			queuedLongEvent.canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting;
			queuedLongEvent.showExtraUIInfo = showExtraUIInfo;
			LongEventHandler.eventQueue.Enqueue(queuedLongEvent);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000F810 File Offset: 0x0000DA10
		public static void ClearQueuedEvents()
		{
			LongEventHandler.eventQueue.Clear();
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000F81C File Offset: 0x0000DA1C
		public static void LongEventsOnGUI()
		{
			if (LongEventHandler.currentEvent == null)
			{
				GameplayTipWindow.ResetTipTimer();
				return;
			}
			float num = LongEventHandler.StatusRectSize.x;
			object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
			lock (currentEventTextLock)
			{
				Text.Font = GameFont.Small;
				num = Mathf.Max(num, Text.CalcSize(LongEventHandler.currentEvent.eventText + "...").x + 40f);
			}
			bool flag2 = Find.UIRoot != null && !LongEventHandler.currentEvent.UseStandardWindow && LongEventHandler.currentEvent.showExtraUIInfo;
			bool flag3 = Find.UIRoot != null && Current.Game != null && !LongEventHandler.currentEvent.UseStandardWindow && LongEventHandler.currentEvent.showExtraUIInfo;
			Vector2 vector = flag3 ? ModSummaryWindow.GetEffectiveSize() : Vector2.zero;
			float num2 = LongEventHandler.StatusRectSize.y;
			if (flag3)
			{
				num2 += 17f + vector.y;
			}
			if (flag2)
			{
				num2 += 17f + GameplayTipWindow.WindowSize.y;
			}
			float num3 = ((float)UI.screenHeight - num2) / 2f;
			Vector2 vector2 = new Vector2(((float)UI.screenWidth - GameplayTipWindow.WindowSize.x) / 2f, num3 + LongEventHandler.StatusRectSize.y + 17f);
			Vector2 offset = new Vector2(((float)UI.screenWidth - vector.x) / 2f, vector2.y + GameplayTipWindow.WindowSize.y + 17f);
			Rect rect = new Rect(((float)UI.screenWidth - num) / 2f, num3, num, LongEventHandler.StatusRectSize.y);
			rect = rect.Rounded();
			if (!LongEventHandler.currentEvent.UseStandardWindow || Find.UIRoot == null || Find.WindowStack == null)
			{
				if (UIMenuBackgroundManager.background == null)
				{
					UIMenuBackgroundManager.background = new UI_BackgroundMain();
				}
				UIMenuBackgroundManager.background.BackgroundOnGUI();
				Widgets.DrawShadowAround(rect);
				Widgets.DrawWindowBackground(rect);
				LongEventHandler.DrawLongEventWindowContents(rect);
				if (flag2)
				{
					GameplayTipWindow.DrawWindow(vector2, false);
				}
				if (flag3)
				{
					ModSummaryWindow.DrawWindow(offset, false);
					TooltipHandler.DoTooltipGUI();
					return;
				}
			}
			else
			{
				LongEventHandler.DrawLongEventWindow(rect);
				if (flag2)
				{
					GameplayTipWindow.DrawWindow(vector2, true);
				}
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000FA4C File Offset: 0x0000DC4C
		private static void DrawLongEventWindow(Rect statusRect)
		{
			Find.WindowStack.ImmediateWindow(62893994, statusRect, WindowLayer.Super, delegate
			{
				LongEventHandler.DrawLongEventWindowContents(statusRect.AtZero());
			}, true, false, 1f);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000FA90 File Offset: 0x0000DC90
		public static void LongEventsUpdate(out bool sceneChanged)
		{
			sceneChanged = false;
			if (LongEventHandler.currentEvent != null)
			{
				if (LongEventHandler.currentEvent.eventActionEnumerator != null)
				{
					LongEventHandler.UpdateCurrentEnumeratorEvent();
				}
				else if (LongEventHandler.currentEvent.doAsynchronously)
				{
					LongEventHandler.UpdateCurrentAsynchronousEvent();
				}
				else
				{
					LongEventHandler.UpdateCurrentSynchronousEvent(out sceneChanged);
				}
			}
			if (LongEventHandler.currentEvent == null && LongEventHandler.eventQueue.Count > 0)
			{
				LongEventHandler.currentEvent = LongEventHandler.eventQueue.Dequeue();
				if (LongEventHandler.currentEvent.eventTextKey == null)
				{
					LongEventHandler.currentEvent.eventText = "";
					return;
				}
				LongEventHandler.currentEvent.eventText = LongEventHandler.currentEvent.eventTextKey.Translate();
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000FB30 File Offset: 0x0000DD30
		public static void ExecuteWhenFinished(Action action)
		{
			LongEventHandler.toExecuteWhenFinished.Add(action);
			if ((LongEventHandler.currentEvent == null || LongEventHandler.currentEvent.ShouldWaitUntilDisplayed) && !LongEventHandler.executingToExecuteWhenFinished)
			{
				LongEventHandler.ExecuteToExecuteWhenFinished();
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000FB5C File Offset: 0x0000DD5C
		public static void SetCurrentEventText(string newText)
		{
			object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
			lock (currentEventTextLock)
			{
				if (LongEventHandler.currentEvent != null)
				{
					LongEventHandler.currentEvent.eventText = newText;
				}
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000FBAC File Offset: 0x0000DDAC
		private static void UpdateCurrentEnumeratorEvent()
		{
			try
			{
				float num = Time.realtimeSinceStartup + 0.1f;
				while (LongEventHandler.currentEvent.eventActionEnumerator.MoveNext())
				{
					if (num <= Time.realtimeSinceStartup)
					{
						return;
					}
				}
				IDisposable disposable = LongEventHandler.currentEvent.eventActionEnumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
				LongEventHandler.ExecuteToExecuteWhenFinished();
			}
			catch (Exception ex)
			{
				Log.Error("Exception from long event: " + ex, false);
				if (LongEventHandler.currentEvent != null)
				{
					IDisposable disposable2 = LongEventHandler.currentEvent.eventActionEnumerator as IDisposable;
					if (disposable2 != null)
					{
						disposable2.Dispose();
					}
					if (LongEventHandler.currentEvent.exceptionHandler != null)
					{
						LongEventHandler.currentEvent.exceptionHandler(ex);
					}
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
			}
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000FC84 File Offset: 0x0000DE84
		private static void UpdateCurrentAsynchronousEvent()
		{
			if (LongEventHandler.eventThread == null)
			{
				LongEventHandler.eventThread = new Thread(delegate
				{
					LongEventHandler.RunEventFromAnotherThread(LongEventHandler.currentEvent.eventAction);
				});
				LongEventHandler.eventThread.Start();
				return;
			}
			if (!LongEventHandler.eventThread.IsAlive)
			{
				bool flag = false;
				if (!LongEventHandler.currentEvent.levelToLoad.NullOrEmpty())
				{
					if (LongEventHandler.levelLoadOp == null)
					{
						LongEventHandler.levelLoadOp = SceneManager.LoadSceneAsync(LongEventHandler.currentEvent.levelToLoad);
					}
					else if (LongEventHandler.levelLoadOp.isDone)
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					LongEventHandler.currentEvent = null;
					LongEventHandler.eventThread = null;
					LongEventHandler.levelLoadOp = null;
					LongEventHandler.ExecuteToExecuteWhenFinished();
				}
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000FD34 File Offset: 0x0000DF34
		private static void UpdateCurrentSynchronousEvent(out bool sceneChanged)
		{
			sceneChanged = false;
			if (LongEventHandler.currentEvent.ShouldWaitUntilDisplayed)
			{
				return;
			}
			try
			{
				if (LongEventHandler.currentEvent.eventAction != null)
				{
					LongEventHandler.currentEvent.eventAction();
				}
				if (!LongEventHandler.currentEvent.levelToLoad.NullOrEmpty())
				{
					SceneManager.LoadScene(LongEventHandler.currentEvent.levelToLoad);
					sceneChanged = true;
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
				LongEventHandler.ExecuteToExecuteWhenFinished();
			}
			catch (Exception ex)
			{
				Log.Error("Exception from long event: " + ex, false);
				if (LongEventHandler.currentEvent != null && LongEventHandler.currentEvent.exceptionHandler != null)
				{
					LongEventHandler.currentEvent.exceptionHandler(ex);
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000FE04 File Offset: 0x0000E004
		private static void RunEventFromAnotherThread(Action action)
		{
			CultureInfoUtility.EnsureEnglish();
			try
			{
				if (action != null)
				{
					action();
				}
			}
			catch (Exception ex)
			{
				Log.Error("Exception from asynchronous event: " + ex, false);
				try
				{
					if (LongEventHandler.currentEvent != null && LongEventHandler.currentEvent.exceptionHandler != null)
					{
						LongEventHandler.currentEvent.exceptionHandler(ex);
					}
				}
				catch (Exception arg)
				{
					Log.Error("Exception was thrown while trying to handle exception. Exception: " + arg, false);
				}
			}
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000FE8C File Offset: 0x0000E08C
		private static void ExecuteToExecuteWhenFinished()
		{
			if (LongEventHandler.executingToExecuteWhenFinished)
			{
				Log.Warning("Already executing.", false);
				return;
			}
			LongEventHandler.executingToExecuteWhenFinished = true;
			if (LongEventHandler.toExecuteWhenFinished.Count > 0)
			{
				DeepProfiler.Start("ExecuteToExecuteWhenFinished()");
			}
			for (int i = 0; i < LongEventHandler.toExecuteWhenFinished.Count; i++)
			{
				DeepProfiler.Start(LongEventHandler.toExecuteWhenFinished[i].Method.DeclaringType.ToString() + " -> " + LongEventHandler.toExecuteWhenFinished[i].Method.ToString());
				try
				{
					LongEventHandler.toExecuteWhenFinished[i]();
				}
				catch (Exception arg)
				{
					Log.Error("Could not execute post-long-event action. Exception: " + arg, false);
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			if (LongEventHandler.toExecuteWhenFinished.Count > 0)
			{
				DeepProfiler.End();
			}
			LongEventHandler.toExecuteWhenFinished.Clear();
			LongEventHandler.executingToExecuteWhenFinished = false;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000FF88 File Offset: 0x0000E188
		private static void DrawLongEventWindowContents(Rect rect)
		{
			if (LongEventHandler.currentEvent == null)
			{
				return;
			}
			if (Event.current.type == EventType.Repaint)
			{
				LongEventHandler.currentEvent.alreadyDisplayed = true;
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			float num = 0f;
			if (LongEventHandler.levelLoadOp != null)
			{
				float f = 1f;
				if (!LongEventHandler.levelLoadOp.isDone)
				{
					f = LongEventHandler.levelLoadOp.progress;
				}
				TaggedString taggedString = "LoadingAssets".Translate() + " " + f.ToStringPercent();
				num = Text.CalcSize(taggedString).x;
				Widgets.Label(rect, taggedString);
			}
			else
			{
				object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
				lock (currentEventTextLock)
				{
					num = Text.CalcSize(LongEventHandler.currentEvent.eventText).x;
					Widgets.Label(rect, LongEventHandler.currentEvent.eventText);
				}
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			rect.xMin = rect.center.x + num / 2f;
			Widgets.Label(rect, (!LongEventHandler.currentEvent.UseAnimatedDots) ? "..." : GenText.MarchingEllipsis(0f));
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x040000A8 RID: 168
		private static Queue<LongEventHandler.QueuedLongEvent> eventQueue = new Queue<LongEventHandler.QueuedLongEvent>();

		// Token: 0x040000A9 RID: 169
		private static LongEventHandler.QueuedLongEvent currentEvent = null;

		// Token: 0x040000AA RID: 170
		private static Thread eventThread = null;

		// Token: 0x040000AB RID: 171
		private static AsyncOperation levelLoadOp = null;

		// Token: 0x040000AC RID: 172
		private static List<Action> toExecuteWhenFinished = new List<Action>();

		// Token: 0x040000AD RID: 173
		private static bool executingToExecuteWhenFinished = false;

		// Token: 0x040000AE RID: 174
		private static readonly object CurrentEventTextLock = new object();

		// Token: 0x040000AF RID: 175
		private static readonly Vector2 StatusRectSize = new Vector2(240f, 75f);

		// Token: 0x02001309 RID: 4873
		private class QueuedLongEvent
		{
			// Token: 0x170013A8 RID: 5032
			// (get) Token: 0x06007389 RID: 29577 RVA: 0x00282437 File Offset: 0x00280637
			public bool UseAnimatedDots
			{
				get
				{
					return this.doAsynchronously || this.eventActionEnumerator != null;
				}
			}

			// Token: 0x170013A9 RID: 5033
			// (get) Token: 0x0600738A RID: 29578 RVA: 0x0028244C File Offset: 0x0028064C
			public bool ShouldWaitUntilDisplayed
			{
				get
				{
					return !this.alreadyDisplayed && this.UseStandardWindow && !this.eventText.NullOrEmpty();
				}
			}

			// Token: 0x170013AA RID: 5034
			// (get) Token: 0x0600738B RID: 29579 RVA: 0x0028246E File Offset: 0x0028066E
			public bool UseStandardWindow
			{
				get
				{
					return this.canEverUseStandardWindow && !this.doAsynchronously && this.eventActionEnumerator == null;
				}
			}

			// Token: 0x0400480D RID: 18445
			public Action eventAction;

			// Token: 0x0400480E RID: 18446
			public IEnumerator eventActionEnumerator;

			// Token: 0x0400480F RID: 18447
			public string levelToLoad;

			// Token: 0x04004810 RID: 18448
			public string eventTextKey = "";

			// Token: 0x04004811 RID: 18449
			public string eventText = "";

			// Token: 0x04004812 RID: 18450
			public bool doAsynchronously;

			// Token: 0x04004813 RID: 18451
			public Action<Exception> exceptionHandler;

			// Token: 0x04004814 RID: 18452
			public bool alreadyDisplayed;

			// Token: 0x04004815 RID: 18453
			public bool canEverUseStandardWindow = true;

			// Token: 0x04004816 RID: 18454
			public bool showExtraUIInfo = true;
		}
	}
}
