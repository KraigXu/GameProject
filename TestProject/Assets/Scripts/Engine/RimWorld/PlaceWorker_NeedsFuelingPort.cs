using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001069 RID: 4201
	public class PlaceWorker_NeedsFuelingPort : PlaceWorker
	{
		// Token: 0x060063F5 RID: 25589 RVA: 0x0022A357 File Offset: 0x00228557
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			if (FuelingPortUtility.FuelingPortGiverAtFuelingPortCell(center, map) == null)
			{
				return "MustPlaceNearFuelingPort".Translate();
			}
			return true;
		}

		// Token: 0x060063F6 RID: 25590 RVA: 0x0022A37C File Offset: 0x0022857C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			List<Building> allBuildingsColonist = currentMap.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building building = allBuildingsColonist[i];
				if (building.def.building.hasFuelingPort && !Find.Selector.IsSelected(building) && FuelingPortUtility.GetFuelingPortCell(building).Standable(currentMap))
				{
					PlaceWorker_FuelingPort.DrawFuelingPortCell(building.Position, building.Rotation);
				}
			}
		}
	}
}
