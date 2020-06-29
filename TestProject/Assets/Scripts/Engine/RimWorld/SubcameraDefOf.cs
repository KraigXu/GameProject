using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class SubcameraDefOf
	{
		
		static SubcameraDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SubcameraDefOf));
		}

		
		public static SubcameraDef WaterDepth;
	}
}
