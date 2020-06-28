using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007E8 RID: 2024
	public class ThinkNode_ConditionalCloseToDutyTarget : ThinkNode_Conditional
	{
		// Token: 0x060033C4 RID: 13252 RVA: 0x0011DD83 File Offset: 0x0011BF83
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalCloseToDutyTarget thinkNode_ConditionalCloseToDutyTarget = (ThinkNode_ConditionalCloseToDutyTarget)base.DeepCopy(resolve);
			thinkNode_ConditionalCloseToDutyTarget.maxDistToDutyTarget = this.maxDistToDutyTarget;
			return thinkNode_ConditionalCloseToDutyTarget;
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x0011DDA0 File Offset: 0x0011BFA0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty.focus.IsValid && pawn.Position.InHorDistOf(pawn.mindState.duty.focus.Cell, this.maxDistToDutyTarget);
		}

		// Token: 0x04001BAB RID: 7083
		private float maxDistToDutyTarget = 10f;
	}
}
