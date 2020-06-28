using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200106E RID: 4206
	public class PlaceWorker_ShipLandingBeacon : PlaceWorker
	{
		// Token: 0x06006404 RID: 25604 RVA: 0x0022A93C File Offset: 0x00228B3C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			if (def.HasComp(typeof(CompShipLandingBeacon)))
			{
				ShipLandingBeaconUtility.DrawLinesToNearbyBeacons(def, center, rot, currentMap, thing);
			}
		}
	}
}
