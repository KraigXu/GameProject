using System;

namespace Verse
{
	// Token: 0x0200030E RID: 782
	public enum DestroyMode : byte
	{
		// Token: 0x04000E63 RID: 3683
		Vanish,
		// Token: 0x04000E64 RID: 3684
		WillReplace,
		// Token: 0x04000E65 RID: 3685
		KillFinalize,
		// Token: 0x04000E66 RID: 3686
		Deconstruct,
		// Token: 0x04000E67 RID: 3687
		FailConstruction,
		// Token: 0x04000E68 RID: 3688
		Cancel,
		// Token: 0x04000E69 RID: 3689
		Refund,
		// Token: 0x04000E6A RID: 3690
		QuestLogic
	}
}
