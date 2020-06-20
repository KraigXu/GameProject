using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005C1 RID: 1473
	public class DutyDef : Def
	{
		// Token: 0x040018B0 RID: 6320
		public ThinkNode thinkNode;

		// Token: 0x040018B1 RID: 6321
		public ThinkNode constantThinkNode;

		// Token: 0x040018B2 RID: 6322
		public bool alwaysShowWeapon;

		// Token: 0x040018B3 RID: 6323
		public ThinkTreeDutyHook hook = ThinkTreeDutyHook.HighPriority;

		// Token: 0x040018B4 RID: 6324
		public RandomSocialMode socialModeMax = RandomSocialMode.SuperActive;

		// Token: 0x040018B5 RID: 6325
		public bool threatDisabled;
	}
}
