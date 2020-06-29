using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	[StaticConstructorOnStartup]
	public abstract class ModRequirement
	{
		
		// (get) Token: 0x06000E90 RID: 3728
		public abstract bool IsSatisfied { get; }

		
		// (get) Token: 0x06000E91 RID: 3729
		public abstract string RequirementTypeLabel { get; }

		
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x0005304B File Offset: 0x0005124B
		public virtual string Tooltip
		{
			get
			{
				return "ModPackageId".Translate() + ": " + this.packageId;
			}
		}

		
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

		
		public abstract void OnClicked(Page_ModsConfig window);

		
		public string packageId;

		
		public string displayName;

		
		public static Texture2D NotResolved = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/NotResolved", true);

		
		public static Texture2D NotInstalled = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/NotInstalled", true);

		
		public static Texture2D Installed = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/Installed", true);

		
		public static Texture2D Resolved = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOn", true);
	}
}
