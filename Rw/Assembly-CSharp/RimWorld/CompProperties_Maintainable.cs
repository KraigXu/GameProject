using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D1D RID: 3357
	public class CompProperties_Maintainable : CompProperties
	{
		// Token: 0x060051B2 RID: 20914 RVA: 0x001B5B1F File Offset: 0x001B3D1F
		public CompProperties_Maintainable()
		{
			this.compClass = typeof(CompMaintainable);
		}

		// Token: 0x04002D22 RID: 11554
		public int ticksHealthy = 1000;

		// Token: 0x04002D23 RID: 11555
		public int ticksNeedsMaintenance = 1000;

		// Token: 0x04002D24 RID: 11556
		public int damagePerTickRare = 10;
	}
}
