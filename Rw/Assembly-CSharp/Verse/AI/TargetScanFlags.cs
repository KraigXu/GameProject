using System;

namespace Verse.AI
{
	// Token: 0x020005BA RID: 1466
	[Flags]
	public enum TargetScanFlags
	{
		// Token: 0x04001879 RID: 6265
		None = 0,
		// Token: 0x0400187A RID: 6266
		NeedLOSToPawns = 1,
		// Token: 0x0400187B RID: 6267
		NeedLOSToNonPawns = 2,
		// Token: 0x0400187C RID: 6268
		NeedLOSToAll = 3,
		// Token: 0x0400187D RID: 6269
		NeedReachable = 4,
		// Token: 0x0400187E RID: 6270
		NeedReachableIfCantHitFromMyPos = 8,
		// Token: 0x0400187F RID: 6271
		NeedNonBurning = 16,
		// Token: 0x04001880 RID: 6272
		NeedThreat = 32,
		// Token: 0x04001881 RID: 6273
		NeedActiveThreat = 64,
		// Token: 0x04001882 RID: 6274
		LOSBlockableByGas = 128,
		// Token: 0x04001883 RID: 6275
		NeedAutoTargetable = 256
	}
}
