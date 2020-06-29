using System;
using Verse;

namespace RimWorld
{
	
	public class Dialog_SaveFileList_Save : Dialog_SaveFileList
	{
		
		// (get) Token: 0x06005965 RID: 22885 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		
		public Dialog_SaveFileList_Save()
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.bottomAreaHeight = 85f;
			if (Faction.OfPlayer.HasName)
			{
				this.typingName = Faction.OfPlayer.Name;
				return;
			}
			this.typingName = SaveGameFilesUtility.UnusedDefaultFileName(Faction.OfPlayer.def.LabelCap);
		}

		
		protected override void DoFileInteraction(string mapName)
		{
			mapName = GenFile.SanitizedFileName(mapName);
			LongEventHandler.QueueLongEvent(delegate
			{
				GameDataSaveLoader.SaveGame(mapName);
			}, "SavingLongEvent", false, null, true);
			Messages.Message("SavedAs".Translate(mapName), MessageTypeDefOf.SilentInput, false);
			PlayerKnowledgeDatabase.Save();
			this.Close(true);
		}
	}
}
