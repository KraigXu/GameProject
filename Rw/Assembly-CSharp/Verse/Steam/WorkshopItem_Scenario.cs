using System;
using System.IO;
using System.Linq;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x0200048F RID: 1167
	public class WorkshopItem_Scenario : WorkshopItem
	{
		// Token: 0x170006CF RID: 1743
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

		// Token: 0x060022A0 RID: 8864 RVA: 0x000D2EC2 File Offset: 0x000D10C2
		public Scenario GetScenario()
		{
			if (this.cachedScenario == null)
			{
				this.LoadScenario();
			}
			return this.cachedScenario;
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x000D2ED8 File Offset: 0x000D10D8
		private void LoadScenario()
		{
			if (GameDataSaveLoader.TryLoadScenario((from fi in base.Directory.GetFiles("*.rsc")
			where fi.Extension == ".rsc"
			select fi).First<FileInfo>().FullName, ScenarioCategory.SteamWorkshop, out this.cachedScenario))
			{
				this.cachedScenario.SetPublishedFileId(this.PublishedFileId);
			}
		}

		// Token: 0x04001526 RID: 5414
		private Scenario cachedScenario;
	}
}
