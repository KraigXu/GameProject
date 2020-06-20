using System;

namespace Verse
{
	// Token: 0x02000257 RID: 599
	public class HediffCompProperties_GetsPermanent : HediffCompProperties
	{
		// Token: 0x0600106B RID: 4203 RVA: 0x0005DE98 File Offset: 0x0005C098
		public HediffCompProperties_GetsPermanent()
		{
			this.compClass = typeof(HediffComp_GetsPermanent);
		}

		// Token: 0x04000BF9 RID: 3065
		public float becomePermanentChanceFactor = 1f;

		// Token: 0x04000BFA RID: 3066
		public string permanentLabel;

		// Token: 0x04000BFB RID: 3067
		public string instantlyPermanentLabel;
	}
}
