using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007EC RID: 2028
	public class ThinkNode_ConditionalRandom : ThinkNode_Conditional
	{
		// Token: 0x060033CD RID: 13261 RVA: 0x0011DEB1 File Offset: 0x0011C0B1
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalRandom thinkNode_ConditionalRandom = (ThinkNode_ConditionalRandom)base.DeepCopy(resolve);
			thinkNode_ConditionalRandom.chance = this.chance;
			return thinkNode_ConditionalRandom;
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x0011DECB File Offset: 0x0011C0CB
		protected override bool Satisfied(Pawn pawn)
		{
			return Rand.Value < this.chance;
		}

		// Token: 0x04001BAC RID: 7084
		public float chance = 0.5f;
	}
}
