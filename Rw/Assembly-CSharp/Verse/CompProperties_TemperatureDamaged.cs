using System;

namespace Verse
{
	// Token: 0x0200008A RID: 138
	public class CompProperties_TemperatureDamaged : CompProperties
	{
		// Token: 0x060004C5 RID: 1221 RVA: 0x00017E39 File Offset: 0x00016039
		public CompProperties_TemperatureDamaged()
		{
			this.compClass = typeof(CompTemperatureDamaged);
		}

		// Token: 0x04000224 RID: 548
		public FloatRange safeTemperatureRange = new FloatRange(-30f, 30f);

		// Token: 0x04000225 RID: 549
		public int damagePerTickRare = 1;
	}
}
