using System;

namespace RimWorld
{
	
	[DefOf]
	public static class ScenarioDefOf
	{
		
		static ScenarioDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenarioDefOf));
		}

		
		public static ScenarioDef Crashlanded;

		
		public static ScenarioDef Tutorial;
	}
}
