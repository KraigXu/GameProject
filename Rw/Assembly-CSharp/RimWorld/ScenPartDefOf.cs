using System;

namespace RimWorld
{
	// Token: 0x02000F5B RID: 3931
	[DefOf]
	public static class ScenPartDefOf
	{
		// Token: 0x06006062 RID: 24674 RVA: 0x00216DE2 File Offset: 0x00214FE2
		static ScenPartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenPartDefOf));
		}

		// Token: 0x0400361D RID: 13853
		public static ScenPartDef PlayerFaction;

		// Token: 0x0400361E RID: 13854
		public static ScenPartDef ConfigPage_ConfigureStartingPawns;

		// Token: 0x0400361F RID: 13855
		public static ScenPartDef PlayerPawnsArriveMethod;

		// Token: 0x04003620 RID: 13856
		public static ScenPartDef ForcedTrait;

		// Token: 0x04003621 RID: 13857
		public static ScenPartDef ForcedHediff;

		// Token: 0x04003622 RID: 13858
		public static ScenPartDef StartingAnimal;

		// Token: 0x04003623 RID: 13859
		public static ScenPartDef ScatterThingsNearPlayerStart;

		// Token: 0x04003624 RID: 13860
		public static ScenPartDef StartingThing_Defined;

		// Token: 0x04003625 RID: 13861
		public static ScenPartDef ScatterThingsAnywhere;

		// Token: 0x04003626 RID: 13862
		public static ScenPartDef GameStartDialog;
	}
}
