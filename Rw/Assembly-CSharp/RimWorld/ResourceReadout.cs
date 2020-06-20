﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF9 RID: 3833
	public class ResourceReadout
	{
		// Token: 0x06005E05 RID: 24069 RVA: 0x00207A34 File Offset: 0x00205C34
		public ResourceReadout()
		{
			this.RootThingCategories = (from cat in DefDatabase<ThingCategoryDef>.AllDefs
			where cat.resourceReadoutRoot
			select cat).ToList<ThingCategoryDef>();
		}

		// Token: 0x06005E06 RID: 24070 RVA: 0x00207A70 File Offset: 0x00205C70
		public void ResourceReadoutOnGUI()
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (Find.MainTabsRoot.OpenTab == MainButtonDefOf.Menu)
			{
				return;
			}
			GenUI.DrawTextWinterShadow(new Rect(256f, 512f, -256f, -512f));
			Text.Font = GameFont.Small;
			Rect rect = Prefs.ResourceReadoutCategorized ? new Rect(2f, 7f, 124f, (float)(UI.screenHeight - 7) - 200f) : new Rect(7f, 7f, 110f, (float)(UI.screenHeight - 7) - 200f);
			Rect rect2 = new Rect(0f, 0f, rect.width, this.lastDrawnHeight);
			bool flag = rect2.height > rect.height;
			if (flag)
			{
				Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, false);
			}
			else
			{
				this.scrollPosition = Vector2.zero;
				GUI.BeginGroup(rect);
			}
			if (!Prefs.ResourceReadoutCategorized)
			{
				this.DoReadoutSimple(rect2, rect.height);
			}
			else
			{
				this.DoReadoutCategorized(rect2);
			}
			if (flag)
			{
				Widgets.EndScrollView();
				return;
			}
			GUI.EndGroup();
		}

		// Token: 0x06005E07 RID: 24071 RVA: 0x00207B98 File Offset: 0x00205D98
		private void DoReadoutCategorized(Rect rect)
		{
			Listing_ResourceReadout listing_ResourceReadout = new Listing_ResourceReadout(Find.CurrentMap);
			listing_ResourceReadout.Begin(rect);
			listing_ResourceReadout.nestIndentWidth = 7f;
			listing_ResourceReadout.lineHeight = 24f;
			listing_ResourceReadout.verticalSpacing = 0f;
			for (int i = 0; i < this.RootThingCategories.Count; i++)
			{
				listing_ResourceReadout.DoCategory(this.RootThingCategories[i].treeNode, 0, 32);
			}
			listing_ResourceReadout.End();
			this.lastDrawnHeight = listing_ResourceReadout.CurHeight;
		}

		// Token: 0x06005E08 RID: 24072 RVA: 0x00207C1C File Offset: 0x00205E1C
		private void DoReadoutSimple(Rect rect, float outRectHeight)
		{
			GUI.BeginGroup(rect);
			Text.Anchor = TextAnchor.MiddleLeft;
			float num = 0f;
			foreach (KeyValuePair<ThingDef, int> keyValuePair in Find.CurrentMap.resourceCounter.AllCountedAmounts)
			{
				if (keyValuePair.Value > 0 || keyValuePair.Key.resourceReadoutAlwaysShow)
				{
					Rect rect2 = new Rect(0f, num, 999f, 24f);
					if (rect2.yMax >= this.scrollPosition.y && rect2.y <= this.scrollPosition.y + outRectHeight)
					{
						this.DrawResourceSimple(rect2, keyValuePair.Key);
					}
					num += 24f;
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			this.lastDrawnHeight = num;
			GUI.EndGroup();
		}

		// Token: 0x06005E09 RID: 24073 RVA: 0x00207D04 File Offset: 0x00205F04
		public void DrawResourceSimple(Rect rect, ThingDef thingDef)
		{
			this.DrawIcon(rect.x, rect.y, thingDef);
			rect.y += 2f;
			int count = Find.CurrentMap.resourceCounter.GetCount(thingDef);
			Widgets.Label(new Rect(34f, rect.y, rect.width - 34f, rect.height), count.ToStringCached());
		}

		// Token: 0x06005E0A RID: 24074 RVA: 0x00207D7C File Offset: 0x00205F7C
		private void DrawIcon(float x, float y, ThingDef thingDef)
		{
			Rect rect = new Rect(x, y, 27f, 27f);
			Color color = GUI.color;
			Widgets.ThingIcon(rect, thingDef, null, 1f);
			GUI.color = color;
			if (Mouse.IsOver(rect))
			{
				TaggedString str = thingDef.LabelCap + ": " + thingDef.description.CapitalizeFirst();
				TooltipHandler.TipRegion(rect, str);
			}
		}

		// Token: 0x040032EF RID: 13039
		private Vector2 scrollPosition;

		// Token: 0x040032F0 RID: 13040
		private float lastDrawnHeight;

		// Token: 0x040032F1 RID: 13041
		private readonly List<ThingCategoryDef> RootThingCategories;

		// Token: 0x040032F2 RID: 13042
		private const float LineHeightSimple = 24f;

		// Token: 0x040032F3 RID: 13043
		private const float LineHeightCategorized = 24f;

		// Token: 0x040032F4 RID: 13044
		private const float DistFromScreenBottom = 200f;
	}
}
