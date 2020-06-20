using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020005E4 RID: 1508
	public class TransitionAction_WakeAll : TransitionAction
	{
		// Token: 0x060029E3 RID: 10723 RVA: 0x000F5B00 File Offset: 0x000F3D00
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				RestUtility.WakeUp(ownedPawns[i]);
			}
		}
	}
}
