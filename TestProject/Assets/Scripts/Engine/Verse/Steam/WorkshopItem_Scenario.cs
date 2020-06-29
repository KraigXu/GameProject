using System;
using System.IO;
using System.Linq;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	
	public class WorkshopItem_Scenario : WorkshopItem
	{
		
		// (get) Token: 0x0600229E RID: 8862 RVA: 0x000D2E9D File Offset: 0x000D109D
		// (set) Token: 0x0600229F RID: 8863 RVA: 0x000D2EA5 File Offset: 0x000D10A5
		public override PublishedFileId_t PublishedFileId
		{
			get
			{
				return base.PublishedFileId;
			}
			set
			{
				base.PublishedFileId = value;
				if (this.cachedScenario != null)
				{
					this.cachedScenario.SetPublishedFileId(value);
				}
			}
		}

		
		public Scenario GetScenario()
		{
			if (this.cachedScenario == null)
			{
				this.LoadScenario();
			}
			return this.cachedScenario;
		}

		
		private void LoadScenario()
		{
			if (GameDataSaveLoader.TryLoadScenario((from fi in base.Directory.GetFiles("*.rsc")
			where fi.Extension == ".rsc"
			select fi).First<FileInfo>().FullName, ScenarioCategory.SteamWorkshop, out this.cachedScenario))
			{
				this.cachedScenario.SetPublishedFileId(this.PublishedFileId);
			}
		}

		
		private Scenario cachedScenario;
	}
}
