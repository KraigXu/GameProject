using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007E0 RID: 2016
	public class ThinkNode_ConditionalStarving : ThinkNode_Conditional
	{
		// Token: 0x060033B3 RID: 13235 RVA: 0x0011DC57 File Offset: 0x0011BE57
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.food != null && pawn.needs.food.CurCategory >= HungerCategory.Starving;
		}
	}
}
