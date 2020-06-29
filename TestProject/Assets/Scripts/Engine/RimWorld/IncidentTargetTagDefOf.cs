using System;

namespace RimWorld
{
	
	[DefOf]
	public static class IncidentTargetTagDefOf
	{
		
		static IncidentTargetTagDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IncidentTargetTagDefOf));
		}

		
		public static IncidentTargetTagDef World;

		
		public static IncidentTargetTagDef Caravan;

		
		public static IncidentTargetTagDef Map_RaidBeacon;

		
		public static IncidentTargetTagDef Map_PlayerHome;

		
		public static IncidentTargetTagDef Map_Misc;
	}
}
