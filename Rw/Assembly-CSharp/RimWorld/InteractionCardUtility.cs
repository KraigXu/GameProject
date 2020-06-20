using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E71 RID: 3697
	public static class InteractionCardUtility
	{
		// Token: 0x06005999 RID: 22937 RVA: 0x001E378C File Offset: 0x001E198C
		public static void DrawInteractionsLog(Rect rect, Pawn pawn, List<LogEntry> entries, int maxEntries)
		{
			float width = rect.width - 29f - 16f - 10f;
			InteractionCardUtility.logStrings.Clear();
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].Concerns(pawn))
				{
					TaggedString taggedString = entries[i].ToGameStringFromPOV(pawn, false);
					InteractionCardUtility.logStrings.Add(new Pair<string, int>(taggedString, i));
					num += Mathf.Max(26f, Text.CalcHeight(taggedString, width));
					num2++;
					if (num2 >= maxEntries)
					{
						break;
					}
				}
			}
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, num);
			Widgets.BeginScrollView(rect, ref InteractionCardUtility.logScrollPosition, viewRect, true);
			float num3 = 0f;
			for (int j = 0; j < InteractionCardUtility.logStrings.Count; j++)
			{
				TaggedString taggedString2 = InteractionCardUtility.logStrings[j].First;
				LogEntry entry = entries[InteractionCardUtility.logStrings[j].Second];
				if (entry.Age > 7500)
				{
					GUI.color = new Color(1f, 1f, 1f, 0.5f);
				}
				float num4 = Mathf.Max(26f, Text.CalcHeight(taggedString2, width));
				Texture2D texture2D = entry.IconFromPOV(pawn);
				if (texture2D != null)
				{
					GUI.DrawTexture(new Rect(0f, num3, 26f, 26f), texture2D);
				}
				Rect rect2 = new Rect(29f, num3, width, num4);
				if (Mouse.IsOver(rect2))
				{
					TooltipHandler.TipRegion(rect2, () => entry.GetTipString(), 613261 + j * 611);
					Widgets.DrawHighlight(rect2);
				}
				Widgets.Label(rect2, taggedString2);
				if (Widgets.ButtonInvisible(rect2, entry.CanBeClickedFromPOV(pawn)))
				{
					entry.ClickedFromPOV(pawn);
				}
				GUI.color = Color.white;
				num3 += num4;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x040030A1 RID: 12449
		private static Vector2 logScrollPosition = Vector2.zero;

		// Token: 0x040030A2 RID: 12450
		public const float ImageSize = 26f;

		// Token: 0x040030A3 RID: 12451
		public const float ImagePadRight = 3f;

		// Token: 0x040030A4 RID: 12452
		public const float TextOffset = 29f;

		// Token: 0x040030A5 RID: 12453
		private static List<Pair<string, int>> logStrings = new List<Pair<string, int>>();
	}
}
