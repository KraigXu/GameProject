using System;

namespace Verse.AI.Group
{
	// Token: 0x020005F2 RID: 1522
	public class Trigger_TicksPassedWithoutHarm : Trigger_TicksPassed
	{
		// Token: 0x06002A03 RID: 10755 RVA: 0x000F5EF8 File Offset: 0x000F40F8
		public Trigger_TicksPassedWithoutHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x000F5F28 File Offset: 0x000F4128
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (Trigger_PawnHarmed.SignalIsHarm(signal))
			{
				base.Data.ticksPassed = 0;
			}
			return base.ActivateOn(lord, signal);
		}
	}
}
