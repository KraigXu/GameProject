using System;

namespace RimWorld
{
	
	[DefOf]
	public static class RaidStrategyDefOf
	{
		
		static RaidStrategyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RaidStrategyDefOf));
		}

		
		public static RaidStrategyDef ImmediateAttack;

		
		public static RaidStrategyDef ImmediateAttackFriendly;
	}
}
