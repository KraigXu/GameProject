using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000877 RID: 2167
	public class CompProperties_SelfhealHitpoints : CompProperties
	{
		// Token: 0x06003539 RID: 13625 RVA: 0x0012302F File Offset: 0x0012122F
		public CompProperties_SelfhealHitpoints()
		{
			this.compClass = typeof(CompSelfhealHitpoints);
		}

		// Token: 0x04001C8C RID: 7308
		public int ticksPerHeal;
	}
}
