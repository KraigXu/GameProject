using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020010EC RID: 4332
	public class QuestNode_Multiply : QuestNode
	{
		// Token: 0x060065F4 RID: 26100 RVA: 0x0023BE3B File Offset: 0x0023A03B
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x060065F5 RID: 26101 RVA: 0x0023BE54 File Offset: 0x0023A054
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) * this.value2.GetValue(slate), false);
		}

		// Token: 0x04003DFA RID: 15866
		public SlateRef<double> value1;

		// Token: 0x04003DFB RID: 15867
		public SlateRef<double> value2;

		// Token: 0x04003DFC RID: 15868
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
