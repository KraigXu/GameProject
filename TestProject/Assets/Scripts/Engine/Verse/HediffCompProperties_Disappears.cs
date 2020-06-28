using System;

namespace Verse
{
	// Token: 0x02000249 RID: 585
	public class HediffCompProperties_Disappears : HediffCompProperties
	{
		// Token: 0x0600103E RID: 4158 RVA: 0x0005D4A4 File Offset: 0x0005B6A4
		public HediffCompProperties_Disappears()
		{
			this.compClass = typeof(HediffComp_Disappears);
		}

		// Token: 0x04000BE7 RID: 3047
		public IntRange disappearsAfterTicks;

		// Token: 0x04000BE8 RID: 3048
		public bool showRemainingTime;
	}
}
