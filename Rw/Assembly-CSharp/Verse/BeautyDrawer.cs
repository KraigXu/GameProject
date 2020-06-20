using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000385 RID: 901
	public static class BeautyDrawer
	{
		// Token: 0x06001AAA RID: 6826 RVA: 0x000A3FC2 File Offset: 0x000A21C2
		public static void BeautyDrawerOnGUI()
		{
			if (Event.current.type != EventType.Repaint || !BeautyDrawer.ShouldShow())
			{
				return;
			}
			BeautyDrawer.DrawBeautyAroundMouse();
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x000A3FDE File Offset: 0x000A21DE
		private static bool ShouldShow()
		{
			return Find.PlaySettings.showBeauty && !Mouse.IsInputBlockedNow && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap);
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x000A401C File Offset: 0x000A221C
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

		// Token: 0x06001AAD RID: 6829 RVA: 0x000A40B4 File Offset: 0x000A22B4
		public static Color BeautyColor(float beauty, float scale)
		{
			float num = Mathf.InverseLerp(-scale, scale, beauty);
			num = Mathf.Clamp01(num);
			return Color.Lerp(Color.Lerp(BeautyDrawer.ColorUgly, BeautyDrawer.ColorBeautiful, num), Color.white, 0.5f);
		}

		// Token: 0x04000FBE RID: 4030
		private static List<Thing> beautyCountedThings = new List<Thing>();

		// Token: 0x04000FBF RID: 4031
		private static Color ColorUgly = Color.red;

		// Token: 0x04000FC0 RID: 4032
		private static Color ColorBeautiful = Color.green;
	}
}
