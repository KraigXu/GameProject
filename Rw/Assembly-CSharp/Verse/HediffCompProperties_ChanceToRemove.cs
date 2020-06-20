using System;

namespace Verse
{
	// Token: 0x02000240 RID: 576
	public class HediffCompProperties_ChanceToRemove : HediffCompProperties
	{
		// Token: 0x06001024 RID: 4132 RVA: 0x0005D004 File Offset: 0x0005B204
		public HediffCompProperties_ChanceToRemove()
		{
			this.compClass = typeof(HediffComp_ChanceToRemove);
		}

		// Token: 0x04000BD8 RID: 3032
		public int intervalTicks;

		// Token: 0x04000BD9 RID: 3033
		public float chance;
	}
}
