using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000592 RID: 1426
	public class ThinkNode_ConditionalRequireCapacities : ThinkNode_Conditional
	{
		// Token: 0x0600286C RID: 10348 RVA: 0x000EEA96 File Offset: 0x000ECC96
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalRequireCapacities thinkNode_ConditionalRequireCapacities = (ThinkNode_ConditionalRequireCapacities)base.DeepCopy(resolve);
			thinkNode_ConditionalRequireCapacities.requiredCapacities = this.requiredCapacities;
			return thinkNode_ConditionalRequireCapacities;
		}

		// Token: 0x0600286D RID: 10349 RVA: 0x000EEAB0 File Offset: 0x000ECCB0
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.health != null && pawn.health.capacities != null)
			{
				foreach (PawnCapacityDef capacity in this.requiredCapacities)
				{
					if (!pawn.health.capacities.CapableOf(capacity))
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x04001843 RID: 6211
		public List<PawnCapacityDef> requiredCapacities;
	}
}
