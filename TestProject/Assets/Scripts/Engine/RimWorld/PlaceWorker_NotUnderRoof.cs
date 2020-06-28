using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001051 RID: 4177
	public class PlaceWorker_NotUnderRoof : PlaceWorker
	{
		// Token: 0x060063BD RID: 25533 RVA: 0x002297C0 File Offset: 0x002279C0
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			if (map.roofGrid.Roofed(loc))
			{
				return new AcceptanceReport("MustPlaceUnroofed".Translate());
			}
			return true;
		}
	}
}
