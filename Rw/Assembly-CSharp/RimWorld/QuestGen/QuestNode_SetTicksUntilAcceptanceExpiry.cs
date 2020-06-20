using System;

namespace RimWorld.QuestGen
{
	// Token: 0x0200118E RID: 4494
	public class QuestNode_SetTicksUntilAcceptanceExpiry : QuestNode
	{
		// Token: 0x0600682F RID: 26671 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006830 RID: 26672 RVA: 0x00246660 File Offset: 0x00244860
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.quest.ticksUntilAcceptanceExpiry = this.ticks.GetValue(slate);
		}

		// Token: 0x04004078 RID: 16504
		public SlateRef<int> ticks;
	}
}
