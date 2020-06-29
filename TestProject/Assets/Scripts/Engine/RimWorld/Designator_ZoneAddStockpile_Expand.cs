using System;
using Verse;

namespace RimWorld
{
	
	public class Designator_ZoneAddStockpile_Expand : Designator_ZoneAddStockpile_Resources
	{
		
		public Designator_ZoneAddStockpile_Expand()
		{
			this.defaultLabel = "DesignatorZoneExpand".Translate();
			this.hotKey = KeyBindingDefOf.Misc6;
		}
	}
}
