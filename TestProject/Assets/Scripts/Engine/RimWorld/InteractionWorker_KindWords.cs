using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B43 RID: 2883
	public class InteractionWorker_KindWords : InteractionWorker
	{
		// Token: 0x060043CA RID: 17354 RVA: 0x0016D700 File Offset: 0x0016B900
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (initiator.story.traits.HasTrait(TraitDefOf.Kind))
			{
				return 0.01f;
			}
			return 0f;
		}
	}
}
