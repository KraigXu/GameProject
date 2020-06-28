using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020010F4 RID: 4340
	public class QuestNode_GreaterOrFail : QuestNode
	{
		// Token: 0x0600660A RID: 26122 RVA: 0x0023C288 File Offset: 0x0023A488
		protected override bool TestRunInt(Slate slate)
		{
			return this.value1.GetValue(slate) > this.value2.GetValue(slate) && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x0600660B RID: 26123 RVA: 0x0023C2BC File Offset: 0x0023A4BC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate) && this.node != null)
			{
				this.node.Run();
			}
		}

		// Token: 0x04003E14 RID: 15892
		public SlateRef<double> value1;

		// Token: 0x04003E15 RID: 15893
		public SlateRef<double> value2;

		// Token: 0x04003E16 RID: 15894
		public QuestNode node;
	}
}
