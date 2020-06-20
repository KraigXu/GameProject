using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B24 RID: 2852
	public class PawnsArrivalModeWorker_ClusterDrop : PawnsArrivalModeWorker
	{
		// Token: 0x06004313 RID: 17171 RVA: 0x00002681 File Offset: 0x00000881
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x00169534 File Offset: 0x00167734
		public override void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			IntVec3 near = DropCellFinder.FindRaidDropCenterDistant_NewTemp(map, false);
			TransportPodsArrivalActionUtility.DropTravelingTransportPods(dropPods, near, map);
		}

		// Token: 0x06004315 RID: 17173 RVA: 0x00169554 File Offset: 0x00167754
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!parms.spawnCenter.IsValid)
			{
				parms.spawnCenter = MechClusterUtility.FindClusterPosition(map, parms.mechClusterSketch, 100, 0.5f);
			}
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
