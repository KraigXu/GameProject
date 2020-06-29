using System;

namespace RimWorld
{
	
	[DefOf]
	public static class StuffCategoryDefOf
	{
		
		static StuffCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StuffCategoryDefOf));
		}

		
		public static StuffCategoryDef Metallic;

		
		public static StuffCategoryDef Woody;

		
		public static StuffCategoryDef Stony;

		
		public static StuffCategoryDef Fabric;

		
		public static StuffCategoryDef Leathery;
	}
}
