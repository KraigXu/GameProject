using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E95 RID: 3733
	public class GlobalControls
	{
		// Token: 0x06005B02 RID: 23298 RVA: 0x001F4C10 File Offset: 0x001F2E10
		public void GlobalControlsOnGUI()
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			float num = (float)UI.screenWidth - 200f;
			float num2 = (float)UI.screenHeight;
			num2 -= 35f;
			GenUI.DrawTextWinterShadow(new Rect((float)(UI.screenWidth - 270), (float)(UI.screenHeight - 450), 270f, 450f));
			num2 -= 4f;
			GlobalControlsUtility.DoPlaySettings(this.rowVisibility, false, ref num2);
			num2 -= 4f;
			GlobalControlsUtility.DoTimespeedControls(num, 200f, ref num2);
			num2 -= 4f;
			GlobalControlsUtility.DoDate(num, 200f, ref num2);
			Rect rect = new Rect(num - 22f, num2 - 26f, 230f, 26f);
			Find.CurrentMap.weatherManager.DoWeatherGUI(rect);
			num2 -= rect.height;
			Rect rect2 = new Rect(num - 100f, num2 - 26f, 293f, 26f);
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, GlobalControls.TemperatureString());
			Text.Anchor = TextAnchor.UpperLeft;
			num2 -= 26f;
			float num3 = 154f;
			float num4 = Find.CurrentMap.gameConditionManager.TotalHeightAt(num3);
			Rect rect3 = new Rect((float)UI.screenWidth - num3, num2 - num4, num3, num4);
			Find.CurrentMap.gameConditionManager.DoConditionsUI(rect3);
			num2 -= rect3.height;
			if (Prefs.ShowRealtimeClock)
			{
				GlobalControlsUtility.DoRealtimeClock(num, 200f, ref num2);
			}
			TimedForcedExit component = Find.CurrentMap.Parent.GetComponent<TimedForcedExit>();
			if (component != null && component.ForceExitAndRemoveMapCountdownActive)
			{
				Rect rect4 = new Rect(num, num2 - 26f, 193f, 26f);
				Text.Anchor = TextAnchor.MiddleRight;
				GlobalControls.DoCountdownTimer(rect4, component);
				Text.Anchor = TextAnchor.UpperLeft;
				num2 -= 26f;
			}
			num2 -= 10f;
			Find.LetterStack.LettersOnGUI(num2);
		}

		// Token: 0x06005B03 RID: 23299 RVA: 0x001F4DE8 File Offset: 0x001F2FE8
		private static string TemperatureString()
		{
			IntVec3 intVec = UI.MouseCell();
			IntVec3 c = intVec;
			Room room = intVec.GetRoom(Find.CurrentMap, RegionType.Set_All);
			if (room == null)
			{
				for (int i = 0; i < 9; i++)
				{
					IntVec3 intVec2 = intVec + GenAdj.AdjacentCellsAndInside[i];
					if (intVec2.InBounds(Find.CurrentMap))
					{
						Room room2 = intVec2.GetRoom(Find.CurrentMap, RegionType.Set_All);
						if (room2 != null && ((!room2.PsychologicallyOutdoors && !room2.UsesOutdoorTemperature) || (!room2.PsychologicallyOutdoors && (room == null || room.PsychologicallyOutdoors)) || (room2.PsychologicallyOutdoors && room == null)))
						{
							c = intVec2;
							room = room2;
						}
					}
				}
			}
			if (room == null && intVec.InBounds(Find.CurrentMap))
			{
				Building edifice = intVec.GetEdifice(Find.CurrentMap);
				if (edifice != null)
				{
					foreach (IntVec3 intVec3 in edifice.OccupiedRect().ExpandedBy(1).ClipInsideMap(Find.CurrentMap))
					{
						room = intVec3.GetRoom(Find.CurrentMap, RegionType.Set_All);
						if (room != null && !room.PsychologicallyOutdoors)
						{
							c = intVec3;
							break;
						}
					}
				}
			}
			string text;
			if (c.InBounds(Find.CurrentMap) && !c.Fogged(Find.CurrentMap) && room != null && !room.PsychologicallyOutdoors)
			{
				if (room.OpenRoofCount == 0)
				{
					text = "Indoors".Translate();
				}
				else
				{
					if (GlobalControls.indoorsUnroofedStringCachedRoofCount != room.OpenRoofCount)
					{
						GlobalControls.indoorsUnroofedStringCached = "IndoorsUnroofed".Translate() + " (" + room.OpenRoofCount.ToStringCached() + ")";
						GlobalControls.indoorsUnroofedStringCachedRoofCount = room.OpenRoofCount;
					}
					text = GlobalControls.indoorsUnroofedStringCached;
				}
			}
			else
			{
				text = "Outdoors".Translate();
			}
			float num = (room == null || c.Fogged(Find.CurrentMap)) ? Find.CurrentMap.mapTemperature.OutdoorTemp : room.Temperature;
			int num2 = Mathf.RoundToInt(GenTemperature.CelsiusTo(GlobalControls.cachedTemperatureStringForTemperature, Prefs.TemperatureMode));
			int num3 = Mathf.RoundToInt(GenTemperature.CelsiusTo(num, Prefs.TemperatureMode));
			if (GlobalControls.cachedTemperatureStringForLabel != text || num2 != num3)
			{
				GlobalControls.cachedTemperatureStringForLabel = text;
				GlobalControls.cachedTemperatureStringForTemperature = num;
				GlobalControls.cachedTemperatureString = text + " " + num.ToStringTemperature("F0");
			}
			return GlobalControls.cachedTemperatureString;
		}

		// Token: 0x06005B04 RID: 23300 RVA: 0x001F5068 File Offset: 0x001F3268
		private static void DoCountdownTimer(Rect rect, TimedForcedExit timedForcedExit)
		{
			string forceExitAndRemoveMapCountdownTimeLeftString = timedForcedExit.ForceExitAndRemoveMapCountdownTimeLeftString;
			string text = "ForceExitAndRemoveMapCountdown".Translate(forceExitAndRemoveMapCountdownTimeLeftString);
			float x = Text.CalcSize(text).x;
			Rect rect2 = new Rect(rect.xMax - x, rect.y, x, rect.height);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			TooltipHandler.TipRegionByKey(rect2, "ForceExitAndRemoveMapCountdownTip", forceExitAndRemoveMapCountdownTimeLeftString);
			Widgets.Label(rect2, text);
		}

		// Token: 0x040031A7 RID: 12711
		public const float Width = 200f;

		// Token: 0x040031A8 RID: 12712
		private WidgetRow rowVisibility = new WidgetRow();

		// Token: 0x040031A9 RID: 12713
		private static string indoorsUnroofedStringCached;

		// Token: 0x040031AA RID: 12714
		private static int indoorsUnroofedStringCachedRoofCount = -1;

		// Token: 0x040031AB RID: 12715
		private static string cachedTemperatureString;

		// Token: 0x040031AC RID: 12716
		private static string cachedTemperatureStringForLabel;

		// Token: 0x040031AD RID: 12717
		private static float cachedTemperatureStringForTemperature;
	}
}
