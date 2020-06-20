using System;

namespace Verse
{
	// Token: 0x020002CF RID: 719
	public enum LookMode : byte
	{
		// Token: 0x04000D99 RID: 3481
		Undefined,
		// Token: 0x04000D9A RID: 3482
		Value,
		// Token: 0x04000D9B RID: 3483
		Deep,
		// Token: 0x04000D9C RID: 3484
		Reference,
		// Token: 0x04000D9D RID: 3485
		Def,
		// Token: 0x04000D9E RID: 3486
		LocalTargetInfo,
		// Token: 0x04000D9F RID: 3487
		TargetInfo,
		// Token: 0x04000DA0 RID: 3488
		GlobalTargetInfo,
		// Token: 0x04000DA1 RID: 3489
		BodyPart
	}
}
