using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D5 RID: 2005
	public class ThinkNode_ConditionalDrafted : ThinkNode_Conditional
	{
		// Token: 0x0600339C RID: 13212 RVA: 0x0011DAB3 File Offset: 0x0011BCB3
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
