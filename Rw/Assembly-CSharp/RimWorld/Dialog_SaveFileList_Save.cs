using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E69 RID: 3689
	public class Dialog_SaveFileList_Save : Dialog_SaveFileList
	{
		// Token: 0x17001015 RID: 4117
		// (get) Token: 0x06005965 RID: 22885 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005966 RID: 22886 RVA: 0x001DE8B0 File Offset: 0x001DCAB0
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

		// Token: 0x06005967 RID: 22887 RVA: 0x001DE920 File Offset: 0x001DCB20
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
