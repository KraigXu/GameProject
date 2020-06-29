using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class GenStepDefOf
	{
		
		static GenStepDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GenStepDefOf));
		}

		
		public static GenStepDef PreciousLump;
	}
}
