using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001262 RID: 4706
	public class SitePartParams : IExposable
	{
		// Token: 0x06006E2D RID: 28205 RVA: 0x00267ADC File Offset: 0x00265CDC
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.randomValue, "randomValue", 0, false);
			Scribe_Values.Look<float>(ref this.threatPoints, "threatPoints", 0f, false);
			Scribe_Defs.Look<ThingDef>(ref this.preciousLumpResources, "preciousLumpResources");
			Scribe_Defs.Look<PawnKindDef>(ref this.animalKind, "animalKind");
			Scribe_Values.Look<int>(ref this.turretsCount, "turretsCount", 0, false);
			Scribe_Values.Look<int>(ref this.mortarsCount, "mortarsCount", 0, false);
		}

		// Token: 0x04004414 RID: 17428
		public int randomValue;

		// Token: 0x04004415 RID: 17429
		public float threatPoints;

		// Token: 0x04004416 RID: 17430
		public ThingDef preciousLumpResources;

		// Token: 0x04004417 RID: 17431
		public PawnKindDef animalKind;

		// Token: 0x04004418 RID: 17432
		public int turretsCount;

		// Token: 0x04004419 RID: 17433
		public int mortarsCount;
	}
}
