using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class ModDependency : ModRequirement
	{
		
		
		public override string RequirementTypeLabel
		{
			get
			{
				return "ModDependsOn".Translate("");
			}
		}

		
		
		public override bool IsSatisfied
		{
			get
			{
				return ModLister.GetActiveModWithIdentifier(this.packageId) != null;
			}
		}

		
		
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
