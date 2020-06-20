using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008F7 RID: 2295
	public class RiverDef : Def
	{
		// Token: 0x04001F70 RID: 8048
		public int spawnFlowThreshold = -1;

		// Token: 0x04001F71 RID: 8049
		public float spawnChance = 1f;

		// Token: 0x04001F72 RID: 8050
		public int degradeThreshold;

		// Token: 0x04001F73 RID: 8051
		public RiverDef degradeChild;

		// Token: 0x04001F74 RID: 8052
		public List<RiverDef.Branch> branches;

		// Token: 0x04001F75 RID: 8053
		public float widthOnWorld = 0.5f;

		// Token: 0x04001F76 RID: 8054
		public float widthOnMap = 10f;

		// Token: 0x04001F77 RID: 8055
		public float debugOpacity;

		// Token: 0x0200192A RID: 6442
		public class Branch
		{
			// Token: 0x04005FCE RID: 24526
			public int minFlow;

			// Token: 0x04005FCF RID: 24527
			public RiverDef child;

			// Token: 0x04005FD0 RID: 24528
			public float chance = 1f;
		}
	}
}
