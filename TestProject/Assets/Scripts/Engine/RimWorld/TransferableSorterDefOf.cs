using System;

namespace RimWorld
{
	
	[DefOf]
	public static class TransferableSorterDefOf
	{
		
		static TransferableSorterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TransferableSorterDefOf));
		}

		
		public static TransferableSorterDef Category;

		
		public static TransferableSorterDef MarketValue;
	}
}
