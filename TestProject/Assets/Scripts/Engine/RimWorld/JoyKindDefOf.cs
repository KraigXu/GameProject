using System;

namespace RimWorld
{
	
	[DefOf]
	public static class JoyKindDefOf
	{
		
		static JoyKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(JoyKindDefOf));
		}

		
		public static JoyKindDef Meditative;

		
		public static JoyKindDef Social;

		
		public static JoyKindDef Gluttonous;
	}
}
