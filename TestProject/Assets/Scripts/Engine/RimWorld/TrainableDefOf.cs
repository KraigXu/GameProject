using System;

namespace RimWorld
{
	
	[DefOf]
	public static class TrainableDefOf
	{
		
		static TrainableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainableDefOf));
		}

		
		public static TrainableDef Tameness;

		
		public static TrainableDef Obedience;

		
		public static TrainableDef Release;
	}
}
