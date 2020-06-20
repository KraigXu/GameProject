using System;

namespace RimWorld
{
	// Token: 0x02000B20 RID: 2848
	public class PawnGroupMakerParms
	{
		// Token: 0x06004302 RID: 17154 RVA: 0x00168C9C File Offset: 0x00166E9C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"groupKind=",
				this.groupKind,
				", tile=",
				this.tile,
				", inhabitants=",
				this.inhabitants.ToString(),
				", points=",
				this.points,
				", faction=",
				this.faction,
				", traderKind=",
				this.traderKind,
				", generateFightersOnly=",
				this.generateFightersOnly.ToString(),
				", dontUseSingleUseRocketLaunchers=",
				this.dontUseSingleUseRocketLaunchers.ToString(),
				", raidStrategy=",
				this.raidStrategy,
				", forceOneIncap=",
				this.forceOneIncap.ToString(),
				", seed=",
				this.seed
			});
		}

		// Token: 0x04002684 RID: 9860
		public PawnGroupKindDef groupKind;

		// Token: 0x04002685 RID: 9861
		public int tile = -1;

		// Token: 0x04002686 RID: 9862
		public bool inhabitants;

		// Token: 0x04002687 RID: 9863
		public float points;

		// Token: 0x04002688 RID: 9864
		public Faction faction;

		// Token: 0x04002689 RID: 9865
		public TraderKindDef traderKind;

		// Token: 0x0400268A RID: 9866
		public bool generateFightersOnly;

		// Token: 0x0400268B RID: 9867
		public bool dontUseSingleUseRocketLaunchers;

		// Token: 0x0400268C RID: 9868
		public RaidStrategyDef raidStrategy;

		// Token: 0x0400268D RID: 9869
		public bool forceOneIncap;

		// Token: 0x0400268E RID: 9870
		public int? seed;
	}
}
