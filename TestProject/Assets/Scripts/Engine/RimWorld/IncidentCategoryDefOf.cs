using System;

namespace RimWorld
{
	
	[DefOf]
	public static class IncidentCategoryDefOf
	{
		
		static IncidentCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IncidentCategoryDefOf));
		}

		
		public static IncidentCategoryDef Misc;

		
		public static IncidentCategoryDef ThreatSmall;

		
		public static IncidentCategoryDef ThreatBig;

		
		public static IncidentCategoryDef FactionArrival;

		
		public static IncidentCategoryDef OrbitalVisitor;

		
		public static IncidentCategoryDef ShipChunkDrop;

		
		public static IncidentCategoryDef DiseaseHuman;

		
		public static IncidentCategoryDef DiseaseAnimal;

		
		public static IncidentCategoryDef AllyAssistance;

		
		public static IncidentCategoryDef EndGame_ShipEscape;

		
		public static IncidentCategoryDef GiveQuest;

		
		public static IncidentCategoryDef DeepDrillInfestation;
	}
}
