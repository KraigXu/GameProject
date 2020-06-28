﻿using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EFD RID: 3837
	public static class TimeAssignmentSelector
	{
		// Token: 0x06005E15 RID: 24085 RVA: 0x00208774 File Offset: 0x00206974
		public static void DrawTimeAssignmentSelectorGrid(Rect rect)
		{
			rect.yMax -= 2f;
			Rect rect2 = rect;
			rect2.xMax = rect2.center.x;
			rect2.yMax = rect2.center.y;
			TimeAssignmentSelector.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Anything);
			rect2.x += rect2.width;
			TimeAssignmentSelector.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Work);
			rect2.y += rect2.height;
			rect2.x -= rect2.width;
			TimeAssignmentSelector.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Joy);
			rect2.x += rect2.width;
			TimeAssignmentSelector.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Sleep);
			if (ModsConfig.RoyaltyActive)
			{
				rect2.x += rect2.width;
				rect2.y -= rect2.height;
				TimeAssignmentSelector.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Meditate);
			}
		}

		// Token: 0x06005E16 RID: 24086 RVA: 0x00208878 File Offset: 0x00206A78
		private static void DrawTimeAssignmentSelectorFor(Rect rect, TimeAssignmentDef ta)
		{
			rect = rect.ContractedBy(2f);
			GUI.DrawTexture(rect, ta.ColorTexture);
			if (Widgets.ButtonInvisible(rect, true))
			{
				TimeAssignmentSelector.selectedAssignment = ta;
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			GUI.color = Color.white;
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = Color.white;
			Widgets.Label(rect, ta.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			if (TimeAssignmentSelector.selectedAssignment == ta)
			{
				Widgets.DrawBox(rect, 2);
				return;
			}
			UIHighlighter.HighlightOpportunity(rect, ta.cachedHighlightNotSelectedTag);
		}

		// Token: 0x04003316 RID: 13078
		public static TimeAssignmentDef selectedAssignment = TimeAssignmentDefOf.Work;
	}
}
