using System;

namespace RimWorld
{
	// Token: 0x02000FB4 RID: 4020
	[DefOf]
	public static class AbilityDefOf
	{
		// Token: 0x060060BB RID: 24763 RVA: 0x002173CB File Offset: 0x002155CB
		static AbilityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(AbilityDefOf));
		}

		// Token: 0x04003AED RID: 15085
		[MayRequireRoyalty]
		public static AbilityDef Speech;
	}
}
