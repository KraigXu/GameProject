using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class SpecialThingFilterDefOf
	{
		
		static SpecialThingFilterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SpecialThingFilterDefOf));
		}

		
		public static SpecialThingFilterDef AllowFresh;

		
		public static SpecialThingFilterDef AllowDeadmansApparel;

		
		public static SpecialThingFilterDef AllowNonDeadmansApparel;
	}
}
