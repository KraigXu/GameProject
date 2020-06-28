using System;

namespace RimWorld
{
	// Token: 0x02000F93 RID: 3987
	[DefOf]
	public static class PrisonerInteractionModeDefOf
	{
		// Token: 0x0600609A RID: 24730 RVA: 0x0021719A File Offset: 0x0021539A
		static PrisonerInteractionModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PrisonerInteractionModeDefOf));
		}

		// Token: 0x04003A46 RID: 14918
		public static PrisonerInteractionModeDef NoInteraction;

		// Token: 0x04003A47 RID: 14919
		public static PrisonerInteractionModeDef AttemptRecruit;

		// Token: 0x04003A48 RID: 14920
		public static PrisonerInteractionModeDef ReduceResistance;

		// Token: 0x04003A49 RID: 14921
		public static PrisonerInteractionModeDef Release;

		// Token: 0x04003A4A RID: 14922
		public static PrisonerInteractionModeDef Execution;
	}
}
