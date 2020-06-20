using System;

namespace Verse
{
	// Token: 0x0200026B RID: 619
	public class HediffCompProperties_SelfHeal : HediffCompProperties
	{
		// Token: 0x060010B1 RID: 4273 RVA: 0x0005EF75 File Offset: 0x0005D175
		public HediffCompProperties_SelfHeal()
		{
			this.compClass = typeof(HediffComp_SelfHeal);
		}

		// Token: 0x04000C28 RID: 3112
		public int healIntervalTicksStanding = 50;

		// Token: 0x04000C29 RID: 3113
		public float healAmount = 1f;
	}
}
