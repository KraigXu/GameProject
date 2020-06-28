using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F66 RID: 3942
	[DefOf]
	public static class PawnCapacityDefOf
	{
		// Token: 0x0600606D RID: 24685 RVA: 0x00216E9D File Offset: 0x0021509D
		static PawnCapacityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnCapacityDefOf));
		}

		// Token: 0x04003754 RID: 14164
		public static PawnCapacityDef Consciousness;

		// Token: 0x04003755 RID: 14165
		public static PawnCapacityDef Sight;

		// Token: 0x04003756 RID: 14166
		public static PawnCapacityDef Hearing;

		// Token: 0x04003757 RID: 14167
		public static PawnCapacityDef Moving;

		// Token: 0x04003758 RID: 14168
		public static PawnCapacityDef Manipulation;

		// Token: 0x04003759 RID: 14169
		public static PawnCapacityDef Talking;

		// Token: 0x0400375A RID: 14170
		public static PawnCapacityDef Eating;

		// Token: 0x0400375B RID: 14171
		public static PawnCapacityDef Breathing;

		// Token: 0x0400375C RID: 14172
		public static PawnCapacityDef BloodFiltration;

		// Token: 0x0400375D RID: 14173
		public static PawnCapacityDef BloodPumping;

		// Token: 0x0400375E RID: 14174
		public static PawnCapacityDef Metabolism;
	}
}
