using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public static class ScenarioLister
	{
		
		public static IEnumerable<Scenario> AllScenarios()
		{
			ScenarioLister.RecacheIfDirty();
			foreach (ScenarioDef scenarioDef in DefDatabase<ScenarioDef>.AllDefs)
			{
				yield return scenarioDef.scenario;
			}
			IEnumerator<ScenarioDef> enumerator = null;
			foreach (Scenario scenario in ScenarioFiles.AllScenariosLocal)
			{
				yield return scenario;
			}
			IEnumerator<Scenario> enumerator2 = null;
			foreach (Scenario scenario2 in ScenarioFiles.AllScenariosWorkshop)
			{
				yield return scenario2;
			}
			enumerator2 = null;
			yield break;
			yield break;
		}

		
		public static IEnumerable<Scenario> ScenariosInCategory(ScenarioCategory cat)
		{
			ScenarioLister.RecacheIfDirty();
			if (cat == ScenarioCategory.FromDef)
			{
				foreach (ScenarioDef scenarioDef in DefDatabase<ScenarioDef>.AllDefs)
				{
					yield return scenarioDef.scenario;
				}
				IEnumerator<ScenarioDef> enumerator = null;
			}
			else if (cat == ScenarioCategory.CustomLocal)
			{
				foreach (Scenario scenario in ScenarioFiles.AllScenariosLocal)
				{
					yield return scenario;
				}
				IEnumerator<Scenario> enumerator2 = null;
			}
			else if (cat == ScenarioCategory.SteamWorkshop)
			{
				foreach (Scenario scenario2 in ScenarioFiles.AllScenariosWorkshop)
				{
					yield return scenario2;
				}
				IEnumerator<Scenario> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		
		public static bool ScenarioIsListedAnywhere(Scenario scen)
		{
			ScenarioLister.RecacheIfDirty();
			IEnumerator<ScenarioDef> enumerator = DefDatabase<ScenarioDef>.AllDefs.GetEnumerator();
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.scenario == scen)
					{
						return true;
					}
				}
			}
			foreach (Scenario scenario in ScenarioFiles.AllScenariosLocal)
			{
				if (scen == scenario)
				{
					return true;
				}
			}
			return false;
		}

		
		public static void MarkDirty()
		{
			ScenarioLister.dirty = true;
		}

		
		private static void RecacheIfDirty()
		{
			if (ScenarioLister.dirty)
			{
				ScenarioLister.RecacheData();
			}
		}

		
		private static void RecacheData()
		{
			ScenarioLister.dirty = false;
			int num = ScenarioLister.ScenarioListHash();
			ScenarioFiles.RecacheData();
			if (ScenarioLister.ScenarioListHash() != num && !LongEventHandler.ShouldWaitForEvent)
			{
				Page_SelectScenario page_SelectScenario = Find.WindowStack.WindowOfType<Page_SelectScenario>();
				if (page_SelectScenario != null)
				{
					page_SelectScenario.Notify_ScenarioListChanged();
				}
			}
		}

		
		public static int ScenarioListHash()
		{
			int num = 9826121;
			foreach (Scenario scenario in ScenarioLister.AllScenarios())
			{
				num ^= 791 * scenario.GetHashCode() * 6121;
			}
			return num;
		}

		
		private static bool dirty = true;
	}
}
