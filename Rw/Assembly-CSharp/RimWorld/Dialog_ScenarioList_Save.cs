using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E6B RID: 3691
	public class Dialog_ScenarioList_Save : Dialog_ScenarioList
	{
		// Token: 0x17001016 RID: 4118
		// (get) Token: 0x0600596A RID: 22890 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600596B RID: 22891 RVA: 0x001DEA2C File Offset: 0x001DCC2C
		public Dialog_ScenarioList_Save(Scenario scen)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.typingName = scen.name;
			this.savingScen = scen;
		}

		// Token: 0x0600596C RID: 22892 RVA: 0x001DEA5C File Offset: 0x001DCC5C
		protected override void DoFileInteraction(string fileName)
		{
			string absPath = GenFilePaths.AbsPathForScenario(fileName);
			LongEventHandler.QueueLongEvent(delegate
			{
				GameDataSaveLoader.SaveScenario(this.savingScen, absPath);
			}, "SavingLongEvent", false, null, true);
			Messages.Message("SavedAs".Translate(fileName), MessageTypeDefOf.SilentInput, false);
			this.Close(true);
		}

		// Token: 0x0400306A RID: 12394
		private Scenario savingScen;
	}
}
