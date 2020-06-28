using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B29 RID: 2857
	public class PawnsArrivalModeWorker_RandomDrop : PawnsArrivalModeWorker
	{
		// Token: 0x06004324 RID: 17188 RVA: 0x001697EC File Offset: 0x001679EC
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			bool canRoofPunch = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);
			for (int i = 0; i < pawns.Count; i++)
			{
				DropPodUtility.DropThingsNear(DropCellFinder.RandomDropSpot(map), map, Gen.YieldSingle<Thing>(pawns[i]), parms.podOpenDelay, false, true, canRoofPunch, true);
			}
		}

		// Token: 0x06004325 RID: 17189 RVA: 0x00169854 File Offset: 0x00167A54
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			if (!parms.raidArrivalModeForQuickMilitaryAid)
			{
				parms.podOpenDelay = 520;
			}
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
