using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102D RID: 4141
	public static class VersionUpdateDialogMaker
	{
		// Token: 0x06006319 RID: 25369 RVA: 0x00226FF4 File Offset: 0x002251F4
		public static void CreateVersionUpdateDialogIfNecessary()
		{
			if (!VersionUpdateDialogMaker.dialogDone && LastPlayedVersion.Version != null && (VersionControl.CurrentMajor != LastPlayedVersion.Version.Major || VersionControl.CurrentMinor != LastPlayedVersion.Version.Minor))
			{
				VersionUpdateDialogMaker.CreateNewVersionDialog();
			}
		}

		// Token: 0x0600631A RID: 25370 RVA: 0x00227034 File Offset: 0x00225234
		private static void CreateNewVersionDialog()
		{
			string value = LastPlayedVersion.Version.Major + "." + LastPlayedVersion.Version.Minor;
			string value2 = VersionControl.CurrentMajor + "." + VersionControl.CurrentMinor;
			string text = "GameUpdatedToNewVersionInitial".Translate(value, value2);
			text += "\n\n";
			if (BackCompatibility.IsSaveCompatibleWith(LastPlayedVersion.Version.ToString()))
			{
				text += "GameUpdatedToNewVersionSavesCompatible".Translate();
			}
			else
			{
				text += "GameUpdatedToNewVersionSavesIncompatible".Translate();
			}
			text += "\n\n";
			text += "GameUpdatedToNewVersionSteam".Translate();
			Find.WindowStack.Add(new Dialog_MessageBox(text, null, null, null, null, null, false, null, null));
			VersionUpdateDialogMaker.dialogDone = true;
		}

		// Token: 0x04003C4B RID: 15435
		private static bool dialogDone;
	}
}
