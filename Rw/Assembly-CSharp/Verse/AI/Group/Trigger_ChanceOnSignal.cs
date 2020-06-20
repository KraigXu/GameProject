using System;

namespace Verse.AI.Group
{
	// Token: 0x020005FA RID: 1530
	public class Trigger_ChanceOnSignal : Trigger
	{
		// Token: 0x06002A14 RID: 10772 RVA: 0x000F6115 File Offset: 0x000F4315
		public Trigger_ChanceOnSignal(TriggerSignalType signalType, float chance)
		{
			this.signalType = signalType;
			this.chance = chance;
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x000F612B File Offset: 0x000F432B
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == this.signalType && Rand.Value < this.chance;
		}

		// Token: 0x04001924 RID: 6436
		private TriggerSignalType signalType;

		// Token: 0x04001925 RID: 6437
		private float chance;
	}
}
