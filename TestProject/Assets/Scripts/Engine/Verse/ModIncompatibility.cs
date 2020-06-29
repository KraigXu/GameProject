using System;
using RimWorld;

namespace Verse
{
	
	public class ModIncompatibility : ModRequirement
	{
		
		// (get) Token: 0x06000E9E RID: 3742 RVA: 0x00053241 File Offset: 0x00051441
		public override string RequirementTypeLabel
		{
			get
			{
				return "ModIncompatibleWith".Translate("");
			}
		}

		
		// (get) Token: 0x06000E9F RID: 3743 RVA: 0x0005325C File Offset: 0x0005145C
		public override bool IsSatisfied
		{
			get
			{
				return ModLister.GetActiveModWithIdentifier(this.packageId) == null;
			}
		}

		
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x0005326C File Offset: 0x0005146C
		public override string Tooltip
		{
			get
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
				if (modWithIdentifier != null && modWithIdentifier.Active)
				{
					return base.Tooltip + "\n" + "ContentActive".Translate() + "\n\n" + "ModClickToSelect".Translate();
				}
				return base.Tooltip;
			}
		}

		
		public override void OnClicked(Page_ModsConfig window)
		{
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
			if (modWithIdentifier != null && modWithIdentifier.Active)
			{
				window.SelectMod(modWithIdentifier);
			}
		}
	}
}
