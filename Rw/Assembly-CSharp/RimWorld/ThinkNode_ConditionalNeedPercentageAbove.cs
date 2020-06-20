using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007E3 RID: 2019
	public class ThinkNode_ConditionalNeedPercentageAbove : ThinkNode_Conditional
	{
		// Token: 0x060033B9 RID: 13241 RVA: 0x0011DC91 File Offset: 0x0011BE91
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalNeedPercentageAbove thinkNode_ConditionalNeedPercentageAbove = (ThinkNode_ConditionalNeedPercentageAbove)base.DeepCopy(resolve);
			thinkNode_ConditionalNeedPercentageAbove.need = this.need;
			thinkNode_ConditionalNeedPercentageAbove.threshold = this.threshold;
			return thinkNode_ConditionalNeedPercentageAbove;
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x0011DCB7 File Offset: 0x0011BEB7
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.TryGetNeed(this.need).CurLevelPercentage > this.threshold;
		}

		// Token: 0x04001BA9 RID: 7081
		private NeedDef need;

		// Token: 0x04001BAA RID: 7082
		private float threshold;
	}
}
