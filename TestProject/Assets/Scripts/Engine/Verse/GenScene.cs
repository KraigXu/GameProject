using System;
using UnityEngine.SceneManagement;
using Verse.Profile;

namespace Verse
{
	
	public static class GenScene
	{
		
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x000255C8 File Offset: 0x000237C8
		public static bool InEntryScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Entry";
			}
		}

		
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x000255EC File Offset: 0x000237EC
		public static bool InPlayScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Play";
			}
		}

		
		public static void GoToMainMenu()
		{
			LongEventHandler.ClearQueuedEvents();
			LongEventHandler.QueueLongEvent(delegate
			{
				MemoryUtility.ClearAllMapsAndWorld();
				Current.Game = null;
			}, "Entry", "LoadingLongEvent", true, null, false);
		}

		
		public const string EntrySceneName = "Entry";

		
		public const string PlaySceneName = "Play";
	}
}
