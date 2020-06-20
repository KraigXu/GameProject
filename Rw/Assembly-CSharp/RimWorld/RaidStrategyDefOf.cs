using System;

namespace RimWorld
{
	// Token: 0x02000F73 RID: 3955
	[DefOf]
	public static class RaidStrategyDefOf
	{
		// Token: 0x0600607A RID: 24698 RVA: 0x00216F7A File Offset: 0x0021517A
		static RaidStrategyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RaidStrategyDefOf));
		}

		// Token: 0x040037B9 RID: 14265
		public static RaidStrategyDef ImmediateAttack;

		// Token: 0x040037BA RID: 14266
		public static RaidStrategyDef ImmediateAttackFriendly;
	}
}
