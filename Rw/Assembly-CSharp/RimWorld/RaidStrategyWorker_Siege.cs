using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007B3 RID: 1971
	public class RaidStrategyWorker_Siege : RaidStrategyWorker
	{
		// Token: 0x0600331F RID: 13087 RVA: 0x0011BAF0 File Offset: 0x00119CF0
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			IntVec3 siegeSpot = RCellFinder.FindSiegePositionFrom_NewTemp(parms.spawnCenter.IsValid ? parms.spawnCenter : pawns[0].PositionHeld, map, false);
			float num = parms.points * Rand.Range(0.2f, 0.3f);
			if (num < 60f)
			{
				num = 60f;
			}
			return new LordJob_Siege(parms.faction, siegeSpot, num);
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x0011BB58 File Offset: 0x00119D58
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canSiege;
		}
	}
}
