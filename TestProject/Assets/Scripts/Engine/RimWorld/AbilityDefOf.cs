using System;

namespace RimWorld
{
	
	[DefOf]
	public static class AbilityDefOf
	{
		
		static AbilityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(AbilityDefOf));
		}

		
		[MayRequireRoyalty]
		public static AbilityDef Speech;
	}
}
