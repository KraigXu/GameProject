using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D04 RID: 3332
	public class CompProperties_EmptyStateGraphic : CompProperties
	{
		// Token: 0x06005104 RID: 20740 RVA: 0x001B3071 File Offset: 0x001B1271
		public CompProperties_EmptyStateGraphic()
		{
			this.compClass = typeof(CompEmptyStateGraphic);
		}

		// Token: 0x04002CF1 RID: 11505
		public GraphicData graphicData;
	}
}
