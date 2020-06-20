using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F2E RID: 3886
	public class UIRoot_Play : UIRoot
	{
		// Token: 0x06005F2F RID: 24367 RVA: 0x0020D948 File Offset: 0x0020BB48
		public override void Init()
		{
			base.Init();
			Messages.Clear();
		}

		// Token: 0x06005F30 RID: 24368 RVA: 0x0020D958 File Offset: 0x0020BB58
		public override void UIRootOnGUI()
		{
			base.UIRootOnGUI();
			Find.GameInfo.GameInfoOnGUI();
			Find.World.UI.WorldInterfaceOnGUI();
			this.mapUI.MapInterfaceOnGUI_BeforeMainTabs();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				this.mainButtonsRoot.MainButtonsOnGUI();
				this.alerts.AlertsReadoutOnGUI();
			}
			this.mapUI.MapInterfaceOnGUI_AfterMainTabs();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				Find.Tutor.TutorOnGUI();
			}
			ReorderableWidget.ReorderableWidgetOnGUI_BeforeWindowStack();
			this.windows.WindowStackOnGUI();
			ReorderableWidget.ReorderableWidgetOnGUI_AfterWindowStack();
			Widgets.WidgetsOnGUI();
			this.mapUI.HandleMapClicks();
			if (Find.DesignatorManager.SelectedDesignator != null)
			{
				Find.DesignatorManager.SelectedDesignator.SelectedProcessInput(Event.current);
			}
			DebugTools.DebugToolsOnGUI();
			this.mainButtonsRoot.HandleLowPriorityShortcuts();
			Find.World.UI.HandleLowPriorityInput();
			this.mapUI.HandleLowPriorityInput();
			this.OpenMainMenuShortcut();
		}

		// Token: 0x06005F31 RID: 24369 RVA: 0x0020DA4C File Offset: 0x0020BC4C
		public override void UIRootUpdate()
		{
			base.UIRootUpdate();
			try
			{
				Find.World.UI.WorldInterfaceUpdate();
				this.mapUI.MapInterfaceUpdate();
				this.alerts.AlertsReadoutUpdate();
				LessonAutoActivator.LessonAutoActivatorUpdate();
				Find.Tutor.TutorUpdate();
			}
			catch (Exception ex)
			{
				Log.Error("Exception in UIRootUpdate: " + ex.ToString(), false);
			}
		}

		// Token: 0x06005F32 RID: 24370 RVA: 0x0020DAC0 File Offset: 0x0020BCC0
		private void OpenMainMenuShortcut()
		{
			if ((Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape) || KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				Event.current.Use();
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Menu, true);
			}
		}

		// Token: 0x04003398 RID: 13208
		public MapInterface mapUI = new MapInterface();

		// Token: 0x04003399 RID: 13209
		public MainButtonsRoot mainButtonsRoot = new MainButtonsRoot();

		// Token: 0x0400339A RID: 13210
		public AlertsReadout alerts = new AlertsReadout();
	}
}
