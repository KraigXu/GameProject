using System;

namespace Verse.AI.Group
{
	// Token: 0x020005E6 RID: 1510
	public class TransitionAction_SetDefendLocalGroup : TransitionAction
	{
		// Token: 0x060029E7 RID: 10727 RVA: 0x000F5B95 File Offset: 0x000F3D95
		public override void DoAction(Transition trans)
		{
			LordToil_DefendPoint lordToil_DefendPoint = (LordToil_DefendPoint)trans.target;
			lordToil_DefendPoint.SetDefendPoint(lordToil_DefendPoint.lord.ownedPawns.RandomElement<Pawn>().Position);
		}
	}
}
