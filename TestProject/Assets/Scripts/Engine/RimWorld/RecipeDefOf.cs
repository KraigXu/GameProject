using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class RecipeDefOf
	{
		
		static RecipeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RecipeDefOf));
		}

		
		public static RecipeDef RemoveBodyPart;

		
		public static RecipeDef CookMealSimple;

		
		public static RecipeDef InstallPegLeg;
	}
}
