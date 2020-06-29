using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class StuffAppearanceDefOf
	{
		
		static StuffAppearanceDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StuffAppearanceDefOf));
		}

		
		public static StuffAppearanceDef Smooth;
	}
}
