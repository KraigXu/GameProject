using System;

namespace RimWorld
{
	// Token: 0x02000A1D RID: 2589
	public class StorytellerCompProperties_SingleMTB : StorytellerCompProperties
	{
		// Token: 0x06003D54 RID: 15700 RVA: 0x00143E24 File Offset: 0x00142024
		public StorytellerCompProperties_SingleMTB()
		{
			this.compClass = typeof(StorytellerComp_SingleMTB);
		}

		// Token: 0x040023C7 RID: 9159
		public IncidentDef incident;

		// Token: 0x040023C8 RID: 9160
		public float mtbDays = 13f;
	}
}
