using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	[StaticConstructorOnStartup]
	public abstract class ModRequirement
	{
		
		
		public abstract bool IsSatisfied { get; }

		
		
		public abstract string RequirementTypeLabel { get; }

		
		
		public virtual string Tooltip
		{
			get
			{
				return "ModPackageId".Translate() + ": " + this.packageId;
			}
		}

		
		
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
