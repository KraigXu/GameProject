using System;

namespace Verse.AI.Group
{
	// Token: 0x020005FC RID: 1532
	public class Trigger_ChanceOnTickInteval : Trigger
	{
		// Token: 0x06002A18 RID: 10776 RVA: 0x000F6175 File Offset: 0x000F4375
		public Trigger_ChanceOnTickInteval(int interval, float chancePerInterval)
		{
			this.chancePerInterval = chancePerInterval;
			this.interval = interval;
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x000F618B File Offset: 0x000F438B
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.interval == 0 && Rand.Value < this.chancePerInterval;
		}

		// Token: 0x04001927 RID: 6439
		private float chancePerInterval;

		// Token: 0x04001928 RID: 6440
		private int interval;
	}
}
