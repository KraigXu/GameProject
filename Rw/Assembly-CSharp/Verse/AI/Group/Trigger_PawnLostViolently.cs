using System;

namespace Verse.AI.Group
{
	// Token: 0x02000604 RID: 1540
	public class Trigger_PawnLostViolently : Trigger
	{
		// Token: 0x06002A2A RID: 10794 RVA: 0x000F6592 File Offset: 0x000F4792
		public Trigger_PawnLostViolently(bool allowRoofCollapse = true)
		{
			this.allowRoofCollapse = allowRoofCollapse;
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x000F65A1 File Offset: 0x000F47A1
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnLost)
			{
				if (signal.condition == PawnLostCondition.MadePrisoner)
				{
					return true;
				}
				if (signal.condition == PawnLostCondition.IncappedOrKilled && (signal.dinfo.Category != DamageInfo.SourceCategory.Collapse || this.allowRoofCollapse))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001930 RID: 6448
		public bool allowRoofCollapse;
	}
}
