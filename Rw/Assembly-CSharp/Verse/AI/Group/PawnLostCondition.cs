using System;

namespace Verse.AI.Group
{
	// Token: 0x020005C5 RID: 1477
	public enum PawnLostCondition : byte
	{
		// Token: 0x040018C3 RID: 6339
		Undefined,
		// Token: 0x040018C4 RID: 6340
		Vanished,
		// Token: 0x040018C5 RID: 6341
		IncappedOrKilled,
		// Token: 0x040018C6 RID: 6342
		MadePrisoner,
		// Token: 0x040018C7 RID: 6343
		ChangedFaction,
		// Token: 0x040018C8 RID: 6344
		ExitedMap,
		// Token: 0x040018C9 RID: 6345
		LeftVoluntarily,
		// Token: 0x040018CA RID: 6346
		Drafted,
		// Token: 0x040018CB RID: 6347
		ForcedToJoinOtherLord,
		// Token: 0x040018CC RID: 6348
		ForcedByPlayerAction,
		// Token: 0x040018CD RID: 6349
		ForcedByQuest,
		// Token: 0x040018CE RID: 6350
		NoLongerEnteringTransportPods
	}
}
