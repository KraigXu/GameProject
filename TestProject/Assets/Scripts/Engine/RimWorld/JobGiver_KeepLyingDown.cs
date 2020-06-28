using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006E0 RID: 1760
	public class JobGiver_KeepLyingDown : ThinkNode_JobGiver
	{
		// Token: 0x06002EDE RID: 11998 RVA: 0x001075A5 File Offset: 0x001057A5
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.GetPosture().Laying())
			{
				return pawn.CurJob;
			}
			return JobMaker.MakeJob(JobDefOf.LayDown, pawn.Position);
		}
	}
}
