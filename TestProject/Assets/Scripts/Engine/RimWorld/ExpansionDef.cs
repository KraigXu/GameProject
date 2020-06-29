using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class ExpansionDef : Def
	{
		
		// (get) Token: 0x060035DB RID: 13787 RVA: 0x00124C9F File Offset: 0x00122E9F
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

		
		// (get) Token: 0x060035DC RID: 13788 RVA: 0x00124CC7 File Offset: 0x00122EC7
		public Texture2D BackgroundImage
		{
			get
			{
				if (this.cachedBG == null)
				{
					this.cachedBG = ContentFinder<Texture2D>.Get(this.backgroundPath, true);
				}
				return this.cachedBG;
			}
		}

		
		// (get) Token: 0x060035DD RID: 13789 RVA: 0x00124CEF File Offset: 0x00122EEF
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

		
		// (get) Token: 0x060035DE RID: 13790 RVA: 0x00124D0B File Offset: 0x00122F0B
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

		
		// (get) Token: 0x060035DF RID: 13791 RVA: 0x00124D38 File Offset: 0x00122F38
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
			foreach (string text in this.n__0())
			{
				yield return text;
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
