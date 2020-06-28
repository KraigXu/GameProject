using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200034F RID: 847
	public class DebugWindowsOpener
	{
		// Token: 0x060019EA RID: 6634 RVA: 0x0009EFF3 File Offset: 0x0009D1F3
		public DebugWindowsOpener()
		{
			this.drawButtonsCached = new Action(this.DrawButtons);
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x0009F018 File Offset: 0x0009D218
		public void DevToolStarterOnGUI()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			Vector2 vector = new Vector2((float)UI.screenWidth * 0.5f, 3f);
			int num = 6;
			if (Current.ProgramState == ProgramState.Playing)
			{
				num += 2;
			}
			float num2 = 25f;
			if (Current.ProgramState == ProgramState.Playing && DebugSettings.godMode)
			{
				num2 += 15f;
			}
			Find.WindowStack.ImmediateWindow(1593759361, new Rect(vector.x, vector.y, (float)num * 28f - 4f + 1f, num2).Rounded(), WindowLayer.GameUI, this.drawButtonsCached, false, false, 0f);
			if (KeyBindingDefOf.Dev_ToggleDebugLog.KeyDownEvent)
			{
				this.ToggleLogWindow();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugActionsMenu.KeyDownEvent)
			{
				this.ToggleDebugActionsMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugLogMenu.KeyDownEvent)
			{
				this.ToggleDebugLogMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugSettingsMenu.KeyDownEvent)
			{
				this.ToggleDebugSettingsMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugInspector.KeyDownEvent)
			{
				this.ToggleDebugInspector();
				Event.current.Use();
			}
			if (Current.ProgramState == ProgramState.Playing && KeyBindingDefOf.Dev_ToggleGodMode.KeyDownEvent)
			{
				this.ToggleGodMode();
				Event.current.Use();
			}
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x0009F168 File Offset: 0x0009D368
		private void DrawButtons()
		{
			this.widgetRow.Init(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			if (this.widgetRow.ButtonIcon(TexButton.ToggleLog, "Open the debug log.", null, true))
			{
				this.ToggleLogWindow();
			}
			if (this.widgetRow.ButtonIcon(TexButton.ToggleTweak, "Open tweakvalues menu.\n\nThis lets you change internal values.", null, true))
			{
				this.ToggleTweakValuesMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenInspectSettings, "Open the view settings.\n\nThis lets you see special debug visuals.", null, true))
			{
				this.ToggleDebugSettingsMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenDebugActionsMenu, "Open debug actions menu.\n\nThis lets you spawn items and force various events.", null, true))
			{
				this.ToggleDebugActionsMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenDebugActionsMenu, "Open debug logging menu.", null, true))
			{
				this.ToggleDebugLogMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenInspector, "Open the inspector.\n\nThis lets you inspect what's happening in the game, down to individual variables.", null, true))
			{
				this.ToggleDebugInspector();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (this.widgetRow.ButtonIcon(TexButton.ToggleGodMode, "Toggle god mode.\n\nWhen god mode is on, you can build stuff instantly, for free, and sell things that aren't yours.", null, true))
				{
					this.ToggleGodMode();
				}
				if (DebugSettings.godMode)
				{
					Text.Font = GameFont.Tiny;
					Widgets.Label(new Rect(0f, 25f, 200f, 100f), "God mode");
				}
				bool pauseOnError = Prefs.PauseOnError;
				this.widgetRow.ToggleableIcon(ref pauseOnError, TexButton.TogglePauseOnError, "Pause the game when an error is logged.", null, null);
				Prefs.PauseOnError = pauseOnError;
			}
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x0009F303 File Offset: 0x0009D503
		private void ToggleLogWindow()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_Log), true))
			{
				Find.WindowStack.Add(new EditWindow_Log());
			}
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x0009F32B File Offset: 0x0009D52B
		private void ToggleDebugSettingsMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(Dialog_DebugSettingsMenu), true))
			{
				Find.WindowStack.Add(new Dialog_DebugSettingsMenu());
			}
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x0009F353 File Offset: 0x0009D553
		private void ToggleDebugActionsMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(Dialog_DebugActionsMenu), true))
			{
				Find.WindowStack.Add(new Dialog_DebugActionsMenu());
			}
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x0009F37B File Offset: 0x0009D57B
		private void ToggleTweakValuesMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_TweakValues), true))
			{
				Find.WindowStack.Add(new EditWindow_TweakValues());
			}
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x0009F3A3 File Offset: 0x0009D5A3
		private void ToggleDebugLogMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(Dialog_DebugOutputMenu), true))
			{
				Find.WindowStack.Add(new Dialog_DebugOutputMenu());
			}
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x0009F3CB File Offset: 0x0009D5CB
		private void ToggleDebugInspector()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_DebugInspector), true))
			{
				Find.WindowStack.Add(new EditWindow_DebugInspector());
			}
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x0009F3F3 File Offset: 0x0009D5F3
		private void ToggleGodMode()
		{
			DebugSettings.godMode = !DebugSettings.godMode;
		}

		// Token: 0x04000F12 RID: 3858
		private Action drawButtonsCached;

		// Token: 0x04000F13 RID: 3859
		private WidgetRow widgetRow = new WidgetRow();
	}
}
