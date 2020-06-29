using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class KeyBindingCategoryDefOf
	{
		
		static KeyBindingCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(KeyBindingCategoryDefOf));
		}

		
		public static KeyBindingCategoryDef MainTabs;
	}
}
