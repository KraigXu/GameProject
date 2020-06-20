using System;

namespace Verse.AI.Group
{
	// Token: 0x020005F1 RID: 1521
	public class Trigger_TicksPassedAndNoRecentHarm : Trigger_TicksPassed
	{
		// Token: 0x06002A01 RID: 10753 RVA: 0x000F5EF8 File Offset: 0x000F40F8
		public Trigger_TicksPassedAndNoRecentHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x000F5F01 File Offset: 0x000F4101
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return base.ActivateOn(lord, signal) && Find.TickManager.TicksGame - lord.lastPawnHarmTick >= 300;
		}

		// Token: 0x0400191A RID: 6426
		private const int MinTicksSinceDamage = 300;
	}
}
