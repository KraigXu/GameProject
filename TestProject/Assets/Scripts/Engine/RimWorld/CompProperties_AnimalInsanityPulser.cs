using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000862 RID: 2146
	public class CompProperties_AnimalInsanityPulser : CompProperties
	{
		// Token: 0x06003503 RID: 13571 RVA: 0x001226CA File Offset: 0x001208CA
		public CompProperties_AnimalInsanityPulser()
		{
			this.compClass = typeof(CompAnimalInsanityPulser);
		}

		// Token: 0x04001C31 RID: 7217
		public IntRange pulseInterval = new IntRange(60000, 150000);

		// Token: 0x04001C32 RID: 7218
		public int radius = 25;
	}
}
