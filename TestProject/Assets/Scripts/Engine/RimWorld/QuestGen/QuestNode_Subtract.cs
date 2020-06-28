using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020010EE RID: 4334
	public class QuestNode_Subtract : QuestNode
	{
		// Token: 0x060065FA RID: 26106 RVA: 0x0023BEEF File Offset: 0x0023A0EF
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x060065FB RID: 26107 RVA: 0x0023BF08 File Offset: 0x0023A108
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) - this.value2.GetValue(slate), false);
		}

		// Token: 0x04003E00 RID: 15872
		public SlateRef<double> value1;

		// Token: 0x04003E01 RID: 15873
		public SlateRef<double> value2;

		// Token: 0x04003E02 RID: 15874
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
