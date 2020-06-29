using System;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	
	public class WorkshopItem
	{
		
		// (get) Token: 0x06002297 RID: 8855 RVA: 0x000D2DA9 File Offset: 0x000D0FA9
		public DirectoryInfo Directory
		{
			get
			{
				return this.directoryInt;
			}
		}

		
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

		
		public override string ToString()
		{
			return base.GetType().ToString() + "-" + this.PublishedFileId;
		}

		
		protected DirectoryInfo directoryInt;

		
		private PublishedFileId_t pfidInt;
	}
}
