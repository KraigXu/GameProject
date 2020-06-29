using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class MapGeneratorDefOf
	{
		
		static MapGeneratorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MapGeneratorDefOf));
		}

		
		public static MapGeneratorDef Encounter;

		
		public static MapGeneratorDef Base_Player;

		
		public static MapGeneratorDef Base_Faction;

		
		public static MapGeneratorDef EscapeShip;
	}
}
