using System;

namespace RimWorld
{
	
	[DefOf]
	public static class HibernatableStateDefOf
	{
		
		static HibernatableStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HibernatableStateDefOf));
		}

		
		public static HibernatableStateDef Running;

		
		public static HibernatableStateDef Starting;

		
		public static HibernatableStateDef Hibernating;
	}
}
