using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001078 RID: 4216
	public class HediffCompProperties_ExplodeOnDeath : HediffCompProperties
	{
		// Token: 0x06006420 RID: 25632 RVA: 0x0022B0AB File Offset: 0x002292AB
		public HediffCompProperties_ExplodeOnDeath()
		{
			this.compClass = typeof(HediffComp_ExplodeOnDeath);
		}

		// Token: 0x04003CED RID: 15597
		public bool destroyGear;

		// Token: 0x04003CEE RID: 15598
		public bool destroyBody;

		// Token: 0x04003CEF RID: 15599
		public float explosionRadius;

		// Token: 0x04003CF0 RID: 15600
		public DamageDef damageDef;

		// Token: 0x04003CF1 RID: 15601
		public int damageAmount = -1;
	}
}
