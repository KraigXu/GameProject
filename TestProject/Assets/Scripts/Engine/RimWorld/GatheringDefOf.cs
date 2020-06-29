using System;

namespace RimWorld
{
	
	[DefOf]
	public static class GatheringDefOf
	{
		
		static GatheringDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GatheringDefOf));
		}

		
		public static GatheringDef Party;

		
		public static GatheringDef MarriageCeremony;

		
		[MayRequireRoyalty]
		public static GatheringDef ThroneSpeech;
	}
}
