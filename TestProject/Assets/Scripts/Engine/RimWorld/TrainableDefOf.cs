using System;

namespace RimWorld
{
	// Token: 0x02000F6F RID: 3951
	[DefOf]
	public static class TrainableDefOf
	{
		// Token: 0x06006076 RID: 24694 RVA: 0x00216F36 File Offset: 0x00215136
		static TrainableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainableDefOf));
		}

		// Token: 0x040037B0 RID: 14256
		public static TrainableDef Tameness;

		// Token: 0x040037B1 RID: 14257
		public static TrainableDef Obedience;

		// Token: 0x040037B2 RID: 14258
		public static TrainableDef Release;
	}
}
