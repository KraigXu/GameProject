using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007B1 RID: 1969
	public class RaidStrategyWorker_ImmediateAttackSmart : RaidStrategyWorker
	{
		// Token: 0x06003319 RID: 13081 RVA: 0x0011BA57 File Offset: 0x00119C57
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return new LordJob_AssaultColony(parms.faction, true, true, false, true, true);
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x0011BA69 File Offset: 0x00119C69
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canUseAvoidGrid;
		}
	}
}
