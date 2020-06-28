using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001063 RID: 4195
	public class PlaceWorker_ShowTradeBeaconRadius : PlaceWorker
	{
		// Token: 0x060063E7 RID: 25575 RVA: 0x0022A208 File Offset: 0x00228408
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			GenDraw.DrawFieldEdges(Building_OrbitalTradeBeacon.TradeableCellsAround(center, currentMap));
		}
	}
}
