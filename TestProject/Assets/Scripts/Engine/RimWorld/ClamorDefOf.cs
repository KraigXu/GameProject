using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class ClamorDefOf
	{
		
		static ClamorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ClamorDefOf));
		}

		
		public static ClamorDef Movement;

		
		public static ClamorDef Harm;

		
		public static ClamorDef Construction;

		
		public static ClamorDef Impact;

		
		public static ClamorDef Ability;
	}
}
