using System;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x0200048D RID: 1165
	public class WorkshopItem
	{
		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06002297 RID: 8855 RVA: 0x000D2DA9 File Offset: 0x000D0FA9
		public DirectoryInfo Directory
		{
			get
			{
				return this.directoryInt;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06002298 RID: 8856 RVA: 0x000D2DB1 File Offset: 0x000D0FB1
		// (set) Token: 0x06002299 RID: 8857 RVA: 0x000D2DB9 File Offset: 0x000D0FB9
		public virtual PublishedFileId_t PublishedFileId
		{
			get
			{
				return this.pfidInt;
			}
			set
			{
				this.pfidInt = value;
			}
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x000D2DC4 File Offset: 0x000D0FC4
		public static WorkshopItem MakeFrom(PublishedFileId_t pfid)
		{
			ulong num;
			string path;
			uint num2;
			bool itemInstallInfo = SteamUGC.GetItemInstallInfo(pfid, out num, out path, 257u, out num2);
			WorkshopItem workshopItem = null;
			if (!itemInstallInfo)
			{
				workshopItem = new WorkshopItem_NotInstalled();
			}
			else
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				if (!directoryInfo.Exists)
				{
					Log.Error("Created WorkshopItem for " + pfid + " but there is no folder for it.", false);
					return new WorkshopItem_NotInstalled();
				}
				FileInfo[] files = directoryInfo.GetFiles();
				for (int i = 0; i < files.Length; i++)
				{
					if (files[i].Extension == ".rsc")
					{
						workshopItem = new WorkshopItem_Scenario();
						break;
					}
				}
				if (workshopItem == null)
				{
					workshopItem = new WorkshopItem_Mod();
				}
				workshopItem.directoryInt = directoryInfo;
			}
			workshopItem.PublishedFileId = pfid;
			return workshopItem;
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x000D2E73 File Offset: 0x000D1073
		public override string ToString()
		{
			return base.GetType().ToString() + "-" + this.PublishedFileId;
		}

		// Token: 0x04001524 RID: 5412
		protected DirectoryInfo directoryInt;

		// Token: 0x04001525 RID: 5413
		private PublishedFileId_t pfidInt;
	}
}
