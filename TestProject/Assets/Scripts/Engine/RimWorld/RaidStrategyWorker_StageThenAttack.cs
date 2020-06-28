using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007B2 RID: 1970
	public class RaidStrategyWorker_StageThenAttack : RaidStrategyWorker
	{
		// Token: 0x0600331C RID: 13084 RVA: 0x0011BA8C File Offset: 0x00119C8C
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			IntVec3 stageLoc = RCellFinder.FindSiegePositionFrom_NewTemp(parms.spawnCenter.IsValid ? parms.spawnCenter : pawns[0].PositionHeld, map, false);
			return new LordJob_StageThenAttack(parms.faction, stageLoc, raidSeed);
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x0011BAD0 File Offset: 0x00119CD0
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canStageAttacks;
		}
	}
}
