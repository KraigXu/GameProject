using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public static class TechprintUtility
	{
		
		public static IEnumerable<ResearchProjectDef> GetResearchProjectsNeedingTechprintsNow(Faction faction, List<ThingDef> alreadyGeneratedTechprints = null, float maxMarketValue = 3.40282347E+38f)
		{
			return DefDatabase<ResearchProjectDef>.AllDefsListForReading.Where(delegate(ResearchProjectDef p)
			{
				if (p.techprintCount == 0)
				{
					return false;
				}
				if (p.IsFinished || p.TechprintRequirementMet)
				{
					return false;
				}
				if (faction != null && (p.heldByFactionCategoryTags == null || !p.heldByFactionCategoryTags.Contains(faction.def.categoryTag)))
				{
					return false;
				}
				if (maxMarketValue != 3.40282347E+38f && p.Techprint.BaseMarketValue > maxMarketValue)
				{
					return false;
				}
				if (alreadyGeneratedTechprints != null)
				{
					CompProperties_Techprint compProperties = p.Techprint.GetCompProperties<CompProperties_Techprint>();
					if (compProperties != null)
					{
						int num = compProperties.project.techprintCount - compProperties.project.TechprintsApplied;
						int num2 = 0;
						for (int i = 0; i < alreadyGeneratedTechprints.Count; i++)
						{
							if (alreadyGeneratedTechprints[i] == p.Techprint)
							{
								num2++;
							}
						}
						if (num2 >= num)
						{
							return false;
						}
					}
				}
				return true;
			});
		}

		
		public static float GetSelectionWeight(ResearchProjectDef project)
		{
			return project.techprintCommonality * (project.PrerequisitesCompleted ? 1f : 0.02f);
		}

		
		public static bool TryGetTechprintDefToGenerate(Faction faction, out ThingDef result, List<ThingDef> alreadyGeneratedTechprints = null, float maxMarketValue = 3.40282347E+38f)
		{
			ResearchProjectDef researchProjectDef;
			if (!TechprintUtility.GetResearchProjectsNeedingTechprintsNow(faction, alreadyGeneratedTechprints, maxMarketValue).TryRandomElementByWeight(new Func<ResearchProjectDef, float>(TechprintUtility.GetSelectionWeight), out researchProjectDef))
			{
				result = null;
				return false;
			}
			result = researchProjectDef.Techprint;
			return true;
		}

		
		[DebugOutput]
		public static void TechprintsFromFactions()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Techprints generated from these factions:");
			foreach (Faction faction in from fa in Find.FactionManager.AllFactions
			where fa.def.humanlikeFaction && !fa.def.hidden
			select fa)
			{
				stringBuilder.AppendLine(faction.Name);
				for (int i = 0; i < 30; i++)
				{
					ThingDef thingDef;
					if (!TechprintUtility.TryGetTechprintDefToGenerate(faction, out thingDef, null, 3.40282347E+38f))
					{
						stringBuilder.AppendLine("    none possible");
						break;
					}
					stringBuilder.AppendLine("    " + thingDef.LabelCap);
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		
		[DebugOutput]
		public static void TechprintsFromFactionsChances()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Faction localFac2 in from fa in Find.FactionManager.AllFactions
			where fa.def.humanlikeFaction && !fa.def.hidden
			select fa)
			{
				Faction localFac = localFac2;
				list.Add(new FloatMenuOption(localFac.Name + " (" + localFac.def.defName + ")", delegate
				{
					List<TableDataGetter<ResearchProjectDef>> list2 = new List<TableDataGetter<ResearchProjectDef>>();
					list2.Add(new TableDataGetter<ResearchProjectDef>("defName", (ResearchProjectDef d) => d.defName));
					IEnumerable<ResearchProjectDef> researchProjectsNeedingTechprintsNow = TechprintUtility.GetResearchProjectsNeedingTechprintsNow(localFac, null, float.MaxValue);
					if (researchProjectsNeedingTechprintsNow.Any<ResearchProjectDef>())
					{
						float sum = researchProjectsNeedingTechprintsNow.Sum((ResearchProjectDef x) => TechprintUtility.GetSelectionWeight(x));
						list2.Add(new TableDataGetter<ResearchProjectDef>("chance", (ResearchProjectDef x) => (TechprintUtility.GetSelectionWeight(x) / sum).ToStringPercent()));
						list2.Add(new TableDataGetter<ResearchProjectDef>("weight", (ResearchProjectDef x) => TechprintUtility.GetSelectionWeight(x).ToString("0.###")));
					}
					DebugTables.MakeTablesDialog<ResearchProjectDef>(researchProjectsNeedingTechprintsNow, list2.ToArray());
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}
	}
}
