using System;

namespace RimWorld
{
	// Token: 0x02000A1F RID: 2591
	public class StorytellerCompProperties_SingleOnceFixed : StorytellerCompProperties
	{
		// Token: 0x06003D59 RID: 15705 RVA: 0x00143E6B File Offset: 0x0014206B
		public StorytellerCompProperties_SingleOnceFixed()
		{
			this.compClass = typeof(StorytellerComp_SingleOnceFixed);
		}

		// Token: 0x040023C9 RID: 9161
		public IncidentDef incident;

		// Token: 0x040023CA RID: 9162
		public int fireAfterDaysPassed;

		// Token: 0x040023CB RID: 9163
		public RoyalTitleDef skipIfColonistHasMinTitle;

		// Token: 0x040023CC RID: 9164
		public bool skipIfOnExtremeBiome;

		// Token: 0x040023CD RID: 9165
		public int minColonistCount;
	}
}
