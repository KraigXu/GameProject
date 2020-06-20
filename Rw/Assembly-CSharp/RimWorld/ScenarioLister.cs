using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C09 RID: 3081
	public static class ScenarioLister
	{
		// Token: 0x0600494F RID: 18767 RVA: 0x0018E27A File Offset: 0x0018C47A
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

		// Token: 0x06004950 RID: 18768 RVA: 0x0018E283 File Offset: 0x0018C483
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

		// Token: 0x06004951 RID: 18769 RVA: 0x0018E294 File Offset: 0x0018C494
		public static bool ScenarioIsListedAnywhere(Scenario scen)
		{
			ScenarioLister.RecacheIfDirty();
			using (IEnumerator<ScenarioDef> enumerator = DefDatabase<ScenarioDef>.AllDefs.GetEnumerator())
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

		// Token: 0x06004952 RID: 18770 RVA: 0x0018E328 File Offset: 0x0018C528
		public static void MarkDirty()
		{
			ScenarioLister.dirty = true;
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x0018E330 File Offset: 0x0018C530
		private static void RecacheIfDirty()
		{
			if (ScenarioLister.dirty)
			{
				ScenarioLister.RecacheData();
			}
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x0018E340 File Offset: 0x0018C540
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

		// Token: 0x06004955 RID: 18773 RVA: 0x0018E384 File Offset: 0x0018C584
		public static int ScenarioListHash()
		{
			int num = 9826121;
			foreach (Scenario scenario in ScenarioLister.AllScenarios())
			{
				num ^= 791 * scenario.GetHashCode() * 6121;
			}
			return num;
		}

		// Token: 0x040029E1 RID: 10721
		private static bool dirty = true;
	}
}
