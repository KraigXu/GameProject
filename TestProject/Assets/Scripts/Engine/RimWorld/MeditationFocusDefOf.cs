using System;

namespace RimWorld
{
	
	[DefOf]
	public static class MeditationFocusDefOf
	{
		
		static MeditationFocusDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MeditationFocusDefOf));
		}

		
		[MayRequireRoyalty]
		public static MeditationFocusDef Natural;
	}
}
