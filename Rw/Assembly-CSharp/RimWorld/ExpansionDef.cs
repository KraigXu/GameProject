using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BC RID: 2236
	public class ExpansionDef : Def
	{
		// Token: 0x17000999 RID: 2457
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

		// Token: 0x1700099A RID: 2458
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

		// Token: 0x1700099B RID: 2459
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

		// Token: 0x1700099C RID: 2460
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

		// Token: 0x1700099D RID: 2461
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

		// Token: 0x060035E0 RID: 13792 RVA: 0x00124D84 File Offset: 0x00122F84
		public override void PostLoad()
		{
			base.PostLoad();
			this.linkedMod = this.linkedMod.ToLower();
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x00124D9D File Offset: 0x00122F9D
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
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

		// Token: 0x04001DC8 RID: 7624
		[NoTranslate]
		public string iconPath;

		// Token: 0x04001DC9 RID: 7625
		[NoTranslate]
		public string backgroundPath;

		// Token: 0x04001DCA RID: 7626
		[NoTranslate]
		public string linkedMod;

		// Token: 0x04001DCB RID: 7627
		[NoTranslate]
		public string steamUrl;

		// Token: 0x04001DCC RID: 7628
		[NoTranslate]
		public string siteUrl;

		// Token: 0x04001DCD RID: 7629
		public bool isCore;

		// Token: 0x04001DCE RID: 7630
		private Texture2D cachedIcon;

		// Token: 0x04001DCF RID: 7631
		private Texture2D cachedBG;
	}
}
