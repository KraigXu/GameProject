using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	
	public static class UnityData
	{
		
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000EF09 File Offset: 0x0000D109
		public static bool IsInMainThread
		{
			get
			{
				return UnityData.mainThreadId == Thread.CurrentThread.ManagedThreadId;
			}
		}

		
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000EF1C File Offset: 0x0000D11C
		public static bool Is32BitBuild
		{
			get
			{
				return IntPtr.Size == 4;
			}
		}

		
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000EF26 File Offset: 0x0000D126
		public static bool Is64BitBuild
		{
			get
			{
				return IntPtr.Size == 8;
			}
		}

		
		static UnityData()
		{
			if (!UnityData.initialized && !UnityDataInitializer.initializing)
			{
				Log.Warning("Used UnityData before it's initialized.", false);
			}
		}

		
		public static void CopyUnityData()
		{
			UnityData.mainThreadId = Thread.CurrentThread.ManagedThreadId;
			UnityData.isEditor = Application.isEditor;
			UnityData.dataPath = Application.dataPath;
			UnityData.platform = Application.platform;
			UnityData.persistentDataPath = Application.persistentDataPath;
			UnityData.initialized = true;
		}

		
		private static bool initialized;

		
		public static bool isEditor;

		
		public static string dataPath;

		
		public static RuntimePlatform platform;

		
		public static string persistentDataPath;

		
		private static int mainThreadId;
	}
}
