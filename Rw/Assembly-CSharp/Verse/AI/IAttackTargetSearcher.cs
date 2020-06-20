using System;

namespace Verse.AI
{
	// Token: 0x020005BB RID: 1467
	public interface IAttackTargetSearcher
	{
		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x060028E5 RID: 10469
		Thing Thing { get; }

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x060028E6 RID: 10470
		Verb CurrentEffectiveVerb { get; }

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x060028E7 RID: 10471
		LocalTargetInfo LastAttackedTarget { get; }

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x060028E8 RID: 10472
		int LastAttackTargetTick { get; }
	}
}
