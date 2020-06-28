using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F5A RID: 3930
	[DefOf]
	public static class WorkTypeDefOf
	{
		// Token: 0x06006061 RID: 24673 RVA: 0x00216DD1 File Offset: 0x00214FD1
		static WorkTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorkTypeDefOf));
		}

		// Token: 0x04003612 RID: 13842
		public static WorkTypeDef Mining;

		// Token: 0x04003613 RID: 13843
		public static WorkTypeDef Growing;

		// Token: 0x04003614 RID: 13844
		public static WorkTypeDef Construction;

		// Token: 0x04003615 RID: 13845
		public static WorkTypeDef Warden;

		// Token: 0x04003616 RID: 13846
		public static WorkTypeDef Doctor;

		// Token: 0x04003617 RID: 13847
		public static WorkTypeDef Firefighter;

		// Token: 0x04003618 RID: 13848
		public static WorkTypeDef Hunting;

		// Token: 0x04003619 RID: 13849
		public static WorkTypeDef Handling;

		// Token: 0x0400361A RID: 13850
		public static WorkTypeDef Crafting;

		// Token: 0x0400361B RID: 13851
		public static WorkTypeDef Hauling;

		// Token: 0x0400361C RID: 13852
		public static WorkTypeDef Research;
	}
}
