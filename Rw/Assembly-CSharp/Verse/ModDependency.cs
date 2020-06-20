using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001FC RID: 508
	public class ModDependency : ModRequirement
	{
		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x000530D5 File Offset: 0x000512D5
		public override string RequirementTypeLabel
		{
			get
			{
				return "ModDependsOn".Translate("");
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x000530F0 File Offset: 0x000512F0
		public override bool IsSatisfied
		{
			get
			{
				return ModLister.GetActiveModWithIdentifier(this.packageId) != null;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000E99 RID: 3737 RVA: 0x00053100 File Offset: 0x00051300
		public override Texture2D StatusIcon
		{
			get
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
				if (modWithIdentifier == null)
				{
					return ModRequirement.NotInstalled;
				}
				if (!modWithIdentifier.Active)
				{
					return ModRequirement.Installed;
				}
				return ModRequirement.Resolved;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000E9A RID: 3738 RVA: 0x00053138 File Offset: 0x00051338
		public override string Tooltip
		{
			get
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
				if (modWithIdentifier == null)
				{
					return base.Tooltip + "\n" + "ContentNotInstalled".Translate() + "\n\n" + "ModClickToGoToWebsite".Translate();
				}
				if (!modWithIdentifier.Active)
				{
					return base.Tooltip + "\n" + "ContentInstalledButNotActive".Translate() + "\n\n" + "ModClickToSelect".Translate();
				}
				return base.Tooltip;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000E9B RID: 3739 RVA: 0x000531DF File Offset: 0x000513DF
		public string Url
		{
			get
			{
				return this.steamWorkshopUrl ?? this.downloadUrl;
			}
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x000531F4 File Offset: 0x000513F4
		public override void OnClicked(Page_ModsConfig window)
		{
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
			if (modWithIdentifier == null)
			{
				if (!this.Url.NullOrEmpty())
				{
					SteamUtility.OpenUrl(this.Url);
					return;
				}
			}
			else if (!modWithIdentifier.Active)
			{
				window.SelectMod(modWithIdentifier);
			}
		}

		// Token: 0x04000AE5 RID: 2789
		public string downloadUrl;

		// Token: 0x04000AE6 RID: 2790
		public string steamWorkshopUrl;
	}
}
