using System;

namespace RimWorld
{
	// Token: 0x02000FB1 RID: 4017
	[DefOf]
	public static class GatheringDefOf
	{
		// Token: 0x060060B8 RID: 24760 RVA: 0x00217398 File Offset: 0x00215598
		static GatheringDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GatheringDefOf));
		}

		// Token: 0x04003ADB RID: 15067
		public static GatheringDef Party;

		// Token: 0x04003ADC RID: 15068
		public static GatheringDef MarriageCeremony;

		// Token: 0x04003ADD RID: 15069
		[MayRequireRoyalty]
		public static GatheringDef ThroneSpeech;
	}
}
