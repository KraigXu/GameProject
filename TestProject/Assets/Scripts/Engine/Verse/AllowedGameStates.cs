using System;

namespace Verse
{
	// Token: 0x02000355 RID: 853
	[Flags]
	public enum AllowedGameStates
	{
		// Token: 0x04000F1C RID: 3868
		Invalid = 0,
		// Token: 0x04000F1D RID: 3869
		Entry = 1,
		// Token: 0x04000F1E RID: 3870
		Playing = 2,
		// Token: 0x04000F1F RID: 3871
		WorldRenderedNow = 4,
		// Token: 0x04000F20 RID: 3872
		IsCurrentlyOnMap = 8,
		// Token: 0x04000F21 RID: 3873
		HasGameCondition = 16,
		// Token: 0x04000F22 RID: 3874
		PlayingOnMap = 10,
		// Token: 0x04000F23 RID: 3875
		PlayingOnWorld = 6
	}
}
