using System;
using System.Collections.Generic;
using System.IO;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	
	public static class ScenarioFiles
	{
		
		// (get) Token: 0x0600494B RID: 18763 RVA: 0x0018E196 File Offset: 0x0018C396
		public static IEnumerable<Scenario> AllScenariosLocal
		{
			get
			{
				return ScenarioFiles.scenariosLocal;
			}
		}

		
		// (get) Token: 0x0600494C RID: 18764 RVA: 0x0018E19D File Offset: 0x0018C39D
		public static IEnumerable<Scenario> AllScenariosWorkshop
		{
			get
			{
				return ScenarioFiles.scenariosWorkshop;
			}
		}

		
		public static void RecacheData()
		{
			ScenarioFiles.scenariosLocal.Clear();
			using (IEnumerator<FileInfo> enumerator = GenFilePaths.AllCustomScenarioFiles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Scenario item;
					if (GameDataSaveLoader.TryLoadScenario(enumerator.Current.FullName, ScenarioCategory.CustomLocal, out item))
					{
						ScenarioFiles.scenariosLocal.Add(item);
					}
				}
			}
			ScenarioFiles.scenariosWorkshop.Clear();
			foreach (WorkshopItem workshopItem in WorkshopItems.AllSubscribedItems)
			{
				WorkshopItem_Scenario workshopItem_Scenario = workshopItem as WorkshopItem_Scenario;
				if (workshopItem_Scenario != null)
				{
					ScenarioFiles.scenariosWorkshop.Add(workshopItem_Scenario.GetScenario());
				}
			}
		}

		
		private static List<Scenario> scenariosLocal = new List<Scenario>();

		
		private static List<Scenario> scenariosWorkshop = new List<Scenario>();
	}
}
