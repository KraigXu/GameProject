using System;

namespace RimWorld
{
	
	[DefOf]
	public static class RoadDefOf
	{
		
		static RoadDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadDefOf));
		}

		
		public static RoadDef DirtRoad;

		
		public static RoadDef AncientAsphaltRoad;

		
		public static RoadDef AncientAsphaltHighway;
	}
}
