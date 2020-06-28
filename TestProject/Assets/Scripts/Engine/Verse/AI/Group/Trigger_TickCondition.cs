using System;

namespace Verse.AI.Group
{
	// Token: 0x020005F7 RID: 1527
	public class Trigger_TickCondition : Trigger
	{
		// Token: 0x06002A0E RID: 10766 RVA: 0x000F6058 File Offset: 0x000F4258
		public Trigger_TickCondition(Func<bool> condition, int checkEveryTicks = 1)
		{
			this.condition = condition;
			this.checkEveryTicks = checkEveryTicks;
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x000F6075 File Offset: 0x000F4275
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.checkEveryTicks == 0 && this.condition();
		}

		// Token: 0x04001920 RID: 6432
		private Func<bool> condition;

		// Token: 0x04001921 RID: 6433
		private int checkEveryTicks = 1;
	}
}
