using System;

namespace RimWorld
{
	// Token: 0x02000FB5 RID: 4021
	[DefOf]
	public static class MeditationFocusDefOf
	{
		// Token: 0x060060BC RID: 24764 RVA: 0x002173DC File Offset: 0x002155DC
		static MeditationFocusDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MeditationFocusDefOf));
		}

		// Token: 0x04003AEE RID: 15086
		[MayRequireRoyalty]
		public static MeditationFocusDef Natural;
	}
}
