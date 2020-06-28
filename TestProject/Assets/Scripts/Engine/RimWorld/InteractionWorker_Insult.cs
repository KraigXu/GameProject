using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B45 RID: 2885
	public class InteractionWorker_Insult : InteractionWorker
	{
		// Token: 0x060043CE RID: 17358 RVA: 0x0016D7D0 File Offset: 0x0016B9D0
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.007f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}

		// Token: 0x040026CF RID: 9935
		private const float BaseSelectionWeight = 0.007f;
	}
}
