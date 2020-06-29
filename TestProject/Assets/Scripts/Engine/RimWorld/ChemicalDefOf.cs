using System;

namespace RimWorld
{
	
	[DefOf]
	public static class ChemicalDefOf
	{
		
		static ChemicalDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ChemicalDefOf));
		}

		
		public static ChemicalDef Alcohol;
	}
}
