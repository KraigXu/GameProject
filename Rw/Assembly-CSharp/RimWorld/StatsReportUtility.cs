using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000E77 RID: 3703
	public static class StatsReportUtility
	{
		// Token: 0x1700101B RID: 4123
		// (get) Token: 0x060059CC RID: 22988 RVA: 0x001E5BC0 File Offset: 0x001E3DC0
		public static int SelectedStatIndex
		{
			get
			{
				if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>() || StatsReportUtility.selectedEntry == null)
				{
					return -1;
				}
				return StatsReportUtility.cachedDrawEntries.IndexOf(StatsReportUtility.selectedEntry);
			}
		}

		// Token: 0x060059CD RID: 22989 RVA: 0x001E5BE6 File Offset: 0x001E3DE6
		public static void Reset()
		{
			StatsReportUtility.scrollPosition = default(Vector2);
			StatsReportUtility.scrollPositionRightPanel = default(Vector2);
			StatsReportUtility.selectedEntry = null;
			StatsReportUtility.mousedOverEntry = null;
			StatsReportUtility.cachedDrawEntries.Clear();
		}

		// Token: 0x060059CE RID: 22990 RVA: 0x001E5C14 File Offset: 0x001E3E14
		public static void DrawStatsReport(Rect rect, Def def, ThingDef stuff)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				BuildableDef buildableDef = def as BuildableDef;
				StatRequest req = (buildableDef != null) ? StatRequest.For(buildableDef, stuff, QualityCategory.Normal) : StatRequest.ForEmpty();
				StatsReportUtility.cachedDrawEntries.AddRange(def.SpecialDisplayStats(req));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(def, stuff)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x060059CF RID: 22991 RVA: 0x001E5CA0 File Offset: 0x001E3EA0
		public static void DrawStatsReport(Rect rect, AbilityDef def)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatRequest req = StatRequest.ForEmpty();
				StatsReportUtility.cachedDrawEntries.AddRange(def.SpecialDisplayStats(req));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(def)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x060059D0 RID: 22992 RVA: 0x001E5D18 File Offset: 0x001E3F18
		public static void DrawStatsReport(Rect rect, Thing thing)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(thing.def.SpecialDisplayStats(StatRequest.For(thing)));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(thing)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.cachedDrawEntries.RemoveAll((StatDrawEntry de) => de.stat != null && !de.stat.showNonAbstract);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, thing, null);
		}

		// Token: 0x060059D1 RID: 22993 RVA: 0x001E5DC0 File Offset: 0x001E3FC0
		public static void DrawStatsReport(Rect rect, WorldObject worldObject)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(worldObject.def.SpecialDisplayStats(StatRequest.ForEmpty()));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(worldObject)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.cachedDrawEntries.RemoveAll((StatDrawEntry de) => de.stat != null && !de.stat.showNonAbstract);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, worldObject);
		}

		// Token: 0x060059D2 RID: 22994 RVA: 0x001E5E68 File Offset: 0x001E4068
		public static void DrawStatsReport(Rect rect, RoyalTitleDef title, Faction faction)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(title.SpecialDisplayStats(StatRequest.For(title, faction)));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(title, faction)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x060059D3 RID: 22995 RVA: 0x001E5EE0 File Offset: 0x001E40E0
		public static void DrawStatsReport(Rect rect, Faction faction)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatRequest req = StatRequest.ForEmpty();
				StatsReportUtility.cachedDrawEntries.AddRange(faction.def.SpecialDisplayStats(req));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(faction)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x060059D4 RID: 22996 RVA: 0x001E5F5B File Offset: 0x001E415B
		private static IEnumerable<StatDrawEntry> StatsToDraw(Def def, ThingDef stuff)
		{
			yield return StatsReportUtility.DescriptionEntry(def);
			BuildableDef eDef = def as BuildableDef;
			if (eDef != null)
			{
				StatRequest statRequest = StatRequest.For(eDef, stuff, QualityCategory.Normal);
				IEnumerable<StatDef> allDefs = DefDatabase<StatDef>.AllDefs;
				Func<StatDef, bool> predicate;
				Func<StatDef, bool> <>9__0;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((StatDef st) => st.Worker.ShouldShowFor(statRequest)));
				}
				foreach (StatDef statDef in allDefs.Where(predicate))
				{
					yield return new StatDrawEntry(statDef.category, statDef, eDef.GetStatValueAbstract(statDef, stuff), StatRequest.For(eDef, stuff, QualityCategory.Normal), ToStringNumberSense.Undefined, null, false);
				}
				IEnumerator<StatDef> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060059D5 RID: 22997 RVA: 0x001E5F72 File Offset: 0x001E4172
		private static IEnumerable<StatDrawEntry> StatsToDraw(RoyalTitleDef title, Faction faction)
		{
			yield return StatsReportUtility.DescriptionEntry(title, faction);
			yield break;
		}

		// Token: 0x060059D6 RID: 22998 RVA: 0x001E5F89 File Offset: 0x001E4189
		private static IEnumerable<StatDrawEntry> StatsToDraw(Faction faction)
		{
			yield return StatsReportUtility.DescriptionEntry(faction);
			yield break;
		}

		// Token: 0x060059D7 RID: 22999 RVA: 0x001E5F99 File Offset: 0x001E4199
		private static IEnumerable<StatDrawEntry> StatsToDraw(AbilityDef def)
		{
			yield return StatsReportUtility.DescriptionEntry(def);
			StatRequest statRequest = StatRequest.For(def);
			IEnumerable<StatDef> allDefs = DefDatabase<StatDef>.AllDefs;
			Func<StatDef, bool> <>9__0;
			Func<StatDef, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((StatDef st) => st.Worker.ShouldShowFor(statRequest)));
			}
			foreach (StatDef statDef in allDefs.Where(predicate))
			{
				yield return new StatDrawEntry(statDef.category, statDef, def.GetStatValueAbstract(statDef), StatRequest.For(def), ToStringNumberSense.Undefined, null, false);
			}
			IEnumerator<StatDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060059D8 RID: 23000 RVA: 0x001E5FA9 File Offset: 0x001E41A9
		private static IEnumerable<StatDrawEntry> StatsToDraw(Thing thing)
		{
			yield return StatsReportUtility.DescriptionEntry(thing);
			StatDrawEntry statDrawEntry = StatsReportUtility.QualityEntry(thing);
			if (statDrawEntry != null)
			{
				yield return statDrawEntry;
			}
			IEnumerable<StatDef> allDefs = DefDatabase<StatDef>.AllDefs;
			Func<StatDef, bool> <>9__0;
			Func<StatDef, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((StatDef st) => st.Worker.ShouldShowFor(StatRequest.For(thing))));
			}
			foreach (StatDef statDef in allDefs.Where(predicate))
			{
				if (!statDef.Worker.IsDisabledFor(thing))
				{
					float statValue = thing.GetStatValue(statDef, true);
					if (statDef.showOnDefaultValue || statValue != statDef.defaultBaseValue)
					{
						yield return new StatDrawEntry(statDef.category, statDef, statValue, StatRequest.For(thing), ToStringNumberSense.Undefined, null, false);
					}
				}
				else
				{
					yield return new StatDrawEntry(statDef.category, statDef);
				}
			}
			IEnumerator<StatDef> enumerator = null;
			if (thing.def.useHitPoints)
			{
				StatDrawEntry statDrawEntry2 = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "HitPointsBasic".Translate().CapitalizeFirst(), thing.HitPoints.ToString() + " / " + thing.MaxHitPoints.ToString(), "Stat_HitPoints_Desc".Translate(), 99998, null, null, false);
				yield return statDrawEntry2;
			}
			foreach (StatDrawEntry statDrawEntry3 in thing.SpecialDisplayStats())
			{
				yield return statDrawEntry3;
			}
			IEnumerator<StatDrawEntry> enumerator2 = null;
			if (thing.def.IsStuff)
			{
				if (!thing.def.stuffProps.statFactors.NullOrEmpty<StatModifier>())
				{
					int num;
					for (int i = 0; i < thing.def.stuffProps.statFactors.Count; i = num + 1)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.StuffStatFactors, thing.def.stuffProps.statFactors[i].stat, thing.def.stuffProps.statFactors[i].value, StatRequest.ForEmpty(), ToStringNumberSense.Factor, null, false);
						num = i;
					}
				}
				if (!thing.def.stuffProps.statOffsets.NullOrEmpty<StatModifier>())
				{
					int num;
					for (int i = 0; i < thing.def.stuffProps.statOffsets.Count; i = num + 1)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.StuffStatOffsets, thing.def.stuffProps.statOffsets[i].stat, thing.def.stuffProps.statOffsets[i].value, StatRequest.ForEmpty(), ToStringNumberSense.Offset, null, false);
						num = i;
					}
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060059D9 RID: 23001 RVA: 0x001E5FB9 File Offset: 0x001E41B9
		private static IEnumerable<StatDrawEntry> StatsToDraw(WorldObject worldObject)
		{
			yield return StatsReportUtility.DescriptionEntry(worldObject);
			foreach (StatDrawEntry statDrawEntry in worldObject.SpecialDisplayStats)
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060059DA RID: 23002 RVA: 0x001E5FCC File Offset: 0x001E41CC
		private static void FinalizeCachedDrawEntries(IEnumerable<StatDrawEntry> original)
		{
			StatsReportUtility.cachedDrawEntries = (from sd in original
			orderby sd.category.displayOrder, sd.DisplayPriorityWithinCategory descending, sd.LabelCap
			select sd).ToList<StatDrawEntry>();
		}

		// Token: 0x060059DB RID: 23003 RVA: 0x001E6050 File Offset: 0x001E4250
		private static StatDrawEntry DescriptionEntry(Def def)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", def.description, 99999, null, Dialog_InfoCard.DefsToHyperlinks(def.descriptionHyperlinks), false);
		}

		// Token: 0x060059DC RID: 23004 RVA: 0x001E6088 File Offset: 0x001E4288
		private static StatDrawEntry DescriptionEntry(Faction faction)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", faction.GetReportText, 99999, null, Dialog_InfoCard.DefsToHyperlinks(faction.def.descriptionHyperlinks), false);
		}

		// Token: 0x060059DD RID: 23005 RVA: 0x001E60C5 File Offset: 0x001E42C5
		private static StatDrawEntry DescriptionEntry(RoyalTitleDef title, Faction faction)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", title.GetReportText(faction), 99999, null, Dialog_InfoCard.TitleDefsToHyperlinks(title.GetHyperlinks(faction)), false);
		}

		// Token: 0x060059DE RID: 23006 RVA: 0x001E60FF File Offset: 0x001E42FF
		private static StatDrawEntry DescriptionEntry(Thing thing)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", thing.DescriptionFlavor, 99999, null, Dialog_InfoCard.DefsToHyperlinks(thing.def.descriptionHyperlinks), false);
		}

		// Token: 0x060059DF RID: 23007 RVA: 0x001E613C File Offset: 0x001E433C
		private static StatDrawEntry DescriptionEntry(WorldObject worldObject)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", worldObject.GetDescription(), 99999, null, null, false);
		}

		// Token: 0x060059E0 RID: 23008 RVA: 0x001E616C File Offset: 0x001E436C
		private static StatDrawEntry QualityEntry(Thing t)
		{
			QualityCategory cat;
			if (!t.TryGetQuality(out cat))
			{
				return null;
			}
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Quality".Translate(), cat.GetLabel().CapitalizeFirst(), "QualityDescription".Translate(), 99999, null, null, false);
		}

		// Token: 0x060059E1 RID: 23009 RVA: 0x001E61C0 File Offset: 0x001E43C0
		public static void SelectEntry(int index)
		{
			if (index < 0 || index > StatsReportUtility.cachedDrawEntries.Count)
			{
				return;
			}
			StatsReportUtility.SelectEntry(StatsReportUtility.cachedDrawEntries[index], true);
		}

		// Token: 0x060059E2 RID: 23010 RVA: 0x001E61E8 File Offset: 0x001E43E8
		public static void SelectEntry(StatDef stat, bool playSound = false)
		{
			foreach (StatDrawEntry statDrawEntry in StatsReportUtility.cachedDrawEntries)
			{
				if (statDrawEntry.stat == stat)
				{
					StatsReportUtility.SelectEntry(statDrawEntry, playSound);
					return;
				}
			}
			Messages.Message("MessageCannotSelectInvisibleStat".Translate(stat), MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x060059E3 RID: 23011 RVA: 0x001E6268 File Offset: 0x001E4468
		private static void SelectEntry(StatDrawEntry rec, bool playSound = true)
		{
			StatsReportUtility.selectedEntry = rec;
			if (playSound)
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x060059E4 RID: 23012 RVA: 0x001E6280 File Offset: 0x001E4480
		private static void DrawStatsWorker(Rect rect, Thing optionalThing, WorldObject optionalWorldObject)
		{
			Rect rect2 = new Rect(rect);
			rect2.width *= 0.5f;
			Rect rect3 = new Rect(rect);
			rect3.x = rect2.xMax;
			rect3.width = rect.xMax - rect3.x;
			Text.Font = GameFont.Small;
			Rect viewRect = new Rect(0f, 0f, rect2.width - 16f, StatsReportUtility.listHeight);
			Widgets.BeginScrollView(rect2, ref StatsReportUtility.scrollPosition, viewRect, true);
			float num = 0f;
			string b = null;
			StatsReportUtility.mousedOverEntry = null;
			for (int i = 0; i < StatsReportUtility.cachedDrawEntries.Count; i++)
			{
				StatDrawEntry ent = StatsReportUtility.cachedDrawEntries[i];
				if (ent.category.LabelCap != b)
				{
					Widgets.ListSeparator(ref num, viewRect.width, ent.category.LabelCap);
					b = ent.category.LabelCap;
				}
				num += ent.Draw(8f, num, viewRect.width - 8f, StatsReportUtility.selectedEntry == ent, delegate
				{
					StatsReportUtility.SelectEntry(ent, true);
				}, delegate
				{
					StatsReportUtility.mousedOverEntry = ent;
				}, StatsReportUtility.scrollPosition, rect2);
			}
			StatsReportUtility.listHeight = num + 100f;
			Widgets.EndScrollView();
			Rect outRect = rect3.ContractedBy(10f);
			StatDrawEntry statDrawEntry;
			if ((statDrawEntry = StatsReportUtility.selectedEntry) == null)
			{
				statDrawEntry = (StatsReportUtility.mousedOverEntry ?? StatsReportUtility.cachedDrawEntries.FirstOrDefault<StatDrawEntry>());
			}
			StatDrawEntry statDrawEntry2 = statDrawEntry;
			if (statDrawEntry2 != null)
			{
				Rect rect4 = new Rect(0f, 0f, outRect.width - 16f, StatsReportUtility.rightPanelHeight);
				StatRequest statRequest;
				if (statDrawEntry2.hasOptionalReq)
				{
					statRequest = statDrawEntry2.optionalReq;
				}
				else if (optionalThing != null)
				{
					statRequest = StatRequest.For(optionalThing);
				}
				else
				{
					statRequest = StatRequest.ForEmpty();
				}
				string explanationText = statDrawEntry2.GetExplanationText(statRequest);
				float num2 = 0f;
				Widgets.BeginScrollView(outRect, ref StatsReportUtility.scrollPositionRightPanel, rect4, true);
				Rect rect5 = rect4;
				rect5.width -= 4f;
				Widgets.Label(rect5, explanationText);
				float num3 = Text.CalcHeight(explanationText, rect5.width) + 10f;
				num2 += num3;
				IEnumerable<Dialog_InfoCard.Hyperlink> hyperlinks = statDrawEntry2.GetHyperlinks(statRequest);
				if (hyperlinks != null)
				{
					Rect rect6 = new Rect(rect5.x, rect5.y + num3, rect5.width, rect5.height - num3);
					Color color = GUI.color;
					GUI.color = Widgets.NormalOptionColor;
					foreach (Dialog_InfoCard.Hyperlink hyperlink in hyperlinks)
					{
						float num4 = Text.CalcHeight(hyperlink.Label, rect6.width);
						Widgets.HyperlinkWithIcon(new Rect(rect6.x, rect6.y, rect6.width, num4), hyperlink, "ViewHyperlink".Translate(hyperlink.Label), 2f, 6f);
						rect6.y += num4;
						rect6.height -= num4;
						num2 += num4;
					}
					GUI.color = color;
				}
				StatsReportUtility.rightPanelHeight = num2;
				Widgets.EndScrollView();
			}
		}

		// Token: 0x040030D2 RID: 12498
		private static StatDrawEntry selectedEntry;

		// Token: 0x040030D3 RID: 12499
		private static StatDrawEntry mousedOverEntry;

		// Token: 0x040030D4 RID: 12500
		private static Vector2 scrollPosition;

		// Token: 0x040030D5 RID: 12501
		private static Vector2 scrollPositionRightPanel;

		// Token: 0x040030D6 RID: 12502
		private static float listHeight;

		// Token: 0x040030D7 RID: 12503
		private static float rightPanelHeight;

		// Token: 0x040030D8 RID: 12504
		private static List<StatDrawEntry> cachedDrawEntries = new List<StatDrawEntry>();
	}
}
