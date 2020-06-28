using System;
using System.IO;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E67 RID: 3687
	public abstract class Dialog_SaveFileList : Dialog_FileList
	{
		// Token: 0x0600595E RID: 22878 RVA: 0x001DE78A File Offset: 0x001DC98A
		protected override Color FileNameColor(SaveFileInfo sfi)
		{
			if (SaveGameFilesUtility.IsAutoSave(Path.GetFileNameWithoutExtension(sfi.FileInfo.Name)))
			{
				GUI.color = Dialog_SaveFileList.AutosaveTextColor;
			}
			return base.FileNameColor(sfi);
		}

		// Token: 0x0600595F RID: 22879 RVA: 0x001DE7B8 File Offset: 0x001DC9B8
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllSavedGameFiles)
			{
				try
				{
					this.files.Add(new SaveFileInfo(fileInfo));
				}
				catch (Exception ex)
				{
					Log.Error("Exception loading " + fileInfo.Name + ": " + ex.ToString(), false);
				}
			}
		}

		// Token: 0x06005960 RID: 22880 RVA: 0x001DE84C File Offset: 0x001DCA4C
		public override void PostClose()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Menu, true);
			}
		}

		// Token: 0x04003069 RID: 12393
		private static readonly Color AutosaveTextColor = new Color(0.75f, 0.75f, 0.75f);
	}
}
