using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B25 RID: 2853
	public class PawnsArrivalModeWorker_EdgeDrop : PawnsArrivalModeWorker
	{
		// Token: 0x06004317 RID: 17175 RVA: 0x0016941F File Offset: 0x0016761F
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			PawnsArrivalModeWorkerUtility.DropInDropPodsNearSpawnCenter(parms, pawns);
		}

		// Token: 0x06004318 RID: 17176 RVA: 0x001695A0 File Offset: 0x001677A0
		public override void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			IntVec3 near = DropCellFinder.FindRaidDropCenterDistant_NewTemp(map, false);
			TransportPodsArrivalActionUtility.DropTravelingTransportPods(dropPods, near, map);
		}

		// Token: 0x06004319 RID: 17177 RVA: 0x001695C0 File Offset: 0x001677C0
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!parms.spawnCenter.IsValid)
			{
				parms.spawnCenter = DropCellFinder.FindRaidDropCenterDistant_NewTemp(map, false);
			}
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
