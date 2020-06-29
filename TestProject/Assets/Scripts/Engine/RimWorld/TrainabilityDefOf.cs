using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class TrainabilityDefOf
	{
		
		static TrainabilityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainabilityDefOf));
		}

		
		public static TrainabilityDef None;

		
		public static TrainabilityDef Simple;

		
		public static TrainabilityDef Intermediate;

		
		public static TrainabilityDef Advanced;
	}
}
