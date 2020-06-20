using System;

namespace RimWorld
{
	// Token: 0x02000811 RID: 2065
	public class Thought_Incestuous : Thought_SituationalSocial
	{
		// Token: 0x0600342F RID: 13359 RVA: 0x0011EFDC File Offset: 0x0011D1DC
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			return LovePartnerRelationUtility.IncestOpinionOffsetFor(this.otherPawn, this.pawn);
		}
	}
}
