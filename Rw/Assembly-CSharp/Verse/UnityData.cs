using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200002B RID: 43
	public static class UnityData
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000EF09 File Offset: 0x0000D109
		public static bool IsInMainThread
		{
			get
			{
				return UnityData.mainThreadId == Thread.CurrentThread.ManagedThreadId;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000EF1C File Offset: 0x0000D11C
		public static bool Is32BitBuild
		{
			get
			{
				return IntPtr.Size == 4;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000EF26 File Offset: 0x0000D126
		public static bool Is64BitBuild
		{
			get
			{
				return IntPtr.Size == 8;
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000EF30 File Offset: 0x0000D130
		static UnityData()
		{
			if (!UnityData.initialized && !UnityDataInitializer.initializing)
			{
				Log.Warning("Used UnityData before it's initialized.", false);
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000EF4B File Offset: 0x0000D14B
		public static void CopyUnityData()
		{
			UnityData.mainThreadId = Thread.CurrentThread.ManagedThreadId;
			UnityData.isEditor = Application.isEditor;
			UnityData.dataPath = Application.dataPath;
			UnityData.platform = Application.platform;
			UnityData.persistentDataPath = Application.persistentDataPath;
			UnityData.initialized = true;
		}

		// Token: 0x0400007C RID: 124
		private static bool initialized;

		// Token: 0x0400007D RID: 125
		public static bool isEditor;

		// Token: 0x0400007E RID: 126
		public static string dataPath;

		// Token: 0x0400007F RID: 127
		public static RuntimePlatform platform;

		// Token: 0x04000080 RID: 128
		public static string persistentDataPath;

		// Token: 0x04000081 RID: 129
		private static int mainThreadId;
	}
}
