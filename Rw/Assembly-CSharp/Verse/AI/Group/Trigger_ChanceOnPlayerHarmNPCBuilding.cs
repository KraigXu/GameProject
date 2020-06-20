using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020005FF RID: 1535
	public class Trigger_ChanceOnPlayerHarmNPCBuilding : Trigger
	{
		// Token: 0x06002A20 RID: 10784 RVA: 0x000F632D File Offset: 0x000F452D
		public Trigger_ChanceOnPlayerHarmNPCBuilding(float chance)
		{
			this.chance = chance;
		}

		// Token: 0x06002A21 RID: 10785 RVA: 0x000F6348 File Offset: 0x000F4548
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.BuildingDamaged && signal.dinfo.Def.ExternalViolenceFor(signal.thing) && signal.thing.def.category == ThingCategory.Building && signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == Faction.OfPlayer && signal.thing.Faction != Faction.OfPlayer && Rand.Value < this.chance;
		}

		// Token: 0x0400192C RID: 6444
		private float chance = 1f;
	}
}
