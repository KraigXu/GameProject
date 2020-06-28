using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E68 RID: 3688
	public class Dialog_SaveFileList_Load : Dialog_SaveFileList
	{
		// Token: 0x06005963 RID: 22883 RVA: 0x001DE889 File Offset: 0x001DCA89
		public Dialog_SaveFileList_Load()
		{
			this.interactButLabel = "LoadGameButton".Translate();
		}

		// Token: 0x06005964 RID: 22884 RVA: 0x001DE8A6 File Offset: 0x001DCAA6
		protected override void DoFileInteraction(string saveFileName)
		{
			GameDataSaveLoader.CheckVersionAndLoadGame(saveFileName);
		}
	}
}
