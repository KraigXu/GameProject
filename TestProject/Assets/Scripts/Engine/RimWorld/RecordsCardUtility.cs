using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E74 RID: 3700
	public static class RecordsCardUtility
	{
		// Token: 0x060059A6 RID: 22950 RVA: 0x001E4370 File Offset: 0x001E2570
		public static void DrawRecordsCard(Rect rect, Pawn pawn)
		{
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, RecordsCardUtility.listHeight);
			Widgets.BeginScrollView(rect, ref RecordsCardUtility.scrollPosition, rect2, true);
			Rect leftRect = rect2;
			leftRect.width *= 0.5f;
			Rect rightRect = rect2;
			rightRect.x = leftRect.xMax;
			rightRect.width = rect2.width - rightRect.x;
			leftRect.xMax -= 6f;
			rightRect.xMin += 6f;
			float a = RecordsCardUtility.DrawTimeRecords(leftRect, pawn);
			float b = RecordsCardUtility.DrawMiscRecords(rightRect, pawn);
			RecordsCardUtility.listHeight = Mathf.Max(a, b) + 100f;
			Widgets.EndScrollView();
		}

		// Token: 0x060059A7 RID: 22951 RVA: 0x001E443C File Offset: 0x001E263C
		private static float DrawTimeRecords(Rect leftRect, Pawn pawn)
		{
			List<RecordDef> allDefsListForReading = DefDatabase<RecordDef>.AllDefsListForReading;
			float num = 0f;
			GUI.BeginGroup(leftRect);
			Widgets.ListSeparator(ref num, leftRect.width, "TimeRecordsCategory".Translate());
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].type == RecordType.Time)
				{
					num += RecordsCardUtility.DrawRecord(8f, num, leftRect.width - 8f, allDefsListForReading[i], pawn);
				}
			}
			GUI.EndGroup();
			return num;
		}

		// Token: 0x060059A8 RID: 22952 RVA: 0x001E44C0 File Offset: 0x001E26C0
		private static float DrawMiscRecords(Rect rightRect, Pawn pawn)
		{
			List<RecordDef> allDefsListForReading = DefDatabase<RecordDef>.AllDefsListForReading;
			float num = 0f;
			GUI.BeginGroup(rightRect);
			Widgets.ListSeparator(ref num, rightRect.width, "MiscRecordsCategory".Translate());
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].type == RecordType.Int || allDefsListForReading[i].type == RecordType.Float)
				{
					num += RecordsCardUtility.DrawRecord(8f, num, rightRect.width - 8f, allDefsListForReading[i], pawn);
				}
			}
			GUI.EndGroup();
			return num;
		}

		// Token: 0x060059A9 RID: 22953 RVA: 0x001E4554 File Offset: 0x001E2754
		private static float DrawRecord(float x, float y, float width, RecordDef record, Pawn pawn)
		{
			float num = width * 0.45f;
			string text;
			if (record.type == RecordType.Time)
			{
				text = pawn.records.GetAsInt(record).ToStringTicksToPeriod(true, false, true, true);
			}
			else
			{
				text = pawn.records.GetValue(record).ToString("0.##");
			}
			Rect rect = new Rect(8f, y, width, Text.CalcHeight(text, num));
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			Rect rect2 = rect;
			rect2.width -= num;
			Widgets.Label(rect2, record.LabelCap);
			Rect rect3 = rect;
			rect3.x = rect2.xMax;
			rect3.width = num;
			Widgets.Label(rect3, text);
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, new TipSignal(() => record.description, record.GetHashCode()));
			}
			return rect.height;
		}

		// Token: 0x040030B3 RID: 12467
		private static Vector2 scrollPosition;

		// Token: 0x040030B4 RID: 12468
		private static float listHeight;

		// Token: 0x040030B5 RID: 12469
		private const float RecordsLeftPadding = 8f;
	}
}
