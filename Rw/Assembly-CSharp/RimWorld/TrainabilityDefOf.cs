using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F98 RID: 3992
	[DefOf]
	public static class TrainabilityDefOf
	{
		// Token: 0x0600609F RID: 24735 RVA: 0x002171EF File Offset: 0x002153EF
		static TrainabilityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainabilityDefOf));
		}

		// Token: 0x04003A54 RID: 14932
		public static TrainabilityDef None;

		// Token: 0x04003A55 RID: 14933
		public static TrainabilityDef Simple;

		// Token: 0x04003A56 RID: 14934
		public static TrainabilityDef Intermediate;

		// Token: 0x04003A57 RID: 14935
		public static TrainabilityDef Advanced;
	}
}
