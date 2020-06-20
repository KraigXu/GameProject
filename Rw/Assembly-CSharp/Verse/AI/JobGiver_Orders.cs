using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005A8 RID: 1448
	public class JobGiver_Orders : ThinkNode_JobGiver
	{
		// Token: 0x060028A6 RID: 10406 RVA: 0x000EF647 File Offset: 0x000ED847
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Drafted)
			{
				return JobMaker.MakeJob(JobDefOf.Wait_Combat, pawn.Position);
			}
			return null;
		}
	}
}
