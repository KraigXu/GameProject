using System;

namespace RimWorld
{
	// Token: 0x02000FB0 RID: 4016
	[DefOf]
	public static class InstructionDefOf
	{
		// Token: 0x060060B7 RID: 24759 RVA: 0x00217387 File Offset: 0x00215587
		static InstructionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InstructionDefOf));
		}

		// Token: 0x04003AD9 RID: 15065
		public static InstructionDef RandomizeCharacter;

		// Token: 0x04003ADA RID: 15066
		public static InstructionDef ChooseLandingSite;
	}
}
