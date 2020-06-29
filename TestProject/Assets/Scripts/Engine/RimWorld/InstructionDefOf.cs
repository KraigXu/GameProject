using System;

namespace RimWorld
{
	
	[DefOf]
	public static class InstructionDefOf
	{
		
		static InstructionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InstructionDefOf));
		}

		
		public static InstructionDef RandomizeCharacter;

		
		public static InstructionDef ChooseLandingSite;
	}
}
