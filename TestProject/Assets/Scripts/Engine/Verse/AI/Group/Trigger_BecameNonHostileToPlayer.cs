using System;
using RimWorld;

namespace Verse.AI.Group
{
	
	public class Trigger_BecameNonHostileToPlayer : Trigger
	{
		
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.FactionRelationsChanged)
			{
				FactionRelationKind? previousRelationKind = signal.previousRelationKind;
				FactionRelationKind factionRelationKind = FactionRelationKind.Hostile;
				return (previousRelationKind.GetValueOrDefault() == factionRelationKind & previousRelationKind != null) && lord.faction != null && !lord.faction.HostileTo(Faction.OfPlayer);
			}
			return false;
		}
	}
}
