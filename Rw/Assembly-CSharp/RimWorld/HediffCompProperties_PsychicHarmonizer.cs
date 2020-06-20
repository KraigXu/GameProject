using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200107A RID: 4218
	public class HediffCompProperties_PsychicHarmonizer : HediffCompProperties
	{
		// Token: 0x06006425 RID: 25637 RVA: 0x0022B19E File Offset: 0x0022939E
		public HediffCompProperties_PsychicHarmonizer()
		{
			this.compClass = typeof(HediffComp_PsychicHarmonizer);
		}

		// Token: 0x04003CF2 RID: 15602
		public float range;

		// Token: 0x04003CF3 RID: 15603
		public ThoughtDef thought;
	}
}
