using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200113F RID: 4415
	public class QuestNode_GetRandomInRangeInt : QuestNode
	{
		// Token: 0x0600671B RID: 26395 RVA: 0x002417C4 File Offset: 0x0023F9C4
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<int>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
			return true;
		}

		// Token: 0x0600671C RID: 26396 RVA: 0x002417FC File Offset: 0x0023F9FC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<int>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
		}

		// Token: 0x04003F40 RID: 16192
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003F41 RID: 16193
		public SlateRef<IntRange> range;
	}
}
