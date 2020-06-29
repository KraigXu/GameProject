using System;

namespace RimWorld
{
	
	[DefOf]
	public static class WorkGiverDefOf
	{
		
		static WorkGiverDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorkGiverDefOf));
		}

		
		public static WorkGiverDef Refuel;

		
		public static WorkGiverDef Repair;
	}
}
