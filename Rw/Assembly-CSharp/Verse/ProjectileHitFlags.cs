using System;

namespace Verse
{
	// Token: 0x0200030B RID: 779
	[Flags]
	public enum ProjectileHitFlags
	{
		// Token: 0x04000E4E RID: 3662
		None = 0,
		// Token: 0x04000E4F RID: 3663
		IntendedTarget = 1,
		// Token: 0x04000E50 RID: 3664
		NonTargetPawns = 2,
		// Token: 0x04000E51 RID: 3665
		NonTargetWorld = 4,
		// Token: 0x04000E52 RID: 3666
		All = -1
	}
}
