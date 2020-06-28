using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007F3 RID: 2035
	public class ThinkNode_ConditionalPawnKind : ThinkNode_Conditional
	{
		// Token: 0x060033DC RID: 13276 RVA: 0x0011E048 File Offset: 0x0011C248
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalPawnKind thinkNode_ConditionalPawnKind = (ThinkNode_ConditionalPawnKind)base.DeepCopy(resolve);
			thinkNode_ConditionalPawnKind.pawnKind = this.pawnKind;
			return thinkNode_ConditionalPawnKind;
		}

		// Token: 0x060033DD RID: 13277 RVA: 0x0011E062 File Offset: 0x0011C262
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.kindDef == this.pawnKind;
		}

		// Token: 0x04001BAD RID: 7085
		public PawnKindDef pawnKind;
	}
}
