using System;

namespace RimWorld
{
	
	[DefOf]
	public static class ResearchTabDefOf
	{
		
		static ResearchTabDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ResearchTabDefOf));
		}

		
		public static ResearchTabDef Main;
	}
}
