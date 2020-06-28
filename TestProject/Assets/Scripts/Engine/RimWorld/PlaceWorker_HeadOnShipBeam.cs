using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001054 RID: 4180
	public class PlaceWorker_HeadOnShipBeam : PlaceWorker
	{
		// Token: 0x060063C4 RID: 25540 RVA: 0x002298D8 File Offset: 0x00227AD8
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			IntVec3 c = loc + rot.FacingCell * -1;
			if (!c.InBounds(map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			if (edifice == null || edifice.def != ThingDefOf.Ship_Beam)
			{
				return "MustPlaceHeadOnShipBeam".Translate();
			}
			return true;
		}
	}
}
