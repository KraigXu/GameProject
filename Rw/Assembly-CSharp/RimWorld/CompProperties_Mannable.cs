using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000871 RID: 2161
	public class CompProperties_Mannable : CompProperties
	{
		// Token: 0x06003528 RID: 13608 RVA: 0x00122EAF File Offset: 0x001210AF
		public CompProperties_Mannable()
		{
			this.compClass = typeof(CompMannable);
		}

		// Token: 0x04001C76 RID: 7286
		public WorkTags manWorkType;
	}
}
