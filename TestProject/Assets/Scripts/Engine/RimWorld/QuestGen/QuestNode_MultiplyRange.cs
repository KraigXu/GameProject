using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020010ED RID: 4333
	public class QuestNode_MultiplyRange : QuestNode
	{
		// Token: 0x060065F7 RID: 26103 RVA: 0x0023BE93 File Offset: 0x0023A093
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x060065F8 RID: 26104 RVA: 0x0023BEAC File Offset: 0x0023A0AC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<FloatRange>(this.storeAs.GetValue(slate), this.range.GetValue(slate) * this.value.GetValue(slate), false);
		}

		// Token: 0x04003DFD RID: 15869
		public SlateRef<FloatRange> range;

		// Token: 0x04003DFE RID: 15870
		public SlateRef<float> value;

		// Token: 0x04003DFF RID: 15871
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
