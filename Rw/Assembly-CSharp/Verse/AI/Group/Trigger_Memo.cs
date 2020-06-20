using System;

namespace Verse.AI.Group
{
	// Token: 0x0200060B RID: 1547
	public class Trigger_Memo : Trigger
	{
		// Token: 0x06002A38 RID: 10808 RVA: 0x000F6751 File Offset: 0x000F4951
		public Trigger_Memo(string memo)
		{
			this.memo = memo;
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x000F6760 File Offset: 0x000F4960
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Memo && signal.memo == this.memo;
		}

		// Token: 0x04001931 RID: 6449
		private string memo;
	}
}
