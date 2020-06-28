using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F9A RID: 3994
	[DefOf]
	public static class LetterDefOf
	{
		// Token: 0x060060A1 RID: 24737 RVA: 0x00217211 File Offset: 0x00215411
		static LetterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LetterDefOf));
		}

		// Token: 0x04003A5D RID: 14941
		public static LetterDef ThreatBig;

		// Token: 0x04003A5E RID: 14942
		public static LetterDef ThreatSmall;

		// Token: 0x04003A5F RID: 14943
		public static LetterDef NegativeEvent;

		// Token: 0x04003A60 RID: 14944
		public static LetterDef NeutralEvent;

		// Token: 0x04003A61 RID: 14945
		public static LetterDef PositiveEvent;

		// Token: 0x04003A62 RID: 14946
		public static LetterDef Death;

		// Token: 0x04003A63 RID: 14947
		public static LetterDef NewQuest;
	}
}
