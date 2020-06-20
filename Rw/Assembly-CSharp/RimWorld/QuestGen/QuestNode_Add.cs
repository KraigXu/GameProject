using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020010EA RID: 4330
	public class QuestNode_Add : QuestNode
	{
		// Token: 0x060065EE RID: 26094 RVA: 0x0023BD83 File Offset: 0x00239F83
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x060065EF RID: 26095 RVA: 0x0023BD9C File Offset: 0x00239F9C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) + this.value2.GetValue(slate), false);
		}

		// Token: 0x04003DF4 RID: 15860
		public SlateRef<double> value1;

		// Token: 0x04003DF5 RID: 15861
		public SlateRef<double> value2;

		// Token: 0x04003DF6 RID: 15862
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
