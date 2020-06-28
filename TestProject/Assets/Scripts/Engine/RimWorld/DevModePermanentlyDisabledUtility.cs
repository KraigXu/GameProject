using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB6 RID: 4022
	public static class DevModePermanentlyDisabledUtility
	{
		// Token: 0x17001127 RID: 4391
		// (get) Token: 0x060060BD RID: 24765 RVA: 0x002173ED File Offset: 0x002155ED
		public static bool Disabled
		{
			get
			{
				if (!DevModePermanentlyDisabledUtility.initialized)
				{
					DevModePermanentlyDisabledUtility.initialized = true;
					DevModePermanentlyDisabledUtility.disabled = File.Exists(GenFilePaths.DevModePermanentlyDisabledFilePath);
				}
				return DevModePermanentlyDisabledUtility.disabled;
			}
		}

		// Token: 0x060060BE RID: 24766 RVA: 0x00217410 File Offset: 0x00215610
		public static void Disable()
		{
			try
			{
				File.Create(GenFilePaths.DevModePermanentlyDisabledFilePath).Dispose();
			}
			catch (Exception arg)
			{
				Log.Error("Could not permanently disable dev mode: " + arg, false);
				return;
			}
			DevModePermanentlyDisabledUtility.disabled = true;
			Prefs.DevMode = false;
		}

		// Token: 0x04003AEF RID: 15087
		private static bool initialized;

		// Token: 0x04003AF0 RID: 15088
		private static bool disabled;
	}
}
