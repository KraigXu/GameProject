using System;

namespace Verse.AI.Group
{
	// Token: 0x020005F6 RID: 1526
	public class Trigger_Custom : Trigger
	{
		// Token: 0x06002A0C RID: 10764 RVA: 0x000F603B File Offset: 0x000F423B
		public Trigger_Custom(Func<TriggerSignal, bool> condition)
		{
			this.condition = condition;
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x000F604A File Offset: 0x000F424A
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return this.condition(signal);
		}

		// Token: 0x0400191F RID: 6431
		private Func<TriggerSignal, bool> condition;
	}
}
