using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public static class BeautyDrawer
	{
		
		public static void BeautyDrawerOnGUI()
		{
			if (Event.current.type != EventType.Repaint || !BeautyDrawer.ShouldShow())
			{
				return;
			}
			BeautyDrawer.DrawBeautyAroundMouse();
		}

		
		private static bool ShouldShow()
		{
			return Find.PlaySettings.showBeauty && !Mouse.IsInputBlockedNow && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap);
		}

		
		private static void DrawBeautyAroundMouse()
		{
			if (!Find.PlaySettings.showBeauty)
			{
				return;
			}
			BeautyUtility.FillBeautyRelevantCells(UI.MouseCell(), Find.CurrentMap);
			for (int i = 0; i < BeautyUtility.beautyRelevantCells.Count; i++)
			{
				IntVec3 intVec = BeautyUtility.beautyRelevantCells[i];
				float num = BeautyUtility.CellBeauty(intVec, Find.CurrentMap, BeautyDrawer.beautyCountedThings);
				if (num != 0f)
				{
					GenMapUI.DrawThingLabel(GenMapUI.LabelDrawPosFor(intVec), Mathf.RoundToInt(num).ToStringCached(), BeautyDrawer.BeautyColor(num, 8f));
				}
			}
			BeautyDrawer.beautyCountedThings.Clear();
		}

		
		public static Color BeautyColor(float beauty, float scale)
		{
			float num = Mathf.InverseLerp(-scale, scale, beauty);
			num = Mathf.Clamp01(num);
			return Color.Lerp(Color.Lerp(BeautyDrawer.ColorUgly, BeautyDrawer.ColorBeautiful, num), Color.white, 0.5f);
		}

		
		private static List<Thing> beautyCountedThings = new List<Thing>();

		
		private static Color ColorUgly = Color.red;

		
		private static Color ColorBeautiful = Color.green;
	}
}
