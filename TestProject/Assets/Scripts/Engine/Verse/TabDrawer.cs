using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003CF RID: 975
	public static class TabDrawer
	{
		// Token: 0x06001CA7 RID: 7335 RVA: 0x000AE7AC File Offset: 0x000AC9AC
		public static TabRecord DrawTabs(Rect baseRect, List<TabRecord> tabs, int rows)
		{
			if (rows <= 1)
			{
				return TabDrawer.DrawTabs(baseRect, tabs, 200f);
			}
			int num = Mathf.FloorToInt((float)(tabs.Count / rows));
			int num2 = 0;
			TabRecord result = null;
			Rect rect = baseRect;
			baseRect.yMin -= (float)(rows - 1) * 31f;
			Rect rect2 = baseRect;
			rect2.yMax = rect.y;
			Widgets.DrawMenuSection(rect2);
			for (int i = 0; i < rows; i++)
			{
				int num3 = (i == 0) ? (tabs.Count - (rows - 1) * num) : num;
				TabDrawer.tmpTabs.Clear();
				for (int j = num2; j < num2 + num3; j++)
				{
					TabDrawer.tmpTabs.Add(tabs[j]);
				}
				TabRecord tabRecord = TabDrawer.DrawTabs(baseRect, TabDrawer.tmpTabs, baseRect.width);
				if (tabRecord != null)
				{
					result = tabRecord;
				}
				baseRect.yMin += 31f;
				num2 += num3;
			}
			TabDrawer.tmpTabs.Clear();
			return result;
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x000AE8A4 File Offset: 0x000ACAA4
		public static TabRecord DrawTabs(Rect baseRect, List<TabRecord> tabs, float maxTabWidth = 200f)
		{
			TabRecord tabRecord = null;
			TabRecord tabRecord2 = tabs.Find((TabRecord t) => t.Selected);
			float num = baseRect.width + (float)(tabs.Count - 1) * 10f;
			float tabWidth = num / (float)tabs.Count;
			if (tabWidth > maxTabWidth)
			{
				tabWidth = maxTabWidth;
			}
			Rect position = new Rect(baseRect);
			position.y -= 32f;
			position.height = 9999f;
			GUI.BeginGroup(position);
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Small;
			Func<TabRecord, Rect> func = (TabRecord tab) => new Rect((float)tabs.IndexOf(tab) * (tabWidth - 10f), 1f, tabWidth, 32f);
			List<TabRecord> list = tabs.ListFullCopy<TabRecord>();
			if (tabRecord2 != null)
			{
				list.Remove(tabRecord2);
				list.Add(tabRecord2);
			}
			TabRecord tabRecord3 = null;
			List<TabRecord> list2 = list.ListFullCopy<TabRecord>();
			list2.Reverse();
			for (int i = 0; i < list2.Count; i++)
			{
				TabRecord tabRecord4 = list2[i];
				Rect rect = func(tabRecord4);
				if (tabRecord3 == null && Mouse.IsOver(rect))
				{
					tabRecord3 = tabRecord4;
				}
				MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Tab);
				if (Widgets.ButtonInvisible(rect, true))
				{
					tabRecord = tabRecord4;
				}
			}
			foreach (TabRecord tabRecord5 in list)
			{
				Rect rect2 = func(tabRecord5);
				tabRecord5.Draw(rect2);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
			if (tabRecord != null && tabRecord != tabRecord2)
			{
				SoundDefOf.RowTabSelect.PlayOneShotOnCamera(null);
				if (tabRecord.clickedAction != null)
				{
					tabRecord.clickedAction();
				}
			}
			return tabRecord;
		}

		// Token: 0x040010F5 RID: 4341
		private const float MaxTabWidth = 200f;

		// Token: 0x040010F6 RID: 4342
		public const float TabHeight = 32f;

		// Token: 0x040010F7 RID: 4343
		public const float TabHoriztonalOverlap = 10f;

		// Token: 0x040010F8 RID: 4344
		private static List<TabRecord> tmpTabs = new List<TabRecord>();
	}
}
