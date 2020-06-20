using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000609 RID: 1545
	public class Trigger_BecameNonHostileToPlayer : Trigger
	{
		// Token: 0x06002A34 RID: 10804 RVA: 0x000F66D8 File Offset: 0x000F48D8
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
