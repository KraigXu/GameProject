using System;

namespace Verse.AI
{
	// Token: 0x02000590 RID: 1424
	public class ThinkNode_ConditionalNoTarget : ThinkNode_Conditional
	{
		// Token: 0x06002867 RID: 10343 RVA: 0x000EEA5B File Offset: 0x000ECC5B
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.enemyTarget == null;
		}
	}
}
