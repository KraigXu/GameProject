using System;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x0200047B RID: 1147
	public static class SteamUtility
	{
		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060021D9 RID: 8665 RVA: 0x000CE618 File Offset: 0x000CC818
		public static string SteamPersonaName
		{
			get
			{
				if (SteamManager.Initialized && SteamUtility.cachedPersonaName == null)
				{
					SteamUtility.cachedPersonaName = SteamFriends.GetPersonaName();
				}
				if (SteamUtility.cachedPersonaName == null)
				{
					return "???";
				}
				return SteamUtility.cachedPersonaName;
			}
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x000CE644 File Offset: 0x000CC844
		public static void OpenUrl(string url)
		{
			if (SteamManager.Initialized && SteamUtils.IsOverlayEnabled())
			{
				SteamFriends.ActivateGameOverlayToWebPage(url, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
				return;
			}
			Application.OpenURL(url);
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x000CE662 File Offset: 0x000CC862
		public static void OpenWorkshopPage(PublishedFileId_t pfid)
		{
			SteamUtility.OpenUrl(SteamUtility.SteamWorkshopPageUrl(pfid));
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x000CE66F File Offset: 0x000CC86F
		public static void OpenSteamWorkshopPage()
		{
			SteamUtility.OpenUrl("http://steamcommunity.com/workshop/browse/?appid=" + SteamUtils.GetAppID());
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x000CE68A File Offset: 0x000CC88A
		public static string SteamWorkshopPageUrl(PublishedFileId_t pfid)
		{
			return "steam://url/CommunityFilePage/" + pfid;
		}

		// Token: 0x040014D2 RID: 5330
		private static string cachedPersonaName;
	}
}
