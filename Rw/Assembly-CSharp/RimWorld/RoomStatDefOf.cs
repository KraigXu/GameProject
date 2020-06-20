using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F7D RID: 3965
	[DefOf]
	public static class RoomStatDefOf
	{
		// Token: 0x06006084 RID: 24708 RVA: 0x00217024 File Offset: 0x00215224
		static RoomStatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoomStatDefOf));
		}

		// Token: 0x04003907 RID: 14599
		public static RoomStatDef Cleanliness;

		// Token: 0x04003908 RID: 14600
		public static RoomStatDef Wealth;

		// Token: 0x04003909 RID: 14601
		public static RoomStatDef Space;

		// Token: 0x0400390A RID: 14602
		public static RoomStatDef Beauty;

		// Token: 0x0400390B RID: 14603
		public static RoomStatDef Impressiveness;

		// Token: 0x0400390C RID: 14604
		public static RoomStatDef InfectionChanceFactor;

		// Token: 0x0400390D RID: 14605
		public static RoomStatDef ResearchSpeedFactor;

		// Token: 0x0400390E RID: 14606
		public static RoomStatDef GraveVisitingJoyGainFactor;

		// Token: 0x0400390F RID: 14607
		public static RoomStatDef FoodPoisonChance;
	}
}
