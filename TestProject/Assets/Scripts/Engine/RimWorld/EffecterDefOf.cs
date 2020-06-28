using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F54 RID: 3924
	[DefOf]
	public static class EffecterDefOf
	{
		// Token: 0x0600605B RID: 24667 RVA: 0x00216D6B File Offset: 0x00214F6B
		static EffecterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(EffecterDefOf));
		}

		// Token: 0x040035C4 RID: 13764
		public static EffecterDef Clean;

		// Token: 0x040035C5 RID: 13765
		public static EffecterDef ConstructMetal;

		// Token: 0x040035C6 RID: 13766
		public static EffecterDef ConstructWood;

		// Token: 0x040035C7 RID: 13767
		public static EffecterDef ConstructDirt;

		// Token: 0x040035C8 RID: 13768
		public static EffecterDef RoofWork;

		// Token: 0x040035C9 RID: 13769
		public static EffecterDef EatMeat;

		// Token: 0x040035CA RID: 13770
		public static EffecterDef ProgressBar;

		// Token: 0x040035CB RID: 13771
		public static EffecterDef Mine;

		// Token: 0x040035CC RID: 13772
		public static EffecterDef Deflect_Metal;

		// Token: 0x040035CD RID: 13773
		public static EffecterDef Deflect_Metal_Bullet;

		// Token: 0x040035CE RID: 13774
		public static EffecterDef Deflect_General;

		// Token: 0x040035CF RID: 13775
		public static EffecterDef Deflect_General_Bullet;

		// Token: 0x040035D0 RID: 13776
		public static EffecterDef DamageDiminished_Metal;

		// Token: 0x040035D1 RID: 13777
		public static EffecterDef DamageDiminished_General;

		// Token: 0x040035D2 RID: 13778
		public static EffecterDef Drill;

		// Token: 0x040035D3 RID: 13779
		public static EffecterDef Research;

		// Token: 0x040035D4 RID: 13780
		public static EffecterDef ClearSnow;

		// Token: 0x040035D5 RID: 13781
		public static EffecterDef Sow;

		// Token: 0x040035D6 RID: 13782
		public static EffecterDef Harvest;

		// Token: 0x040035D7 RID: 13783
		public static EffecterDef Vomit;

		// Token: 0x040035D8 RID: 13784
		public static EffecterDef PlayPoker;

		// Token: 0x040035D9 RID: 13785
		public static EffecterDef Interceptor_BlockedProjectile;

		// Token: 0x040035DA RID: 13786
		[MayRequireRoyalty]
		public static EffecterDef ActivatorProximityTriggered;

		// Token: 0x040035DB RID: 13787
		[MayRequireRoyalty]
		public static EffecterDef DisabledByEMP;
	}
}
