using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class ExpansionDef : Def
	{
		
		
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					this.cachedIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.cachedIcon;
			}
		}

		
		
		public Texture2D BackgroundImage
		{
			get
			{
				if (this.cachedBG == null)
				{
					Debug.Log(this.backgroundPath);
					this.cachedBG = ContentFinder<Texture2D>.Get(this.backgroundPath, true);
				}
				return this.cachedBG;
			}
		}

		
		
		public string StoreURL
		{
			get
			{
				if (!this.steamUrl.NullOrEmpty())
				{
					return this.steamUrl;
				}
				return this.siteUrl;
			}
		}

		
		
		public ExpansionStatus Status
		{
			get
			{
				if (ModsConfig.IsActive(this.linkedMod))
				{
					return ExpansionStatus.Active;
				}
				if (ModLister.AllInstalledMods.Any((ModMetaData m) => m.SamePackageId(this.linkedMod, false)))
				{
					return ExpansionStatus.Installed;
				}
				return ExpansionStatus.NotInstalled;
			}
		}

		
		
		public string StatusDescription
		{
			get
			{
				ExpansionStatus status = this.Status;
				if (status == ExpansionStatus.Active)
				{
					return "ContentActive".Translate();
				}
				if (status != ExpansionStatus.Installed)
				{
					return "ContentNotInstalled".Translate();
				}
				return "ContentInstalledButNotActive".Translate();
			}
		}

		
		public override void PostLoad()
		{
			base.PostLoad();
			this.linkedMod = this.linkedMod.ToLower();
		}

		
		public override IEnumerable<string> ConfigErrors()
		{

			{
				
			}
			IEnumerator<string> enumerator = null;
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.linkedMod, false);
			if (modWithIdentifier != null && !modWithIdentifier.Official)
			{
				yield return modWithIdentifier.Name + " - ExpansionDefs are used for official content. For mods, you should define ModMetaData in About.xml.";
			}
			yield break;
			yield break;
		}

		
		[NoTranslate]
		public string iconPath;

		
		[NoTranslate]
		public string backgroundPath;

		
		[NoTranslate]
		public string linkedMod;

		
		[NoTranslate]
		public string steamUrl;

		
		[NoTranslate]
		public string siteUrl;

		
		public bool isCore;

		
		private Texture2D cachedIcon;

		
		private Texture2D cachedBG;
	}
}
