using System;

namespace Verse
{
	// Token: 0x02000089 RID: 137
	public class CompProperties_Lifespan : CompProperties
	{
		// Token: 0x060004C4 RID: 1220 RVA: 0x00017E19 File Offset: 0x00016019
		public CompProperties_Lifespan()
		{
			this.compClass = typeof(CompLifespan);
		}

		// Token: 0x04000223 RID: 547
		public int lifespanTicks = 100;
	}
}
