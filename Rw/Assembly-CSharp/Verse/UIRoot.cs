using System;
using RimWorld;
using UnityEngine;
using Verse.Noise;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003BF RID: 959
	public abstract class UIRoot
	{
		// Token: 0x06001C41 RID: 7233 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Init()
		{
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x000ABD24 File Offset: 0x000A9F24
		public virtual void UIRootOnGUI()
		{
			UnityGUIBugsFixer.OnGUI();
			Text.StartOfOnGUI();
			this.CheckOpenLogWindow();
			DelayedErrorWindowRequest.DelayedErrorWindowRequestOnGUI();
			DebugInputLogger.InputLogOnGUI();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				this.debugWindowOpener.DevToolStarterOnGUI();
			}
			this.windows.HandleEventsHighPriority();
			this.screenshotMode.ScreenshotModesOnGUI();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				TooltipHandler.DoTooltipGUI();
				this.feedbackFloaters.FeedbackOnGUI();
				DragSliderManager.DragSlidersOnGUI();
				Messages.MessagesDoGUI();
			}
			this.shortcutKeys.ShortcutKeysOnGUI();
			NoiseDebugUI.NoiseDebugOnGUI();
			Debug.developerConsoleVisible = false;
			if (Current.Game != null)
			{
				GameComponentUtility.GameComponentOnGUI();
			}
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x000ABDC2 File Offset: 0x000A9FC2
		public virtual void UIRootUpdate()
		{
			ScreenshotTaker.Update();
			DragSliderManager.DragSlidersUpdate();
			this.windows.WindowsUpdate();
			MouseoverSounds.ResolveFrame();
			UIHighlighter.UIHighlighterUpdate();
			Messages.Update();
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x000ABDE8 File Offset: 0x000A9FE8
		private void CheckOpenLogWindow()
		{
			if (EditWindow_Log.wantsToOpen && !Find.WindowStack.IsOpen(typeof(EditWindow_Log)))
			{
				Find.WindowStack.Add(new EditWindow_Log());
				EditWindow_Log.wantsToOpen = false;
			}
		}

		// Token: 0x040010AA RID: 4266
		public WindowStack windows = new WindowStack();

		// Token: 0x040010AB RID: 4267
		protected DebugWindowsOpener debugWindowOpener = new DebugWindowsOpener();

		// Token: 0x040010AC RID: 4268
		public ScreenshotModeHandler screenshotMode = new ScreenshotModeHandler();

		// Token: 0x040010AD RID: 4269
		private ShortcutKeys shortcutKeys = new ShortcutKeys();

		// Token: 0x040010AE RID: 4270
		public FeedbackFloaters feedbackFloaters = new FeedbackFloaters();
	}
}
