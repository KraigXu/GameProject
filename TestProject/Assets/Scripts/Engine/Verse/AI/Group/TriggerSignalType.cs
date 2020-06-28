using System;

namespace Verse.AI.Group
{
	// Token: 0x02000612 RID: 1554
	public enum TriggerSignalType : byte
	{
		// Token: 0x04001939 RID: 6457
		Undefined,
		// Token: 0x0400193A RID: 6458
		Tick,
		// Token: 0x0400193B RID: 6459
		Memo,
		// Token: 0x0400193C RID: 6460
		PawnDamaged,
		// Token: 0x0400193D RID: 6461
		PawnArrestAttempted,
		// Token: 0x0400193E RID: 6462
		PawnLost,
		// Token: 0x0400193F RID: 6463
		BuildingDamaged,
		// Token: 0x04001940 RID: 6464
		BuildingLost,
		// Token: 0x04001941 RID: 6465
		FactionRelationsChanged,
		// Token: 0x04001942 RID: 6466
		DormancyWakeup,
		// Token: 0x04001943 RID: 6467
		Clamor,
		// Token: 0x04001944 RID: 6468
		MechClusterDefeated
	}
}
