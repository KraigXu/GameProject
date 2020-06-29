using System;

namespace RimWorld
{
	
	[DefOf]
	public static class RoadPathingDefOf
	{
		
		static RoadPathingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadPathingDefOf));
		}

		
		public static RoadPathingDef Avoid;

		
		public static RoadPathingDef Bulldoze;
	}
}
