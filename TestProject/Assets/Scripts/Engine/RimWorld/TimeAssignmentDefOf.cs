using System;

namespace RimWorld
{
	
	[DefOf]
	public static class TimeAssignmentDefOf
	{
		
		static TimeAssignmentDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TimeAssignmentDefOf));
		}

		
		public static TimeAssignmentDef Anything;

		
		public static TimeAssignmentDef Work;

		
		public static TimeAssignmentDef Joy;

		
		public static TimeAssignmentDef Sleep;

		
		[MayRequireRoyalty]
		public static TimeAssignmentDef Meditate;
	}
}
