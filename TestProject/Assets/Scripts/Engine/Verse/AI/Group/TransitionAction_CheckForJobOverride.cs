using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020005E8 RID: 1512
	public class TransitionAction_CheckForJobOverride : TransitionAction
	{
		// Token: 0x060029EB RID: 10731 RVA: 0x000F5C58 File Offset: 0x000F3E58
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (ownedPawns[i].CurJob != null)
				{
					ownedPawns[i].jobs.CheckForJobOverride();
				}
			}
		}
	}
}
