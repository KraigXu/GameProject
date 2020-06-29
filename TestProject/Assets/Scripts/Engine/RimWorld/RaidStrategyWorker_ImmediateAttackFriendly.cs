using System;

namespace RimWorld
{
	
	public class RaidStrategyWorker_ImmediateAttackFriendly : RaidStrategyWorker_ImmediateAttack
	{
		
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction != null && !parms.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
