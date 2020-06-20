using System;

namespace Verse.AI.Group
{
	// Token: 0x0200060D RID: 1549
	public class Trigger_ThingDamageTaken : Trigger
	{
		// Token: 0x06002A3C RID: 10812 RVA: 0x000F6802 File Offset: 0x000F4A02
		public Trigger_ThingDamageTaken(Thing thing, float damageFraction)
		{
			this.thing = thing;
			this.damageFraction = damageFraction;
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x000F6824 File Offset: 0x000F4A24
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && (this.thing.DestroyedOrNull() || (float)this.thing.HitPoints < (1f - this.damageFraction) * (float)this.thing.MaxHitPoints);
		}

		// Token: 0x04001933 RID: 6451
		private Thing thing;

		// Token: 0x04001934 RID: 6452
		private float damageFraction = 0.5f;
	}
}
