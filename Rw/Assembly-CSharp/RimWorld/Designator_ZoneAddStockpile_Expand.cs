using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E44 RID: 3652
	public class Designator_ZoneAddStockpile_Expand : Designator_ZoneAddStockpile_Resources
	{
		// Token: 0x0600585A RID: 22618 RVA: 0x001D5275 File Offset: 0x001D3475
		public Designator_ZoneAddStockpile_Expand()
		{
			this.defaultLabel = "DesignatorZoneExpand".Translate();
			this.hotKey = KeyBindingDefOf.Misc6;
		}
	}
}
