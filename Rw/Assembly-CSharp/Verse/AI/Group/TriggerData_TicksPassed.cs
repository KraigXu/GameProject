using System;

namespace Verse.AI.Group
{
	// Token: 0x020005F0 RID: 1520
	public class TriggerData_TicksPassed : TriggerData
	{
		// Token: 0x060029FF RID: 10751 RVA: 0x000F5EDC File Offset: 0x000F40DC
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x04001919 RID: 6425
		public int ticksPassed;
	}
}
