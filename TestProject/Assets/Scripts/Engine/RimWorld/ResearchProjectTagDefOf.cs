using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class ResearchProjectTagDefOf
	{
		
		static ResearchProjectTagDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ResearchProjectTagDefOf));
		}

		
		public static ResearchProjectTagDef ShipRelated;
	}
}
