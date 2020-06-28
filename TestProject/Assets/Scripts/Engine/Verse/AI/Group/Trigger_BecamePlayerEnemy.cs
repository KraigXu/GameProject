using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200060A RID: 1546
	public class Trigger_BecamePlayerEnemy : Trigger
	{
		// Token: 0x06002A36 RID: 10806 RVA: 0x000F672A File Offset: 0x000F492A
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.FactionRelationsChanged && lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
