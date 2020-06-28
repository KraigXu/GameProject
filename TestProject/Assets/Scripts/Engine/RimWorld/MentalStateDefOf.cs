using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F69 RID: 3945
	[DefOf]
	public static class MentalStateDefOf
	{
		// Token: 0x06006070 RID: 24688 RVA: 0x00216ED0 File Offset: 0x002150D0
		static MentalStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MentalStateDefOf));
		}

		// Token: 0x04003762 RID: 14178
		public static MentalStateDef Berserk;

		// Token: 0x04003763 RID: 14179
		public static MentalStateDef Binging_DrugExtreme;

		// Token: 0x04003764 RID: 14180
		public static MentalStateDef Wander_Psychotic;

		// Token: 0x04003765 RID: 14181
		public static MentalStateDef Binging_DrugMajor;

		// Token: 0x04003766 RID: 14182
		public static MentalStateDef Wander_Sad;

		// Token: 0x04003767 RID: 14183
		public static MentalStateDef Wander_OwnRoom;

		// Token: 0x04003768 RID: 14184
		public static MentalStateDef PanicFlee;

		// Token: 0x04003769 RID: 14185
		public static MentalStateDef Manhunter;

		// Token: 0x0400376A RID: 14186
		public static MentalStateDef ManhunterPermanent;

		// Token: 0x0400376B RID: 14187
		public static MentalStateDef SocialFighting;
	}
}
