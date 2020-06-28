using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI.Group
{
	// Token: 0x020005F3 RID: 1523
	public class Trigger_TicksPassedWithoutHarmOrMemos : Trigger_TicksPassed
	{
		// Token: 0x06002A05 RID: 10757 RVA: 0x000F5F46 File Offset: 0x000F4146
		public Trigger_TicksPassedWithoutHarmOrMemos(int tickLimit, params string[] memos) : base(tickLimit)
		{
			this.memos = memos.ToList<string>();
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x000F5F5B File Offset: 0x000F415B
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (Trigger_PawnHarmed.SignalIsHarm(signal) || this.memos.Contains(signal.memo))
			{
				base.Data.ticksPassed = 0;
			}
			return base.ActivateOn(lord, signal);
		}

		// Token: 0x0400191B RID: 6427
		private List<string> memos;
	}
}
