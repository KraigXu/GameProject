using System;
using UnityEngine.SceneManagement;
using Verse.Profile;

namespace Verse
{
	// Token: 0x0200011E RID: 286
	public static class GenScene
	{
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x000255C8 File Offset: 0x000237C8
		public static bool InEntryScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Entry";
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x000255EC File Offset: 0x000237EC
		public static bool InPlayScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Play";
			}
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00025610 File Offset: 0x00023810
		public static void GoToMainMenu()
		{
			LongEventHandler.ClearQueuedEvents();
			LongEventHandler.QueueLongEvent(delegate
			{
				MemoryUtility.ClearAllMapsAndWorld();
				Current.Game = null;
			}, "Entry", "LoadingLongEvent", true, null, false);
		}

		// Token: 0x0400072A RID: 1834
		public const string EntrySceneName = "Entry";

		// Token: 0x0400072B RID: 1835
		public const string PlaySceneName = "Play";
	}
}
