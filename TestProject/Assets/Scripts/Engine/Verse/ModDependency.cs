using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class ModDependency : ModRequirement
	{
		
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x000530D5 File Offset: 0x000512D5
		public override string RequirementTypeLabel
		{
			get
			{
				return "ModDependsOn".Translate("");
			}
		}

		
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x000530F0 File Offset: 0x000512F0
		public override bool IsSatisfied
		{
			get
			{
				return ModLister.GetActiveModWithIdentifier(this.packageId) != null;
			}
		}

		
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

		
		// (get) Token: 0x06000E9B RID: 3739 RVA: 0x000531DF File Offset: 0x000513DF
		public string Url
		{
			get
			{
				return this.steamWorkshopUrl ?? this.downloadUrl;
			}
		}

		
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

		
		public string downloadUrl;

		
		public string steamWorkshopUrl;
	}
}
