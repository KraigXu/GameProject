using System;

namespace RimWorld
{
	// Token: 0x02000F95 RID: 3989
	[DefOf]
	public static class BillStoreModeDefOf
	{
		// Token: 0x0600609C RID: 24732 RVA: 0x002171BC File Offset: 0x002153BC
		static BillStoreModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillStoreModeDefOf));
		}

		// Token: 0x04003A4E RID: 14926
		public static BillStoreModeDef DropOnFloor;

		// Token: 0x04003A4F RID: 14927
		public static BillStoreModeDef BestStockpile;

		// Token: 0x04003A50 RID: 14928
		public static BillStoreModeDef SpecificStockpile;
	}
}
