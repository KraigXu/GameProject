using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	
	public class GameplayTipWindow
	{
		
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

		
		public static void ResetTipTimer()
		{
			GameplayTipWindow.lastTimeUpdatedTooltip = -1f;
		}

		
		private static List<string> allTipsCached;

		
		private static float lastTimeUpdatedTooltip = -1f;

		
		private static int currentTipIndex = 0;

		
		public const float tipUpdateInterval = 17.5f;

		
		public static readonly Vector2 WindowSize = new Vector2(776f, 60f);

		
		private static readonly Vector2 TextMargin = new Vector2(15f, 8f);
	}
}
