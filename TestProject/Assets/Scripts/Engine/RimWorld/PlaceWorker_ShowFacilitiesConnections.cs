using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001062 RID: 4194
	public class PlaceWorker_ShowFacilitiesConnections : PlaceWorker
	{
		// Token: 0x060063E5 RID: 25573 RVA: 0x0022A1D0 File Offset: 0x002283D0
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			if (def.HasComp(typeof(CompAffectedByFacilities)))
			{
				CompAffectedByFacilities.DrawLinesToPotentialThingsToLinkTo(def, center, rot, currentMap);
				return;
			}
			CompFacility.DrawLinesToPotentialThingsToLinkTo(def, center, rot, currentMap);
		}
	}
}
