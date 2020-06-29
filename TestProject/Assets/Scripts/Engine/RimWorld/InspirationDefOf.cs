using System;

namespace RimWorld
{
	
	[DefOf]
	public static class InspirationDefOf
	{
		
		static InspirationDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InspirationDefOf));
		}

		
		public static InspirationDef Inspired_Trade;

		
		public static InspirationDef Inspired_Recruitment;

		
		public static InspirationDef Inspired_Taming;

		
		public static InspirationDef Inspired_Surgery;

		
		public static InspirationDef Inspired_Creativity;
	}
}
