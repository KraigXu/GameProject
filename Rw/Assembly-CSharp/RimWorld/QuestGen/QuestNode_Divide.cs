using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020010EB RID: 4331
	public class QuestNode_Divide : QuestNode
	{
		// Token: 0x060065F1 RID: 26097 RVA: 0x0023BDE3 File Offset: 0x00239FE3
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x060065F2 RID: 26098 RVA: 0x0023BDFC File Offset: 0x00239FFC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) / this.value2.GetValue(slate), false);
		}

		// Token: 0x04003DF7 RID: 15863
		public SlateRef<double> value1;

		// Token: 0x04003DF8 RID: 15864
		public SlateRef<double> value2;

		// Token: 0x04003DF9 RID: 15865
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
