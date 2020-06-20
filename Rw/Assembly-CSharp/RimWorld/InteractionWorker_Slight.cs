using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B4B RID: 2891
	public class InteractionWorker_Slight : InteractionWorker
	{
		// Token: 0x060043E6 RID: 17382 RVA: 0x0016F2AB File Offset: 0x0016D4AB
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.02f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}

		// Token: 0x040026E5 RID: 9957
		private const float BaseSelectionWeight = 0.02f;
	}
}
