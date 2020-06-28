using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001FB RID: 507
	[StaticConstructorOnStartup]
	public abstract class ModRequirement
	{
		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000E90 RID: 3728
		public abstract bool IsSatisfied { get; }

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000E91 RID: 3729
		public abstract string RequirementTypeLabel { get; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x0005304B File Offset: 0x0005124B
		public virtual string Tooltip
		{
			get
			{
				return "ModPackageId".Translate() + ": " + this.packageId;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000E93 RID: 3731 RVA: 0x00053071 File Offset: 0x00051271
		public virtual Texture2D StatusIcon
		{
			get
			{
				if (!this.IsSatisfied)
				{
					return ModRequirement.NotResolved;
				}
				return ModRequirement.Resolved;
			}
		}

		// Token: 0x06000E94 RID: 3732
		public abstract void OnClicked(Page_ModsConfig window);

		// Token: 0x04000ADF RID: 2783
		public string packageId;

		// Token: 0x04000AE0 RID: 2784
		public string displayName;

		// Token: 0x04000AE1 RID: 2785
		public static Texture2D NotResolved = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/NotResolved", true);

		// Token: 0x04000AE2 RID: 2786
		public static Texture2D NotInstalled = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/NotInstalled", true);

		// Token: 0x04000AE3 RID: 2787
		public static Texture2D Installed = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/Installed", true);

		// Token: 0x04000AE4 RID: 2788
		public static Texture2D Resolved = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOn", true);
	}
}
