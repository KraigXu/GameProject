using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001072 RID: 4210
	public struct ApparelGraphicRecord
	{
		// Token: 0x0600640C RID: 25612 RVA: 0x0022AA1C File Offset: 0x00228C1C
		public ApparelGraphicRecord(Graphic graphic, Apparel sourceApparel)
		{
			this.graphic = graphic;
			this.sourceApparel = sourceApparel;
		}

		// Token: 0x04003CDF RID: 15583
		public Graphic graphic;

		// Token: 0x04003CE0 RID: 15584
		public Apparel sourceApparel;
	}
}
