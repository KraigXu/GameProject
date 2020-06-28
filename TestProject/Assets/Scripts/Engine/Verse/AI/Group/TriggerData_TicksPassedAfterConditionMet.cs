using System;

namespace Verse.AI.Group
{
	// Token: 0x020005F5 RID: 1525
	public class TriggerData_TicksPassedAfterConditionMet : TriggerData_TicksPassed
	{
		// Token: 0x06002A0A RID: 10762 RVA: 0x000F601F File Offset: 0x000F421F
		public override void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.conditionMet, "conditionMet", false, false);
		}

		// Token: 0x0400191E RID: 6430
		public bool conditionMet;
	}
}
