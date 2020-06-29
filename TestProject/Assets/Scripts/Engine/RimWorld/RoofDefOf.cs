using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class RoofDefOf
	{
		
		static RoofDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoofDefOf));
		}

		
		public static RoofDef RoofConstructed;

		
		public static RoofDef RoofRockThick;

		
		public static RoofDef RoofRockThin;
	}
}
