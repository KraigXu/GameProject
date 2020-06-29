using System;

namespace RimWorld
{
	
	[DefOf]
	public static class DifficultyDefOf
	{
		
		static DifficultyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DifficultyDefOf));
		}

		
		public static DifficultyDef Easy;

		
		public static DifficultyDef Rough;
	}
}
