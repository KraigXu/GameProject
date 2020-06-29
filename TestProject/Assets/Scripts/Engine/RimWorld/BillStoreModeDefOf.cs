using System;

namespace RimWorld
{
	
	[DefOf]
	public static class BillStoreModeDefOf
	{
		
		static BillStoreModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillStoreModeDefOf));
		}

		
		public static BillStoreModeDef DropOnFloor;

		
		public static BillStoreModeDef BestStockpile;

		
		public static BillStoreModeDef SpecificStockpile;
	}
}
