using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000E0E RID: 3598
	public static class AreaAllowedGUI
	{
		// Token: 0x060056E3 RID: 22243 RVA: 0x001CD598 File Offset: 0x001CB798
		public static void DoAllowedAreaSelectors(Rect rect, Pawn p)
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			List<Area> allAreas = Find.CurrentMap.areaManager.AllAreas;
			int num = 1;
			for (int i = 0; i < allAreas.Count; i++)
			{
				if (allAreas[i].AssignableAsAllowed())
				{
					num++;
				}
			}
			float num2 = rect.width / (float)num;
			Text.WordWrap = false;
			Text.Font = GameFont.Tiny;
			AreaAllowedGUI.DoAreaSelector(new Rect(rect.x + 0f, rect.y, num2, rect.height), p, null);
			int num3 = 1;
			for (int j = 0; j < allAreas.Count; j++)
			{
				if (allAreas[j].AssignableAsAllowed())
				{
					float num4 = (float)num3 * num2;
					AreaAllowedGUI.DoAreaSelector(new Rect(rect.x + num4, rect.y, num2, rect.height), p, allAreas[j]);
					num3++;
				}
			}
			Text.WordWrap = true;
			Text.Font = GameFont.Small;
		}

		// Token: 0x060056E4 RID: 22244 RVA: 0x001CD690 File Offset: 0x001CB890
		private static void DoAreaSelector(Rect rect, Pawn p, Area area)
		{
			MouseoverSounds.DoRegion(rect);
			rect = rect.ContractedBy(1f);
			GUI.DrawTexture(rect, (area != null) ? area.ColorTexture : BaseContent.GreyTex);
			Text.Anchor = TextAnchor.MiddleLeft;
			string text = AreaUtility.AreaAllowedLabel_Area(area);
			Rect rect2 = rect;
			rect2.xMin += 3f;
			rect2.yMin += 2f;
			Widgets.Label(rect2, text);
			if (p.playerSettings.AreaRestriction == area)
			{
				Widgets.DrawBox(rect, 2);
			}
			if (Event.current.rawType == EventType.MouseUp && Event.current.button == 0)
			{
				AreaAllowedGUI.dragging = false;
			}
			if (!Input.GetMouseButton(0) && Event.current.type != EventType.MouseDown)
			{
				AreaAllowedGUI.dragging = false;
			}
			if (Mouse.IsOver(rect))
			{
				if (area != null)
				{
					area.MarkForDraw();
				}
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
				{
					AreaAllowedGUI.dragging = true;
				}
				if (AreaAllowedGUI.dragging && p.playerSettings.AreaRestriction != area)
				{
					p.playerSettings.AreaRestriction = area;
					SoundDefOf.Designate_DragStandard_Changed.PlayOneShotOnCamera(null);
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			TooltipHandler.TipRegion(rect, text);
		}

		// Token: 0x04002F59 RID: 12121
		private static bool dragging;
	}
}
