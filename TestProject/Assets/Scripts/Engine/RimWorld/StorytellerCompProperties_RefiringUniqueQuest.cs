using System;

namespace RimWorld
{
	// Token: 0x02000A19 RID: 2585
	public class StorytellerCompProperties_RefiringUniqueQuest : StorytellerCompProperties
	{
		// Token: 0x06003D4A RID: 15690 RVA: 0x00143CF3 File Offset: 0x00141EF3
		public StorytellerCompProperties_RefiringUniqueQuest()
		{
			this.compClass = typeof(StorytellerComp_RefiringUniqueQuest);
		}

		// Token: 0x040023C4 RID: 9156
		public IncidentDef incident;

		// Token: 0x040023C5 RID: 9157
		public float refireEveryDays = -1f;
	}
}
