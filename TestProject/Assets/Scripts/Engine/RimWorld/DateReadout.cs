using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E94 RID: 3732
	public static class DateReadout
	{
		// Token: 0x17001052 RID: 4178
		// (get) Token: 0x06005AFD RID: 23293 RVA: 0x001F47EF File Offset: 0x001F29EF
		public static float Height
		{
			get
			{
				return (float)(48 + (DateReadout.SeasonLabelVisible ? 26 : 0));
			}
		}

		// Token: 0x17001053 RID: 4179
		// (get) Token: 0x06005AFE RID: 23294 RVA: 0x001F4801 File Offset: 0x001F2A01
		private static bool SeasonLabelVisible
		{
			get
			{
				return !WorldRendererUtility.WorldRenderedNow && Find.CurrentMap != null;
			}
		}

		// Token: 0x06005AFF RID: 23295 RVA: 0x001F4814 File Offset: 0x001F2A14
		static DateReadout()
		{
			DateReadout.Reset();
		}

		// Token: 0x06005B00 RID: 23296 RVA: 0x001F4848 File Offset: 0x001F2A48
		public static void Reset()
		{
			DateReadout.dateString = null;
			DateReadout.dateStringDay = -1;
			DateReadout.dateStringSeason = Season.Undefined;
			DateReadout.dateStringQuadrum = Quadrum.Undefined;
			DateReadout.dateStringYear = -1;
			DateReadout.fastHourStrings.Clear();
			for (int i = 0; i < 24; i++)
			{
				DateReadout.fastHourStrings.Add(i + "LetterHour".Translate());
			}
			DateReadout.seasonsCached.Clear();
			int length = Enum.GetValues(typeof(Season)).Length;
			for (int j = 0; j < length; j++)
			{
				Season season = (Season)j;
				DateReadout.seasonsCached.Add((season == Season.Undefined) ? "" : season.LabelCap());
			}
		}

		// Token: 0x06005B01 RID: 23297 RVA: 0x001F48F8 File Offset: 0x001F2AF8
		public static void DateOnGUI(Rect dateRect)
		{
			Vector2 vector;
			if (WorldRendererUtility.WorldRenderedNow && Find.WorldSelector.selectedTile >= 0)
			{
				vector = Find.WorldGrid.LongLatOf(Find.WorldSelector.selectedTile);
			}
			else if (WorldRendererUtility.WorldRenderedNow && Find.WorldSelector.NumSelectedObjects > 0)
			{
				vector = Find.WorldGrid.LongLatOf(Find.WorldSelector.FirstSelectedObject.Tile);
			}
			else
			{
				if (Find.CurrentMap == null)
				{
					return;
				}
				vector = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
			}
			int index = GenDate.HourInteger((long)Find.TickManager.TicksAbs, vector.x);
			int num = GenDate.DayOfTwelfth((long)Find.TickManager.TicksAbs, vector.x);
			Season season = GenDate.Season((long)Find.TickManager.TicksAbs, vector);
			Quadrum quadrum = GenDate.Quadrum((long)Find.TickManager.TicksAbs, vector.x);
			int num2 = GenDate.Year((long)Find.TickManager.TicksAbs, vector.x);
			string text = DateReadout.SeasonLabelVisible ? DateReadout.seasonsCached[(int)season] : "";
			if (num != DateReadout.dateStringDay || season != DateReadout.dateStringSeason || quadrum != DateReadout.dateStringQuadrum || num2 != DateReadout.dateStringYear)
			{
				DateReadout.dateString = GenDate.DateReadoutStringAt((long)Find.TickManager.TicksAbs, vector);
				DateReadout.dateStringDay = num;
				DateReadout.dateStringSeason = season;
				DateReadout.dateStringQuadrum = quadrum;
				DateReadout.dateStringYear = num2;
			}
			Text.Font = GameFont.Small;
			float num3 = Mathf.Max(Mathf.Max(Text.CalcSize(DateReadout.fastHourStrings[index]).x, Text.CalcSize(DateReadout.dateString).x), Text.CalcSize(text).x) + 7f;
			dateRect.xMin = dateRect.xMax - num3;
			if (Mouse.IsOver(dateRect))
			{
				Widgets.DrawHighlight(dateRect);
			}
			GUI.BeginGroup(dateRect);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperRight;
			Rect rect = dateRect.AtZero();
			rect.xMax -= 7f;
			Widgets.Label(rect, DateReadout.fastHourStrings[index]);
			rect.yMin += 26f;
			Widgets.Label(rect, DateReadout.dateString);
			rect.yMin += 26f;
			if (!text.NullOrEmpty())
			{
				Widgets.Label(rect, text);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
			if (Mouse.IsOver(dateRect))
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < 4; i++)
				{
					Quadrum quadrum2 = (Quadrum)i;
					stringBuilder.AppendLine(quadrum2.Label() + " - " + quadrum2.GetSeason(vector.y).LabelCap());
				}
				TaggedString taggedString = "DateReadoutTip".Translate(GenDate.DaysPassed, 15, season.LabelCap(), 15, GenDate.Quadrum((long)GenTicks.TicksAbs, vector.x).Label(), stringBuilder.ToString());
				TooltipHandler.TipRegion(dateRect, new TipSignal(taggedString, 86423));
			}
		}

		// Token: 0x0400319F RID: 12703
		private static string dateString;

		// Token: 0x040031A0 RID: 12704
		private static int dateStringDay = -1;

		// Token: 0x040031A1 RID: 12705
		private static Season dateStringSeason = Season.Undefined;

		// Token: 0x040031A2 RID: 12706
		private static Quadrum dateStringQuadrum = Quadrum.Undefined;

		// Token: 0x040031A3 RID: 12707
		private static int dateStringYear = -1;

		// Token: 0x040031A4 RID: 12708
		private static readonly List<string> fastHourStrings = new List<string>();

		// Token: 0x040031A5 RID: 12709
		private static readonly List<string> seasonsCached = new List<string>();

		// Token: 0x040031A6 RID: 12710
		private const float DateRightPadding = 7f;
	}
}
