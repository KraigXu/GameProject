using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B26 RID: 2854
	public class PawnsArrivalModeWorker_EdgeDropGroups : PawnsArrivalModeWorker
	{
		// Token: 0x0600431B RID: 17179 RVA: 0x00169600 File Offset: 0x00167800
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			bool canRoofPunch = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);
			List<Pair<List<Pawn>, IntVec3>> list = PawnsArrivalModeWorkerUtility.SplitIntoRandomGroupsNearMapEdge(pawns, map, true);
			PawnsArrivalModeWorkerUtility.SetPawnGroupsInfo(parms, list);
			for (int i = 0; i < list.Count; i++)
			{
				DropPodUtility.DropThingsNear(list[i].Second, map, list[i].First.Cast<Thing>(), parms.podOpenDelay, false, true, canRoofPunch, true);
			}
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x0016968B File Offset: 0x0016788B
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
