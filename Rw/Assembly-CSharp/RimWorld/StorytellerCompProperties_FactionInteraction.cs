using System;

namespace RimWorld
{
	// Token: 0x02000A0F RID: 2575
	public class StorytellerCompProperties_FactionInteraction : StorytellerCompProperties
	{
		// Token: 0x06003D2C RID: 15660 RVA: 0x001438D0 File Offset: 0x00141AD0
		public StorytellerCompProperties_FactionInteraction()
		{
			this.compClass = typeof(StorytellerComp_FactionInteraction);
		}

		// Token: 0x040023AD RID: 9133
		public IncidentDef incident;

		// Token: 0x040023AE RID: 9134
		public float baseIncidentsPerYear;

		// Token: 0x040023AF RID: 9135
		public float minSpacingDays;

		// Token: 0x040023B0 RID: 9136
		public StoryDanger minDanger;

		// Token: 0x040023B1 RID: 9137
		public bool fullAlliesOnly;
	}
}
