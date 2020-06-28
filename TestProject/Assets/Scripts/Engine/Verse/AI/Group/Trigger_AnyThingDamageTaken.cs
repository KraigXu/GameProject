using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x0200060E RID: 1550
	public class Trigger_AnyThingDamageTaken : Trigger
	{
		// Token: 0x06002A3E RID: 10814 RVA: 0x000F6872 File Offset: 0x000F4A72
		public Trigger_AnyThingDamageTaken(List<Thing> things, float damageFraction)
		{
			this.things = things;
			this.damageFraction = damageFraction;
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x000F6894 File Offset: 0x000F4A94
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				foreach (Thing thing in this.things)
				{
					if (thing.DestroyedOrNull() || (float)thing.HitPoints < (1f - this.damageFraction) * (float)thing.MaxHitPoints)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x04001935 RID: 6453
		private List<Thing> things;

		// Token: 0x04001936 RID: 6454
		private float damageFraction = 0.5f;
	}
}
