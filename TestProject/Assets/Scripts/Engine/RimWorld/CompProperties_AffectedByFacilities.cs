using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000861 RID: 2145
	public class CompProperties_AffectedByFacilities : CompProperties
	{
		// Token: 0x06003502 RID: 13570 RVA: 0x001226B2 File Offset: 0x001208B2
		public CompProperties_AffectedByFacilities()
		{
			this.compClass = typeof(CompAffectedByFacilities);
		}

		// Token: 0x04001C30 RID: 7216
		public List<ThingDef> linkableFacilities;
	}
}
