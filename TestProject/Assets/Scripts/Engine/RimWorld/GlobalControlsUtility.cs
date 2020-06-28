using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E96 RID: 3734
	public static class GlobalControlsUtility
	{
		// Token: 0x06005B07 RID: 23303 RVA: 0x001F5100 File Offset: 0x001F3300
		public static void DoPlaySettings(WidgetRow rowVisibility, bool worldView, ref float curBaseY)
		{
			float y = curBaseY - TimeControls.TimeButSize.y;
			rowVisibility.Init((float)UI.screenWidth, y, UIDirection.LeftThenUp, 141f, 4f);
			Find.PlaySettings.DoPlaySettingsGlobalControls(rowVisibility, worldView);
			curBaseY = rowVisibility.FinalY;
		}

		// Token: 0x06005B08 RID: 23304 RVA: 0x001F5148 File Offset: 0x001F3348
		public static void DoTimespeedControls(float leftX, float width, ref float curBaseY)
		{
			leftX += Mathf.Max(0f, width - 150f);
			width = Mathf.Min(width, 150f);
			float y = TimeControls.TimeButSize.y;
			Rect timerRect = new Rect(leftX + 16f, curBaseY - y, width, y);
			TimeControls.DoTimeControlsGUI(timerRect);
			curBaseY -= timerRect.height;
		}

		// Token: 0x06005B09 RID: 23305 RVA: 0x001F51A8 File Offset: 0x001F33A8
		public static void DoDate(float leftX, float width, ref float curBaseY)
		{
			Rect dateRect = new Rect(leftX, curBaseY - DateReadout.Height, width, DateReadout.Height);
			DateReadout.DateOnGUI(dateRect);
			curBaseY -= dateRect.height;
		}

		// Token: 0x06005B0A RID: 23306 RVA: 0x001F51E0 File Offset: 0x001F33E0
		public static void DoRealtimeClock(float leftX, float width, ref float curBaseY)
		{
			Rect rect = new Rect(leftX - 20f, curBaseY - 26f, width + 20f - 7f, 26f);
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect, DateTime.Now.ToString("HH:mm"));
			Text.Anchor = TextAnchor.UpperLeft;
			curBaseY -= 26f;
		}

		// Token: 0x040031AE RID: 12718
		private const int VisibilityControlsPerRow = 5;
	}
}
