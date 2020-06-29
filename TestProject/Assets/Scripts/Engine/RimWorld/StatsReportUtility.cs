using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public static class StatsReportUtility
	{
		
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

		
		public static void Reset()
		{
			StatsReportUtility.scrollPosition = default(Vector2);
			StatsReportUtility.scrollPositionRightPanel = default(Vector2);
			StatsReportUtility.selectedEntry = null;
			StatsReportUtility.mousedOverEntry = null;
			StatsReportUtility.cachedDrawEntries.Clear();
		}

		
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

		
		private static IEnumerable<StatDrawEntry> StatsToDraw(Def def, ThingDef stuff)
		{
			yield return StatsReportUtility.DescriptionEntry(def);
			BuildableDef eDef = def as BuildableDef;
			if (eDef != null)
			{
				StatRequest statRequest = StatRequest.For(eDef, stuff, QualityCategory.Normal);
				IEnumerable<StatDef> allDefs = DefDatabase<StatDef>.AllDefs;
				Func<StatDef, bool> predicate= st=> st.Worker.ShouldShowFor(statRequest);
				foreach (StatDef statDef in allDefs.Where(predicate))
				{
					yield return new StatDrawEntry(statDef.category, statDef, eDef.GetStatValueAbstract(statDef, stuff), StatRequest.For(eDef, stuff, QualityCategory.Normal), ToStringNumberSense.Undefined, null, false);
				}
				IEnumerator<StatDef> enumerator = null;
			}
			yield break;
			yield break;
		}

		
		private static IEnumerable<StatDrawEntry> StatsToDraw(RoyalTitleDef title, Faction faction)
		{
			yield return StatsReportUtility.DescriptionEntry(title, faction);
			yield break;
		}

		
		private static IEnumerable<StatDrawEntry> StatsToDraw(Faction faction)
		{
			yield return StatsReportUtility.DescriptionEntry(faction);
			yield break;
		}

		
		private static IEnumerable<StatDrawEntry> StatsToDraw(AbilityDef def)
		{
			yield return StatsReportUtility.DescriptionEntry(def);
			StatRequest statRequest = StatRequest.For(def);
			IEnumerable<StatDef> allDefs = DefDatabase<StatDef>.AllDefs;
			Func<StatDef, bool> predicate=st=> st.Worker.ShouldShowFor(statRequest);

			foreach (StatDef statDef in allDefs.Where(predicate))
			{
				yield return new StatDrawEntry(statDef.category, statDef, def.GetStatValueAbstract(statDef), StatRequest.For(def), ToStringNumberSense.Undefined, null, false);
			}
			IEnumerator<StatDef> enumerator = null;
			yield break;
			yield break;
		}

		
		private static IEnumerable<StatDrawEntry> StatsToDraw(Thing thing)
		{
			yield return StatsReportUtility.DescriptionEntry(thing);
			StatDrawEntry statDrawEntry = StatsReportUtility.QualityEntry(thing);
			if (statDrawEntry != null)
			{
				yield return statDrawEntry;
			}
			IEnumerable<StatDef> allDefs = DefDatabase<StatDef>.AllDefs;
			Func<StatDef, bool> predicate= st => st.Worker.ShouldShowFor(StatRequest.For(thing));

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

		
		private static void FinalizeCachedDrawEntries(IEnumerable<StatDrawEntry> original)
		{
			StatsReportUtility.cachedDrawEntries = (from sd in original
			orderby sd.category.displayOrder, sd.DisplayPriorityWithinCategory descending, sd.LabelCap
			select sd).ToList<StatDrawEntry>();
		}

		
		private static StatDrawEntry DescriptionEntry(Def def)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", def.description, 99999, null, Dialog_InfoCard.DefsToHyperlinks(def.descriptionHyperlinks), false);
		}

		
		private static StatDrawEntry DescriptionEntry(Faction faction)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", faction.GetReportText, 99999, null, Dialog_InfoCard.DefsToHyperlinks(faction.def.descriptionHyperlinks), false);
		}

		
		private static StatDrawEntry DescriptionEntry(RoyalTitleDef title, Faction faction)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", title.GetReportText(faction), 99999, null, Dialog_InfoCard.TitleDefsToHyperlinks(title.GetHyperlinks(faction)), false);
		}

		
		private static StatDrawEntry DescriptionEntry(Thing thing)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", thing.DescriptionFlavor, 99999, null, Dialog_InfoCard.DefsToHyperlinks(thing.def.descriptionHyperlinks), false);
		}

		
		private static StatDrawEntry DescriptionEntry(WorldObject worldObject)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", worldObject.GetDescription(), 99999, null, null, false);
		}

		
		private static StatDrawEntry QualityEntry(Thing t)
		{
			QualityCategory cat;
			if (!t.TryGetQuality(out cat))
			{
				return null;
			}
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Quality".Translate(), cat.GetLabel().CapitalizeFirst(), "QualityDescription".Translate(), 99999, null, null, false);
		}

		
		public static void SelectEntry(int index)
		{
			if (index < 0 || index > StatsReportUtility.cachedDrawEntries.Count)
			{
				return;
			}
			StatsReportUtility.SelectEntry(StatsReportUtility.cachedDrawEntries[index], true);
		}

		
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

		
		private static void SelectEntry(StatDrawEntry rec, bool playSound = true)
		{
			StatsReportUtility.selectedEntry = rec;
			if (playSound)
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
		}

		
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

		
		private static StatDrawEntry selectedEntry;

		
		private static StatDrawEntry mousedOverEntry;

		
		private static Vector2 scrollPosition;

		
		private static Vector2 scrollPositionRightPanel;

		
		private static float listHeight;

		
		private static float rightPanelHeight;

		
		private static List<StatDrawEntry> cachedDrawEntries = new List<StatDrawEntry>();
	}
}
