using System;

namespace Verse.AI
{
	
	public interface IAttackTarget : ILoadReferenceable
	{
		
		// (get) Token: 0x0600290D RID: 10509
		Thing Thing { get; }

		
		// (get) Token: 0x0600290E RID: 10510
		LocalTargetInfo TargetCurrentlyAimingAt { get; }

		
		// (get) Token: 0x0600290F RID: 10511
		float TargetPriorityFactor { get; }

		
		bool ThreatDisabled(IAttackTargetSearcher disabledFor);
	}
}
