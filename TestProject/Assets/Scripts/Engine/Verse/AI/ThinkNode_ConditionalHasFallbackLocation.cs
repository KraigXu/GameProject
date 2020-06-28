using System;

namespace Verse.AI
{
	// Token: 0x02000593 RID: 1427
	public class ThinkNode_ConditionalHasFallbackLocation : ThinkNode_Conditional
	{
		// Token: 0x0600286F RID: 10351 RVA: 0x000EEB2C File Offset: 0x000ECD2C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focusSecond.IsValid;
		}
	}
}
