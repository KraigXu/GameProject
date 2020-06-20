using System;

namespace RimWorld
{
	// Token: 0x02000FAE RID: 4014
	[DefOf]
	public static class ExpectationDefOf
	{
		// Token: 0x060060B5 RID: 24757 RVA: 0x00217365 File Offset: 0x00215565
		static ExpectationDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ExpectationDefOf));
		}

		// Token: 0x04003AD2 RID: 15058
		public static ExpectationDef ExtremelyLow;

		// Token: 0x04003AD3 RID: 15059
		public static ExpectationDef VeryLow;

		// Token: 0x04003AD4 RID: 15060
		public static ExpectationDef Low;

		// Token: 0x04003AD5 RID: 15061
		public static ExpectationDef Moderate;

		// Token: 0x04003AD6 RID: 15062
		public static ExpectationDef High;

		// Token: 0x04003AD7 RID: 15063
		public static ExpectationDef SkyHigh;
	}
}
