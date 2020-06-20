using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D51 RID: 3409
	public class CompProperties_ShipLandingBeacon : CompProperties
	{
		// Token: 0x060052E7 RID: 21223 RVA: 0x001BAE84 File Offset: 0x001B9084
		public CompProperties_ShipLandingBeacon()
		{
			this.compClass = typeof(CompShipLandingBeacon);
		}

		// Token: 0x04002DBF RID: 11711
		public FloatRange edgeLengthRange;
	}
}
