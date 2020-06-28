using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020005E7 RID: 1511
	public class TransitionAction_SetDefendTrader : TransitionAction
	{
		// Token: 0x060029E9 RID: 10729 RVA: 0x000F5BBC File Offset: 0x000F3DBC
		public override void DoAction(Transition trans)
		{
			LordToil_DefendPoint lordToil_DefendPoint = (LordToil_DefendPoint)trans.target;
			Pawn pawn = TraderCaravanUtility.FindTrader(lordToil_DefendPoint.lord);
			if (pawn != null)
			{
				lordToil_DefendPoint.SetDefendPoint(pawn.Position);
				return;
			}
			IEnumerable<Pawn> source = from x in lordToil_DefendPoint.lord.ownedPawns
			where x.GetTraderCaravanRole() == TraderCaravanRole.Carrier
			select x;
			if (source.Any<Pawn>())
			{
				lordToil_DefendPoint.SetDefendPoint(source.RandomElement<Pawn>().Position);
				return;
			}
			lordToil_DefendPoint.SetDefendPoint(lordToil_DefendPoint.lord.ownedPawns.RandomElement<Pawn>().Position);
		}
	}
}
