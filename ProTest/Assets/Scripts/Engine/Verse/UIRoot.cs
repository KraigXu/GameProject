using RimWorld;
using UnityEngine;
using UnityEngine.UI;
using Verse.Noise;
using Verse.Sound;
using Verse.UIFrameWork;

namespace Verse
{
	public abstract class UIRoot
	{
		public WindowStack windows = new WindowStack();

		protected DebugWindowsOpener debugWindowOpener = new DebugWindowsOpener();

		public ScreenshotModeHandler screenshotMode = new ScreenshotModeHandler();

		private ShortcutKeys shortcutKeys = new ShortcutKeys();

		public FeedbackFloaters feedbackFloaters = new FeedbackFloaters();

		public UICenterMasterManager uiCenter;

		public virtual void Init()
		{
			GameObject gameObject = new GameObject("UICanvas",typeof(Canvas));
			gameObject.SetActive(value: true);
			GameObject cameragameObject = new GameObject("UICamera", typeof(Camera));
			cameragameObject.transform.SetParent(gameObject.transform);
			Camera component = cameragameObject.GetComponent<Camera>();
			component.transform.position = Vector3.zero;
			component.transform.rotation = Quaternion.identity;
			component.orthographic = true;
			component.cullingMask = 0;
			component.orthographicSize = 1f;
			component.clearFlags = CameraClearFlags.Color;
			component.backgroundColor = new Color(0f, 0f, 0f, 0f);
			component.useOcclusionCulling = false;
			component.renderingPath = RenderingPath.Forward;
			component.nearClipPlane = -1;
			component.farClipPlane = 1;
			
			Canvas canvas = gameObject.GetComponent<Canvas>();
			canvas.renderMode =RenderMode.ScreenSpaceCamera;
			canvas.worldCamera = component;

			CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();

			GraphicRaycaster graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();


			uiCenter = gameObject.AddComponent<UICenterMasterManager>();
		}

		public virtual void UIRootOnGUI()
		{
			UnityGUIBugsFixer.OnGUI();
			Text.StartOfOnGUI();
			CheckOpenLogWindow();
			DelayedErrorWindowRequest.DelayedErrorWindowRequestOnGUI();
			DebugInputLogger.InputLogOnGUI();
			if (!screenshotMode.FiltersCurrentEvent)
			{
				debugWindowOpener.DevToolStarterOnGUI();
			}
			windows.HandleEventsHighPriority();
			screenshotMode.ScreenshotModesOnGUI();
			if (!screenshotMode.FiltersCurrentEvent)
			{
				TooltipHandler.DoTooltipGUI();
				feedbackFloaters.FeedbackOnGUI();
				DragSliderManager.DragSlidersOnGUI();
				Messages.MessagesDoGUI();
			}
			shortcutKeys.ShortcutKeysOnGUI();
			NoiseDebugUI.NoiseDebugOnGUI();
			Debug.developerConsoleVisible = false;
			if (Current.Game != null)
			{
				GameComponentUtility.GameComponentOnGUI();
			}
		}

		public virtual void UIRootUpdate()
		{
			ScreenshotTaker.Update();
			DragSliderManager.DragSlidersUpdate();
			windows.WindowsUpdate();
			MouseoverSounds.ResolveFrame();
			UIHighlighter.UIHighlighterUpdate();
			Messages.Update();
		}

		private void CheckOpenLogWindow()
		{
			if (EditWindow_Log.wantsToOpen && !Find.WindowStack.IsOpen(typeof(EditWindow_Log)))
			{
				Find.WindowStack.Add(new EditWindow_Log());
				EditWindow_Log.wantsToOpen = false;
			}
		}
	}
}
