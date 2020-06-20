using System;

namespace Verse
{
	// Token: 0x02000265 RID: 613
	public class HediffCompProperties_Link : HediffCompProperties
	{
		// Token: 0x0600109F RID: 4255 RVA: 0x0005EBBF File Offset: 0x0005CDBF
		public HediffCompProperties_Link()
		{
			this.compClass = typeof(HediffComp_Link);
		}

		// Token: 0x04000C1E RID: 3102
		public bool showName = true;

		// Token: 0x04000C1F RID: 3103
		public float maxDistance = -1f;
	}
}
