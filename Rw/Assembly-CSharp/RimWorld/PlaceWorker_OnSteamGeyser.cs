using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001052 RID: 4178
	public class PlaceWorker_OnSteamGeyser : PlaceWorker
	{
		// Token: 0x060063BF RID: 25535 RVA: 0x002297EC File Offset: 0x002279EC
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			Thing thing2 = map.thingGrid.ThingAt(loc, ThingDefOf.SteamGeyser);
			if (thing2 == null || thing2.Position != loc)
			{
				return "MustPlaceOnSteamGeyser".Translate();
			}
			return true;
		}

		// Token: 0x060063C0 RID: 25536 RVA: 0x00229833 File Offset: 0x00227A33
		public override bool ForceAllowPlaceOver(BuildableDef otherDef)
		{
			return otherDef == ThingDefOf.SteamGeyser;
		}
	}
}
