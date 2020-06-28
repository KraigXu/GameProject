using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA7 RID: 4007
	[DefOf]
	public static class ApparelLayerDefOf
	{
		// Token: 0x060060AE RID: 24750 RVA: 0x002172EE File Offset: 0x002154EE
		static ApparelLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ApparelLayerDefOf));
		}

		// Token: 0x04003AB0 RID: 15024
		public static ApparelLayerDef OnSkin;

		// Token: 0x04003AB1 RID: 15025
		public static ApparelLayerDef Shell;

		// Token: 0x04003AB2 RID: 15026
		public static ApparelLayerDef Middle;

		// Token: 0x04003AB3 RID: 15027
		public static ApparelLayerDef Belt;

		// Token: 0x04003AB4 RID: 15028
		public static ApparelLayerDef Overhead;
	}
}
