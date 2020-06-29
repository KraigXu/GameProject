using System;

namespace RimWorld
{
	
	[DefOf]
	public static class BillRepeatModeDefOf
	{
		
		static BillRepeatModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillRepeatModeDefOf));
		}

		
		public static BillRepeatModeDef RepeatCount;

		
		public static BillRepeatModeDef TargetCount;

		
		public static BillRepeatModeDef Forever;
	}
}
