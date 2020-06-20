using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E6E RID: 3694
	public static class FactionUIUtility
	{
		// Token: 0x0600597B RID: 22907 RVA: 0x001E08F0 File Offset: 0x001DEAF0
		public static void DoWindowContents(Rect fillRect, ref Vector2 scrollPosition, ref float scrollViewHeight)
		{
			Rect position = new Rect(0f, 0f, fillRect.width, fillRect.height);
			GUI.BeginGroup(position);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			if (Prefs.DevMode)
			{
				Widgets.CheckboxLabeled(new Rect(position.width - 120f, 0f, 120f, 24f), "Dev: Show all", ref FactionUIUtility.showAll, false, null, null, false);
			}
			else
			{
				FactionUIUtility.showAll = false;
			}
			Rect outRect = new Rect(0f, 50f, position.width, position.height - 50f);
			Rect rect = new Rect(0f, 0f, position.width - 16f, scrollViewHeight);
			Widgets.BeginScrollView(outRect, ref scrollPosition, rect, true);
			float num = 0f;
			foreach (Faction faction in Find.FactionManager.AllFactionsInViewOrder)
			{
				if ((!faction.IsPlayer && !faction.def.hidden) || FactionUIUtility.showAll)
				{
					GUI.color = new Color(1f, 1f, 1f, 0.2f);
					Widgets.DrawLineHorizontal(0f, num, rect.width);
					GUI.color = Color.white;
					num += FactionUIUtility.DrawFactionRow(faction, num, rect);
				}
			}
			if (Event.current.type == EventType.Layout)
			{
				scrollViewHeight = num;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
		}

		// Token: 0x0600597C RID: 22908 RVA: 0x001E0A84 File Offset: 0x001DEC84
		private static float DrawFactionRow(Faction faction, float rowY, Rect fillRect)
		{
			float num = fillRect.width - 250f - 40f - 90f - 16f - 120f;
			Faction[] array = (from f in Find.FactionManager.AllFactionsInViewOrder
			where f != faction && f.HostileTo(faction) && ((!f.IsPlayer && !f.def.hidden) || FactionUIUtility.showAll)
			select f).ToArray<Faction>();
			Rect rect = new Rect(90f, rowY, 250f, 80f);
			Rect r = new Rect(24f, rowY + 4f, 42f, 42f);
			float num2 = 62f;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			FactionUIUtility.DrawFactionIconWithTooltip(r, faction);
			string label = faction.Name.CapitalizeFirst() + "\n" + faction.def.LabelCap + "\n" + ((faction.leader != null) ? (faction.LeaderTitle.CapitalizeFirst() + ": " + faction.leader.Name.ToStringFull) : "");
			Widgets.Label(rect, label);
			Rect rect2 = new Rect(rect.xMax, rowY, 40f, 80f);
			Widgets.InfoCardButton(rect2.x, rect2.y, faction);
			Rect rect3 = new Rect(rect2.xMax, rowY, 90f, 80f);
			if (!faction.IsPlayer)
			{
				string text = faction.PlayerGoodwill.ToStringWithSign();
				text = text + "\n" + faction.PlayerRelationKind.GetLabel();
				if (faction.defeated)
				{
					text += "\n(" + "DefeatedLower".Translate() + ")";
				}
				GUI.color = faction.PlayerRelationKind.GetColor();
				Widgets.Label(rect3, text);
				GUI.color = Color.white;
				if (Mouse.IsOver(rect3))
				{
					TaggedString taggedString = "CurrentGoodwillTip".Translate();
					if (faction.def.permanentEnemy)
					{
						taggedString += "\n\n" + "CurrentGoodwillTip_PermanentEnemy".Translate();
					}
					else
					{
						taggedString += "\n\n";
						switch (faction.PlayerRelationKind)
						{
						case FactionRelationKind.Hostile:
							taggedString += "CurrentGoodwillTip_Hostile".Translate(0.ToString("F0"));
							break;
						case FactionRelationKind.Neutral:
							taggedString += "CurrentGoodwillTip_Neutral".Translate(-75.ToString("F0"), 75.ToString("F0"));
							break;
						case FactionRelationKind.Ally:
							taggedString += "CurrentGoodwillTip_Ally".Translate(0.ToString("F0"));
							break;
						}
						if (faction.def.goodwillDailyGain > 0f || faction.def.goodwillDailyFall > 0f)
						{
							float num3 = faction.def.goodwillDailyGain * 60f;
							float num4 = faction.def.goodwillDailyFall * 60f;
							taggedString += "\n\n" + "CurrentGoodwillTip_NaturalGoodwill".Translate(faction.def.naturalColonyGoodwill.min.ToString("F0"), faction.def.naturalColonyGoodwill.max.ToString("F0"));
							if (faction.def.naturalColonyGoodwill.min > -100)
							{
								taggedString += " " + "CurrentGoodwillTip_NaturalGoodwillRise".Translate(faction.def.naturalColonyGoodwill.min.ToString("F0"), num3.ToString("F0"));
							}
							if (faction.def.naturalColonyGoodwill.max < 100)
							{
								taggedString += " " + "CurrentGoodwillTip_NaturalGoodwillFall".Translate(faction.def.naturalColonyGoodwill.max.ToString("F0"), num4.ToString("F0"));
							}
						}
					}
					TooltipHandler.TipRegion(rect3, taggedString);
				}
				if (Mouse.IsOver(rect3))
				{
					GUI.DrawTexture(rect3, TexUI.HighlightTex);
				}
			}
			float num5 = rect3.xMax;
			string text2 = "EnemyOf".Translate();
			Vector2 vector = Text.CalcSize(text2);
			Rect rect4 = new Rect(num5, rowY + 4f, vector.x + 10f, 42f);
			num5 += rect4.width;
			Widgets.Label(rect4, text2);
			for (int i = 0; i < array.Length; i++)
			{
				if (num5 >= rect3.xMax + num)
				{
					num5 = rect3.xMax + rect4.width;
					rowY += vector.y + 5f;
					num2 += vector.y + 5f;
				}
				FactionUIUtility.DrawFactionIconWithTooltip(new Rect(num5, rowY + 4f, vector.y, vector.y), array[i]);
				num5 += vector.y + 5f;
			}
			return Mathf.Max(80f, num2);
		}

		// Token: 0x0600597D RID: 22909 RVA: 0x001E1074 File Offset: 0x001DF274
		public static void DrawFactionIconWithTooltip(Rect r, Faction faction)
		{
			GUI.color = faction.Color;
			GUI.DrawTexture(r, faction.def.FactionIcon);
			GUI.color = Color.white;
			if (Mouse.IsOver(r))
			{
				TipSignal tip = new TipSignal(() => faction.Name + "\n\n" + faction.def.description, faction.loadID ^ 1938473043);
				TooltipHandler.TipRegion(r, tip);
				Widgets.DrawHighlight(r);
			}
		}

		// Token: 0x0600597E RID: 22910 RVA: 0x001E10F8 File Offset: 0x001DF2F8
		public static void DrawRelatedFactionInfo(Rect rect, Faction faction, ref float curY)
		{
			Text.Anchor = TextAnchor.LowerRight;
			curY += 10f;
			FactionRelationKind playerRelationKind = faction.PlayerRelationKind;
			string text = faction.Name.CapitalizeFirst() + "\n" + "goodwill".Translate().CapitalizeFirst() + ": " + faction.PlayerGoodwill.ToStringWithSign();
			GUI.color = Color.gray;
			Rect rect2 = new Rect(rect.x, curY, rect.width, Text.CalcHeight(text, rect.width));
			Widgets.Label(rect2, text);
			curY += rect2.height;
			GUI.color = playerRelationKind.GetColor();
			Rect rect3 = new Rect(rect2.x, curY - 7f, rect2.width, 25f);
			Widgets.Label(rect3, playerRelationKind.GetLabel());
			curY += rect3.height;
			GUI.color = Color.white;
			GenUI.ResetLabelAlign();
		}

		// Token: 0x0400307E RID: 12414
		private static bool showAll;

		// Token: 0x0400307F RID: 12415
		private const float FactionIconRectSize = 42f;

		// Token: 0x04003080 RID: 12416
		private const float FactionIconRectGapX = 24f;

		// Token: 0x04003081 RID: 12417
		private const float FactionIconRectGapY = 4f;

		// Token: 0x04003082 RID: 12418
		private const float RowMinHeight = 80f;

		// Token: 0x04003083 RID: 12419
		private const float LabelRowHeight = 50f;

		// Token: 0x04003084 RID: 12420
		private const float NameLeftMargin = 15f;

		// Token: 0x04003085 RID: 12421
		private const float FactionIconSpacing = 5f;
	}
}
