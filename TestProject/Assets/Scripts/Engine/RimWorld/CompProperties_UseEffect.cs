using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087F RID: 2175
	public class CompProperties_UseEffect : CompProperties
	{
		// Token: 0x06003541 RID: 13633 RVA: 0x00123174 File Offset: 0x00121374
		public CompProperties_UseEffect()
		{
			this.compClass = typeof(CompUseEffect);
		}

		// Token: 0x04001CAA RID: 7338
		public bool doCameraShake;

		// Token: 0x04001CAB RID: 7339
		public ThingDef moteOnUsed;

		// Token: 0x04001CAC RID: 7340
		public float moteOnUsedScale = 1f;
	}
}
