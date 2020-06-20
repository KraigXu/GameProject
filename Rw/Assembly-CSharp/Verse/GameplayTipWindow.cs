using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200002F RID: 47
	public class GameplayTipWindow
	{
		// Token: 0x060002EA RID: 746 RVA: 0x0000F0F0 File Offset: 0x0000D2F0
		public static void DrawWindow(Vector2 offset, bool useWindowStack)
		{
			if (GameplayTipWindow.allTipsCached == null)
			{
				GameplayTipWindow.allTipsCached = DefDatabase<TipSetDef>.AllDefsListForReading.SelectMany((TipSetDef set) => set.tips).InRandomOrder(null).ToList<string>();
			}
			Rect rect = new Rect(offset.x, offset.y, GameplayTipWindow.WindowSize.x, GameplayTipWindow.WindowSize.y);
			if (useWindowStack)
			{
				Find.WindowStack.ImmediateWindow(62893997, rect, WindowLayer.Super, delegate
				{
					GameplayTipWindow.DrawContents(rect.AtZero());
				}, true, false, 1f);
				return;
			}
			Widgets.DrawShadowAround(rect);
			Widgets.DrawWindowBackground(rect);
			GameplayTipWindow.DrawContents(rect);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000F1BC File Offset: 0x0000D3BC
		private static void DrawContents(Rect rect)
		{
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			if (Time.realtimeSinceStartup - GameplayTipWindow.lastTimeUpdatedTooltip > 17.5f || GameplayTipWindow.lastTimeUpdatedTooltip < 0f)
			{
				GameplayTipWindow.currentTipIndex = (GameplayTipWindow.currentTipIndex + 1) % GameplayTipWindow.allTipsCached.Count;
				GameplayTipWindow.lastTimeUpdatedTooltip = Time.realtimeSinceStartup;
			}
			Rect rect2 = rect;
			rect2.x += GameplayTipWindow.TextMargin.x;
			rect2.width -= GameplayTipWindow.TextMargin.x * 2f;
			rect2.y += GameplayTipWindow.TextMargin.y;
			rect2.height -= GameplayTipWindow.TextMargin.y * 2f;
			Widgets.Label(rect2, GameplayTipWindow.allTipsCached[GameplayTipWindow.currentTipIndex]);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000F29D File Offset: 0x0000D49D
		public static void ResetTipTimer()
		{
			GameplayTipWindow.lastTimeUpdatedTooltip = -1f;
		}

		// Token: 0x04000091 RID: 145
		private static List<string> allTipsCached;

		// Token: 0x04000092 RID: 146
		private static float lastTimeUpdatedTooltip = -1f;

		// Token: 0x04000093 RID: 147
		private static int currentTipIndex = 0;

		// Token: 0x04000094 RID: 148
		public const float tipUpdateInterval = 17.5f;

		// Token: 0x04000095 RID: 149
		public static readonly Vector2 WindowSize = new Vector2(776f, 60f);

		// Token: 0x04000096 RID: 150
		private static readonly Vector2 TextMargin = new Vector2(15f, 8f);
	}
}
