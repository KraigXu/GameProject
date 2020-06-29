using System;
using Verse;

namespace RimWorld
{
	
	public class Dialog_ScenarioList_Save : Dialog_ScenarioList
	{
		
		// (get) Token: 0x0600596A RID: 22890 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		
		public Dialog_ScenarioList_Save(Scenario scen)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.typingName = scen.name;
			this.savingScen = scen;
		}

		
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

		
		private Scenario savingScen;
	}
}
