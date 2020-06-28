using System;

namespace Verse
{
	// Token: 0x0200002C RID: 44
	public static class UnityDataInitializer
	{
		// Token: 0x060002E5 RID: 741 RVA: 0x0000EF8C File Offset: 0x0000D18C
		public static void CopyUnityData()
		{
			UnityDataInitializer.initializing = true;
			try
			{
				UnityData.CopyUnityData();
			}
			finally
			{
				UnityDataInitializer.initializing = false;
			}
		}

		// Token: 0x04000082 RID: 130
		public static bool initializing;
	}
}
