using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class ToolCapacityDefOf
	{
		
		static ToolCapacityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ToolCapacityDef));
		}

		
		public static ToolCapacityDef KickMaterialInEyes;
	}
}
