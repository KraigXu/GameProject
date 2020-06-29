using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class ResearchProjectDefOf
	{
		
		static ResearchProjectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ResearchProjectDefOf));
		}

		
		public static ResearchProjectDef CarpetMaking;
	}
}
