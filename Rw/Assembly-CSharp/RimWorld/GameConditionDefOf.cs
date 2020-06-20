using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F57 RID: 3927
	[DefOf]
	public static class GameConditionDefOf
	{
		// Token: 0x0600605E RID: 24670 RVA: 0x00216D9E File Offset: 0x00214F9E
		static GameConditionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GameConditionDefOf));
		}

		// Token: 0x040035F1 RID: 13809
		public static GameConditionDef SolarFlare;

		// Token: 0x040035F2 RID: 13810
		public static GameConditionDef Eclipse;

		// Token: 0x040035F3 RID: 13811
		public static GameConditionDef PsychicDrone;

		// Token: 0x040035F4 RID: 13812
		public static GameConditionDef PsychicSoothe;

		// Token: 0x040035F5 RID: 13813
		public static GameConditionDef HeatWave;

		// Token: 0x040035F6 RID: 13814
		public static GameConditionDef ColdSnap;

		// Token: 0x040035F7 RID: 13815
		public static GameConditionDef Flashstorm;

		// Token: 0x040035F8 RID: 13816
		public static GameConditionDef VolcanicWinter;

		// Token: 0x040035F9 RID: 13817
		public static GameConditionDef ToxicFallout;

		// Token: 0x040035FA RID: 13818
		public static GameConditionDef Aurora;

		// Token: 0x040035FB RID: 13819
		[MayRequireRoyalty]
		public static GameConditionDef PsychicSuppression;

		// Token: 0x040035FC RID: 13820
		[MayRequireRoyalty]
		public static GameConditionDef WeatherController;

		// Token: 0x040035FD RID: 13821
		[MayRequireRoyalty]
		public static GameConditionDef EMIField;

		// Token: 0x040035FE RID: 13822
		[MayRequireRoyalty]
		public static GameConditionDef ToxicSpewer;
	}
}
