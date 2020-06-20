using System;

namespace RimWorld
{
	// Token: 0x02000A22 RID: 2594
	public class StorytellerCompProperties_Triggered : StorytellerCompProperties
	{
		// Token: 0x06003D5E RID: 15710 RVA: 0x00143EBF File Offset: 0x001420BF
		public StorytellerCompProperties_Triggered()
		{
			this.compClass = typeof(StorytellerComp_Triggered);
		}

		// Token: 0x040023CF RID: 9167
		public IncidentDef incident;

		// Token: 0x040023D0 RID: 9168
		public int delayTicks = 60;
	}
}
