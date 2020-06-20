using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005A5 RID: 1445
	public class JobGiver_IdleForever : ThinkNode_JobGiver
	{
		// Token: 0x0600289F RID: 10399 RVA: 0x000EF529 File Offset: 0x000ED729
		protected override Job TryGiveJob(Pawn pawn)
		{
			return JobMaker.MakeJob(JobDefOf.Wait_Downed);
		}
	}
}
