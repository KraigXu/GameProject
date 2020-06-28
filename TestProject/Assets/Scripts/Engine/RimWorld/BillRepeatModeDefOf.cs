using System;

namespace RimWorld
{
	// Token: 0x02000F94 RID: 3988
	[DefOf]
	public static class BillRepeatModeDefOf
	{
		// Token: 0x0600609B RID: 24731 RVA: 0x002171AB File Offset: 0x002153AB
		static BillRepeatModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillRepeatModeDefOf));
		}

		// Token: 0x04003A4B RID: 14923
		public static BillRepeatModeDef RepeatCount;

		// Token: 0x04003A4C RID: 14924
		public static BillRepeatModeDef TargetCount;

		// Token: 0x04003A4D RID: 14925
		public static BillRepeatModeDef Forever;
	}
}
