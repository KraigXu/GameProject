using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020001FD RID: 509
	public class ModIncompatibility : ModRequirement
	{
		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000E9E RID: 3742 RVA: 0x00053241 File Offset: 0x00051441
		public override string RequirementTypeLabel
		{
			get
			{
				return "ModIncompatibleWith".Translate("");
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000E9F RID: 3743 RVA: 0x0005325C File Offset: 0x0005145C
		public override bool IsSatisfied
		{
			get
			{
				return ModLister.GetActiveModWithIdentifier(this.packageId) == null;
			}
		}

		// Token: 0x170002EA RID: 746
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

		// Token: 0x06000EA1 RID: 3745 RVA: 0x000532D8 File Offset: 0x000514D8
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
