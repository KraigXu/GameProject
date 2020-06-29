using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	
	public static class WorkshopItems
	{
		
		// (get) Token: 0x060022BC RID: 8892 RVA: 0x000D307E File Offset: 0x000D127E
		public static IEnumerable<WorkshopItem> AllSubscribedItems
		{
			get
			{
				return WorkshopItems.subbedItems;
			}
		}

		
		// (get) Token: 0x060022BD RID: 8893 RVA: 0x000D3088 File Offset: 0x000D1288
		public static int DownloadingItemsCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < WorkshopItems.subbedItems.Count; i++)
				{
					if (WorkshopItems.subbedItems[i] is WorkshopItem_NotInstalled)
					{
						num++;
					}
				}
				return num;
			}
		}

		
		static WorkshopItems()
		{
			WorkshopItems.RebuildItemsList();
		}

		
		public static void EnsureInit()
		{
		}

		
		public static WorkshopItem GetItem(PublishedFileId_t pfid)
		{
			for (int i = 0; i < WorkshopItems.subbedItems.Count; i++)
			{
				if (WorkshopItems.subbedItems[i].PublishedFileId == pfid)
				{
					return WorkshopItems.subbedItems[i];
				}
			}
			return null;
		}

		
		public static bool HasItem(PublishedFileId_t pfid)
		{
			return WorkshopItems.GetItem(pfid) != null;
		}

		
		private static void RebuildItemsList()
		{
			if (!SteamManager.Initialized)
			{
				return;
			}
			WorkshopItems.subbedItems.Clear();
			foreach (PublishedFileId_t pfid in Workshop.AllSubscribedItems())
			{
				WorkshopItems.subbedItems.Add(WorkshopItem.MakeFrom(pfid));
			}
			ModLister.RebuildModList();
			ScenarioLister.MarkDirty();
		}

		
		internal static void Notify_Subscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		
		internal static void Notify_Installed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		
		internal static void Notify_Unsubscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		
		public static string DebugOutput()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Subscribed items:");
			foreach (WorkshopItem workshopItem in WorkshopItems.subbedItems)
			{
				stringBuilder.AppendLine("  " + workshopItem.ToString());
			}
			return stringBuilder.ToString();
		}

		
		private static List<WorkshopItem> subbedItems = new List<WorkshopItem>();
	}
}
