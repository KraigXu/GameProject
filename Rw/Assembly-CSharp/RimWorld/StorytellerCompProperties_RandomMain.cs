using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A15 RID: 2581
	public class StorytellerCompProperties_RandomMain : StorytellerCompProperties
	{
		// Token: 0x06003D40 RID: 15680 RVA: 0x00143C00 File Offset: 0x00141E00
		public StorytellerCompProperties_RandomMain()
		{
			this.compClass = typeof(StorytellerComp_RandomMain);
		}

		// Token: 0x040023BE RID: 9150
		public float mtbDays;

		// Token: 0x040023BF RID: 9151
		public List<IncidentCategoryEntry> categoryWeights = new List<IncidentCategoryEntry>();

		// Token: 0x040023C0 RID: 9152
		public float maxThreatBigIntervalDays = 99999f;

		// Token: 0x040023C1 RID: 9153
		public FloatRange randomPointsFactorRange = new FloatRange(0.5f, 1.5f);

		// Token: 0x040023C2 RID: 9154
		public bool skipThreatBigIfRaidBeacon;
	}
}
