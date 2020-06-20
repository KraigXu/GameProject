using System;

namespace Verse.AI.Group
{
	// Token: 0x020005F4 RID: 1524
	public class Trigger_TicksPassedAfterConditionMet : Trigger_TicksPassed
	{
		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06002A07 RID: 10759 RVA: 0x000F5F8C File Offset: 0x000F418C
		protected new TriggerData_TicksPassedAfterConditionMet Data
		{
			get
			{
				return (TriggerData_TicksPassedAfterConditionMet)this.data;
			}
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x000F5F99 File Offset: 0x000F4199
		public Trigger_TicksPassedAfterConditionMet(int tickLimit, Func<bool> condition, int checkEveryTicks = 1) : base(tickLimit)
		{
			this.condition = condition;
			this.checkEveryTicks = checkEveryTicks;
			this.data = new TriggerData_TicksPassedAfterConditionMet();
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x000F5FBC File Offset: 0x000F41BC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (!this.Data.conditionMet && signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.checkEveryTicks == 0)
			{
				this.Data.conditionMet = this.condition();
			}
			return this.Data.conditionMet && base.ActivateOn(lord, signal);
		}

		// Token: 0x0400191C RID: 6428
		private Func<bool> condition;

		// Token: 0x0400191D RID: 6429
		private int checkEveryTicks;
	}
}
