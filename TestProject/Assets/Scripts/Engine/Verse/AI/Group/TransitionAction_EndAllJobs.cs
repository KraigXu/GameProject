using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020005E2 RID: 1506
	public class TransitionAction_EndAllJobs : TransitionAction
	{
		// Token: 0x060029DF RID: 10719 RVA: 0x000F5A0C File Offset: 0x000F3C0C
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Pawn pawn = ownedPawns[i];
				if (pawn.jobs != null && pawn.jobs.curJob != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}
	}
}
