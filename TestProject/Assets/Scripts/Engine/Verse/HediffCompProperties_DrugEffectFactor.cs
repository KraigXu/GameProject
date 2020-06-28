using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000251 RID: 593
	public class HediffCompProperties_DrugEffectFactor : HediffCompProperties
	{
		// Token: 0x0600105C RID: 4188 RVA: 0x0005DC81 File Offset: 0x0005BE81
		public HediffCompProperties_DrugEffectFactor()
		{
			this.compClass = typeof(HediffComp_DrugEffectFactor);
		}

		// Token: 0x04000BF4 RID: 3060
		public ChemicalDef chemical;
	}
}
