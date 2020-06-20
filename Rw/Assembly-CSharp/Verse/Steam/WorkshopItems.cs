using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000493 RID: 1171
	public static class WorkshopItems
	{
		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x060022BC RID: 8892 RVA: 0x000D307E File Offset: 0x000D127E
		public static IEnumerable<WorkshopItem> AllSubscribedItems
		{
			get
			{
				return WorkshopItems.subbedItems;
			}
		}

		// Token: 0x170006DA RID: 1754
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

		// Token: 0x060022BE RID: 8894 RVA: 0x000D30C3 File Offset: 0x000D12C3
		static WorkshopItems()
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x00002681 File Offset: 0x00000881
		public static void EnsureInit()
		{
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x000D30D4 File Offset: 0x000D12D4
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

		// Token: 0x060022C1 RID: 8897 RVA: 0x000D311B File Offset: 0x000D131B
		public static bool HasItem(PublishedFileId_t pfid)
		{
			return WorkshopItems.GetItem(pfid) != null;
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x000D3128 File Offset: 0x000D1328
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

		// Token: 0x060022C3 RID: 8899 RVA: 0x000D319C File Offset: 0x000D139C
		internal static void Notify_Subscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x000D319C File Offset: 0x000D139C
		internal static void Notify_Installed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x000D319C File Offset: 0x000D139C
		internal static void Notify_Unsubscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x000D31A4 File Offset: 0x000D13A4
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

		// Token: 0x0400152A RID: 5418
		private static List<WorkshopItem> subbedItems = new List<WorkshopItem>();
	}
}
