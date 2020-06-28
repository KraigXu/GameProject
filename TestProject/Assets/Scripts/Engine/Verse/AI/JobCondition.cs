using System;

namespace Verse.AI
{
	// Token: 0x02000515 RID: 1301
	public enum JobCondition : byte
	{
		// Token: 0x040016CF RID: 5839
		None,
		// Token: 0x040016D0 RID: 5840
		Ongoing,
		// Token: 0x040016D1 RID: 5841
		Succeeded,
		// Token: 0x040016D2 RID: 5842
		Incompletable,
		// Token: 0x040016D3 RID: 5843
		InterruptOptional,
		// Token: 0x040016D4 RID: 5844
		InterruptForced,
		// Token: 0x040016D5 RID: 5845
		QueuedNoLongerValid,
		// Token: 0x040016D6 RID: 5846
		Errored,
		// Token: 0x040016D7 RID: 5847
		ErroredPather
	}
}
