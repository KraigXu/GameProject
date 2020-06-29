using System;

namespace RimWorld
{
	
	[DefOf]
	public static class RoyalTitleDefOf
	{
		
		static RoyalTitleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoyalTitleDefOf));
		}

		
		[MayRequireRoyalty]
		public static RoyalTitleDef Knight;

		
		[MayRequireRoyalty]
		public static RoyalTitleDef Count;
	}
}
