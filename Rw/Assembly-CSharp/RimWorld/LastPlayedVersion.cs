using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102C RID: 4140
	public static class LastPlayedVersion
	{
		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x06006316 RID: 25366 RVA: 0x00226F12 File Offset: 0x00225112
		public static Version Version
		{
			get
			{
				LastPlayedVersion.InitializeIfNeeded();
				return LastPlayedVersion.lastPlayedVersionInt;
			}
		}

		// Token: 0x06006317 RID: 25367 RVA: 0x00226F20 File Offset: 0x00225120
		public static void InitializeIfNeeded()
		{
			if (LastPlayedVersion.initialized)
			{
				return;
			}
			try
			{
				string text = null;
				if (File.Exists(GenFilePaths.LastPlayedVersionFilePath))
				{
					try
					{
						text = File.ReadAllText(GenFilePaths.LastPlayedVersionFilePath);
					}
					catch (Exception ex)
					{
						Log.Error("Exception getting last played version data. Path: " + GenFilePaths.LastPlayedVersionFilePath + ". Exception: " + ex.ToString(), false);
					}
				}
				if (text != null)
				{
					try
					{
						LastPlayedVersion.lastPlayedVersionInt = VersionControl.VersionFromString(text);
					}
					catch (Exception ex2)
					{
						Log.Error("Exception parsing last version from string '" + text + "': " + ex2.ToString(), false);
					}
				}
				if (LastPlayedVersion.lastPlayedVersionInt != VersionControl.CurrentVersion)
				{
					File.WriteAllText(GenFilePaths.LastPlayedVersionFilePath, VersionControl.CurrentVersionString);
				}
			}
			finally
			{
				LastPlayedVersion.initialized = true;
			}
		}

		// Token: 0x04003C49 RID: 15433
		private static bool initialized;

		// Token: 0x04003C4A RID: 15434
		private static Version lastPlayedVersionInt;
	}
}
