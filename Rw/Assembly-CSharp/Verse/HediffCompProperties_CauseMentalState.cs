using System;

namespace Verse
{
	// Token: 0x0200023E RID: 574
	public class HediffCompProperties_CauseMentalState : HediffCompProperties
	{
		// Token: 0x0600101F RID: 4127 RVA: 0x0005CD54 File Offset: 0x0005AF54
		public HediffCompProperties_CauseMentalState()
		{
			this.compClass = typeof(HediffComp_CauseMentalState);
		}

		// Token: 0x04000BD3 RID: 3027
		public MentalStateDef animalMentalState;

		// Token: 0x04000BD4 RID: 3028
		public MentalStateDef animalMentalStateAlias;

		// Token: 0x04000BD5 RID: 3029
		public MentalStateDef humanMentalState;

		// Token: 0x04000BD6 RID: 3030
		public LetterDef letterDef;

		// Token: 0x04000BD7 RID: 3031
		public float mtbDaysToCauseMentalState;
	}
}
