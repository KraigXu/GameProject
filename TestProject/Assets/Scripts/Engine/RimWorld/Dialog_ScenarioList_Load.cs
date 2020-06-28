using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E6C RID: 3692
	public class Dialog_ScenarioList_Load : Dialog_ScenarioList
	{
		// Token: 0x0600596D RID: 22893 RVA: 0x001DEAC0 File Offset: 0x001DCCC0
		public Dialog_ScenarioList_Load(Action<Scenario> scenarioReturner)
		{
			this.interactButLabel = "LoadGameButton".Translate();
			this.scenarioReturner = scenarioReturner;
		}

		// Token: 0x0600596E RID: 22894 RVA: 0x001DEAE4 File Offset: 0x001DCCE4
		protected override void DoFileInteraction(string fileName)
		{
			string filePath = GenFilePaths.AbsPathForScenario(fileName);
			PreLoadUtility.CheckVersionAndLoad(filePath, ScribeMetaHeaderUtility.ScribeHeaderMode.Scenario, delegate
			{
				Scenario obj = null;
				if (GameDataSaveLoader.TryLoadScenario(filePath, ScenarioCategory.CustomLocal, out obj))
				{
					this.scenarioReturner(obj);
				}
				this.Close(true);
			});
		}

		// Token: 0x0400306B RID: 12395
		private Action<Scenario> scenarioReturner;
	}
}
