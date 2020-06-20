using System;

namespace Verse
{
	// Token: 0x020002CE RID: 718
	public enum LoadSaveMode : byte
	{
		// Token: 0x04000D93 RID: 3475
		Inactive,
		// Token: 0x04000D94 RID: 3476
		Saving,
		// Token: 0x04000D95 RID: 3477
		LoadingVars,
		// Token: 0x04000D96 RID: 3478
		ResolvingCrossRefs,
		// Token: 0x04000D97 RID: 3479
		PostLoadInit
	}
}
