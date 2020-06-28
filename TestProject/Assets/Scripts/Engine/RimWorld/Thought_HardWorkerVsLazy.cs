using System;

namespace RimWorld
{
	// Token: 0x02000814 RID: 2068
	public class Thought_HardWorkerVsLazy : Thought_SituationalSocial
	{
		// Token: 0x06003435 RID: 13365 RVA: 0x0011F108 File Offset: 0x0011D308
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			int num = this.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.Industriousness);
			if (num > 0)
			{
				return 0f;
			}
			if (num == 0)
			{
				return -5f;
			}
			if (num == -1)
			{
				return -20f;
			}
			return -30f;
		}
	}
}
