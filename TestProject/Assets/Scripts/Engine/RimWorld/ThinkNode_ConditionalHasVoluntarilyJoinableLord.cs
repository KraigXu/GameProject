using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007D4 RID: 2004
	public class ThinkNode_ConditionalHasVoluntarilyJoinableLord : ThinkNode_Conditional
	{
		// Token: 0x0600339A RID: 13210 RVA: 0x0011DA8C File Offset: 0x0011BC8C
		protected override bool Satisfied(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && lord.LordJob is LordJob_VoluntarilyJoinable;
		}
	}
}
