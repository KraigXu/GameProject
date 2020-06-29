using System;

namespace RimWorld
{
	
	[DefOf]
	public static class PawnTableDefOf
	{
		
		static PawnTableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnTableDefOf));
		}

		
		public static PawnTableDef Work;

		
		public static PawnTableDef Assign;

		
		public static PawnTableDef Restrict;

		
		public static PawnTableDef Animals;

		
		public static PawnTableDef Wildlife;
	}
}
