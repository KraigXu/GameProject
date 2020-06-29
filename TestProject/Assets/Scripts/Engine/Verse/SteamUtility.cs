using System;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	
	public static class SteamUtility
	{
		
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

		
		public static void OpenUrl(string url)
		{
			if (SteamManager.Initialized && SteamUtils.IsOverlayEnabled())
			{
				SteamFriends.ActivateGameOverlayToWebPage(url, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
				return;
			}
			Application.OpenURL(url);
		}

		
		public static void OpenWorkshopPage(PublishedFileId_t pfid)
		{
			SteamUtility.OpenUrl(SteamUtility.SteamWorkshopPageUrl(pfid));
		}

		
		public static void OpenSteamWorkshopPage()
		{
			SteamUtility.OpenUrl("http://steamcommunity.com/workshop/browse/?appid=" + SteamUtils.GetAppID());
		}

		
		public static string SteamWorkshopPageUrl(PublishedFileId_t pfid)
		{
			return "steam://url/CommunityFilePage/" + pfid;
		}

		
		private static string cachedPersonaName;
	}
}
