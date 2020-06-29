using System;

namespace Verse.AI
{
	
	public interface IAttackTargetSearcher
	{
		
		// (get) Token: 0x060028E5 RID: 10469
		Thing Thing { get; }

		
		// (get) Token: 0x060028E6 RID: 10470
		Verb CurrentEffectiveVerb { get; }

		
		// (get) Token: 0x060028E7 RID: 10471
		LocalTargetInfo LastAttackedTarget { get; }

		
		// (get) Token: 0x060028E8 RID: 10472
		int LastAttackTargetTick { get; }
	}
}
