using System;

namespace RimWorld
{
	// Token: 0x02000FA6 RID: 4006
	[DefOf]
	public static class PawnsArrivalModeDefOf
	{
		// Token: 0x060060AD RID: 24749 RVA: 0x002172DD File Offset: 0x002154DD
		static PawnsArrivalModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnsArrivalModeDefOf));
		}

		// Token: 0x04003AAC RID: 15020
		public static PawnsArrivalModeDef EdgeWalkIn;

		// Token: 0x04003AAD RID: 15021
		public static PawnsArrivalModeDef CenterDrop;

		// Token: 0x04003AAE RID: 15022
		public static PawnsArrivalModeDef EdgeDrop;

		// Token: 0x04003AAF RID: 15023
		public static PawnsArrivalModeDef RandomDrop;
	}
}
