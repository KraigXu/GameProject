using System;

namespace RimWorld
{
	// Token: 0x020007B0 RID: 1968
	public class RaidStrategyWorker_ImmediateAttackFriendly : RaidStrategyWorker_ImmediateAttack
	{
		// Token: 0x06003317 RID: 13079 RVA: 0x0011BA24 File Offset: 0x00119C24
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction != null && !parms.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
