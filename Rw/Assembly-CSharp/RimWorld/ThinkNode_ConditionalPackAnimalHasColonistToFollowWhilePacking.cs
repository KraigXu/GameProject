using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007F5 RID: 2037
	public class ThinkNode_ConditionalPackAnimalHasColonistToFollowWhilePacking : ThinkNode_Conditional
	{
		// Token: 0x060033E1 RID: 13281 RVA: 0x0011E07A File Offset: 0x0011C27A
		protected override bool Satisfied(Pawn pawn)
		{
			return JobGiver_PackAnimalFollowColonists.GetPawnToFollow(pawn) != null;
		}
	}
}
