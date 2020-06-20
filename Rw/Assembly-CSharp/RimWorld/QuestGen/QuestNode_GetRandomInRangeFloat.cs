using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200113D RID: 4413
	public class QuestNode_GetRandomInRangeFloat : QuestNode
	{
		// Token: 0x06006714 RID: 26388 RVA: 0x00241698 File Offset: 0x0023F898
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
			return true;
		}

		// Token: 0x06006715 RID: 26389 RVA: 0x002416D0 File Offset: 0x0023F8D0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<float>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
		}

		// Token: 0x04003F39 RID: 16185
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003F3A RID: 16186
		public SlateRef<FloatRange> range;
	}
}
