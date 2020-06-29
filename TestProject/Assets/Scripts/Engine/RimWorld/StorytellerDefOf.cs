using System;

namespace RimWorld
{
	
	[DefOf]
	public static class StorytellerDefOf
	{
		
		static StorytellerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StorytellerDefOf));
		}

		
		public static StorytellerDef Cassandra;

		
		public static StorytellerDef Tutor;
	}
}
