using System;

namespace RimWorld
{
	// Token: 0x0200081A RID: 2074
	public class Thought_ChemicalInterestVsTeetotaler : Thought_SituationalSocial
	{
		// Token: 0x06003441 RID: 13377 RVA: 0x0011F38C File Offset: 0x0011D58C
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			int num = this.pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			if (this.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) >= 0)
			{
				return 0f;
			}
			if (num == 1)
			{
				return -20f;
			}
			return -30f;
		}
	}
}
