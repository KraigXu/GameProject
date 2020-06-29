using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class DesignationCategoryDefOf
	{
		
		static DesignationCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DesignationCategoryDefOf));
		}

		
		public static DesignationCategoryDef Production;

		
		public static DesignationCategoryDef Structure;

		
		public static DesignationCategoryDef Security;

		
		public static DesignationCategoryDef Floors;
	}
}
