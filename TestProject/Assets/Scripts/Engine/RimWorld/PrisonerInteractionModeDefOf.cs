using System;

namespace RimWorld
{
	
	[DefOf]
	public static class PrisonerInteractionModeDefOf
	{
		
		static PrisonerInteractionModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PrisonerInteractionModeDefOf));
		}

		
		public static PrisonerInteractionModeDef NoInteraction;

		
		public static PrisonerInteractionModeDef AttemptRecruit;

		
		public static PrisonerInteractionModeDef ReduceResistance;

		
		public static PrisonerInteractionModeDef Release;

		
		public static PrisonerInteractionModeDef Execution;
	}
}
