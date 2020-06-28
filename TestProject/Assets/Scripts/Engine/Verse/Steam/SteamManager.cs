using System;
using System.Text;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x0200048B RID: 1163
	public static class SteamManager
	{
		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x0600227D RID: 8829 RVA: 0x000D2252 File Offset: 0x000D0452
		public static bool Initialized
		{
			get
			{
				return SteamManager.initializedInt;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x0600227E RID: 8830 RVA: 0x0001028D File Offset: 0x0000E48D
		public static bool Active
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x000D225C File Offset: 0x000D045C
		public static void InitIfNeeded()
		{
			if (SteamManager.initializedInt)
			{
				return;
			}
			if (!Packsize.Test())
			{
				Log.Error("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", false);
			}
			if (!DllCheck.Test())
			{
				Log.Error("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", false);
			}
			try
			{
				if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
				{
					Application.Quit();
					return;
				}
			}
			catch (DllNotFoundException arg)
			{
				Log.Error("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg, false);
				Application.Quit();
				return;
			}
			SteamManager.initializedInt = SteamAPI.Init();
			if (!SteamManager.initializedInt)
			{
				Log.Warning("[Steamworks.NET] SteamAPI.Init() failed. Possible causes: Steam client not running, launched from outside Steam without steam_appid.txt in place, running with different privileges than Steam client (e.g. \"as administrator\")", false);
				return;
			}
			if (SteamManager.steamAPIWarningMessageHook == null)
			{
				SteamManager.steamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
				SteamClient.SetWarningMessageHook(SteamManager.steamAPIWarningMessageHook);
			}
			Workshop.Init();
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x000D2318 File Offset: 0x000D0518
		public static void Update()
		{
			if (!SteamManager.initializedInt)
			{
				return;
			}
			SteamAPI.RunCallbacks();
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x000D2327 File Offset: 0x000D0527
		public static void ShutdownSteam()
		{
			if (!SteamManager.initializedInt)
			{
				return;
			}
			SteamAPI.Shutdown();
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x000D2336 File Offset: 0x000D0536
		private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
		{
			Log.Error(pchDebugText.ToString(), false);
		}

		// Token: 0x04001516 RID: 5398
		private static SteamAPIWarningMessageHook_t steamAPIWarningMessageHook;

		// Token: 0x04001517 RID: 5399
		private static bool initializedInt;
	}
}
