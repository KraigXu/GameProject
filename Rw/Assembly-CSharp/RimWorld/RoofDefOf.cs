using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F6B RID: 3947
	[DefOf]
	public static class RoofDefOf
	{
		// Token: 0x06006072 RID: 24690 RVA: 0x00216EF2 File Offset: 0x002150F2
		static RoofDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoofDefOf));
		}

		// Token: 0x0400378D RID: 14221
		public static RoofDef RoofConstructed;

		// Token: 0x0400378E RID: 14222
		public static RoofDef RoofRockThick;

		// Token: 0x0400378F RID: 14223
		public static RoofDef RoofRockThin;
	}
}
