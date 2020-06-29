using System;

namespace RimWorld
{
	
	[DefOf]
	public static class NeedDefOf
	{
		
		static NeedDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(NeedDefOf));
		}

		
		public static NeedDef Food;

		
		public static NeedDef Rest;

		
		public static NeedDef Joy;
	}
}
