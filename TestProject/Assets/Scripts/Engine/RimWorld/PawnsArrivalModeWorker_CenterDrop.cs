using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B23 RID: 2851
	public class PawnsArrivalModeWorker_CenterDrop : PawnsArrivalModeWorker
	{
		// Token: 0x0600430F RID: 17167 RVA: 0x0016941F File Offset: 0x0016761F
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			PawnsArrivalModeWorkerUtility.DropInDropPodsNearSpawnCenter(parms, pawns);
		}

		// Token: 0x06004310 RID: 17168 RVA: 0x00169428 File Offset: 0x00167628
		public override void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			IntVec3 near;
			if (!DropCellFinder.TryFindRaidDropCenterClose(out near, map, true, true, true, -1))
			{
				near = DropCellFinder.FindRaidDropCenterDistant_NewTemp(map, false);
			}
			TransportPodsArrivalActionUtility.DropTravelingTransportPods(dropPods, near, map);
		}

		// Token: 0x06004311 RID: 17169 RVA: 0x00169454 File Offset: 0x00167654
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!parms.raidArrivalModeForQuickMilitaryAid)
			{
				parms.podOpenDelay = 520;
			}
			parms.spawnRotation = Rot4.Random;
			if (!parms.spawnCenter.IsValid)
			{
				bool flag = parms.faction == Faction.OfMechanoids;
				bool flag2 = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);
				if (Rand.Chance(0.4f) && !flag && map.listerBuildings.ColonistsHaveBuildingWithPowerOn(ThingDefOf.OrbitalTradeBeacon))
				{
					parms.spawnCenter = DropCellFinder.TradeDropSpot(map);
				}
				else if (!DropCellFinder.TryFindRaidDropCenterClose(out parms.spawnCenter, map, !flag && flag2, !flag, true, -1))
				{
					parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeDrop;
					return parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms);
				}
			}
			return true;
		}

		// Token: 0x04002690 RID: 9872
		public const int PodOpenDelay = 520;
	}
}
