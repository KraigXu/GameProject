using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020005E9 RID: 1513
	public class TransitionAction_CheckGiveGift : TransitionAction
	{
		// Token: 0x060029ED RID: 10733 RVA: 0x000F5CA8 File Offset: 0x000F3EA8
		public override void DoAction(Transition trans)
		{
			if (DebugSettings.instantVisitorsGift || (trans.target.lord.numPawnsLostViolently == 0 && Rand.Chance(VisitorGiftForPlayerUtility.ChanceToLeaveGift(trans.target.lord.faction, trans.Map))))
			{
				VisitorGiftForPlayerUtility.GiveGift(trans.target.lord.ownedPawns, trans.target.lord.faction);
			}
		}
	}
}
