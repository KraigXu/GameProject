using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class DamageArmorCategoryDefOf
	{
		
		static DamageArmorCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DamageArmorCategoryDefOf));
		}

		
		public static DamageArmorCategoryDef Sharp;
	}
}
