using System;

namespace RimWorld
{
	
	[DefOf]
	public static class FleshTypeDefOf
	{
		
		static FleshTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FleshTypeDefOf));
		}

		
		public static FleshTypeDef Normal;

		
		public static FleshTypeDef Mechanoid;

		
		public static FleshTypeDef Insectoid;
	}
}
