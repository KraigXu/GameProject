using System;

namespace Verse
{
	// Token: 0x02000267 RID: 615
	public class HediffCompProperties_ReactOnDamage : HediffCompProperties
	{
		// Token: 0x060010A7 RID: 4263 RVA: 0x0005EDD0 File Offset: 0x0005CFD0
		public HediffCompProperties_ReactOnDamage()
		{
			this.compClass = typeof(HediffComp_ReactOnDamage);
		}

		// Token: 0x04000C23 RID: 3107
		public DamageDef damageDefIncoming;

		// Token: 0x04000C24 RID: 3108
		public BodyPartDef createHediffOn;

		// Token: 0x04000C25 RID: 3109
		public HediffDef createHediff;

		// Token: 0x04000C26 RID: 3110
		public bool vomit;
	}
}
