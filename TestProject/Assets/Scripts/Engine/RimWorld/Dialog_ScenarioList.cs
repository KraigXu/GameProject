using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E6A RID: 3690
	public abstract class Dialog_ScenarioList : Dialog_FileList
	{
		// Token: 0x06005968 RID: 22888 RVA: 0x001DE998 File Offset: 0x001DCB98
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllCustomScenarioFiles)
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
	}
}
