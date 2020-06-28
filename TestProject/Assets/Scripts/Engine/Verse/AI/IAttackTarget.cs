using System;

namespace Verse.AI
{
	// Token: 0x020005C3 RID: 1475
	public interface IAttackTarget : ILoadReferenceable
	{
		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x0600290D RID: 10509
		Thing Thing { get; }

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x0600290E RID: 10510
		LocalTargetInfo TargetCurrentlyAimingAt { get; }

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x0600290F RID: 10511
		float TargetPriorityFactor { get; }

		// Token: 0x06002910 RID: 10512
		bool ThreatDisabled(IAttackTargetSearcher disabledFor);
	}
}
