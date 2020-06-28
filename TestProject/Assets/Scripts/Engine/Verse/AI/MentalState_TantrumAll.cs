using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200055D RID: 1373
	public class MentalState_TantrumAll : MentalState_TantrumRandom
	{
		// Token: 0x0600270D RID: 9997 RVA: 0x000E4B4A File Offset: 0x000E2D4A
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}
	}
}
