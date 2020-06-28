using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020010FD RID: 4349
	public class QuestNode_LessOrFail : QuestNode
	{
		// Token: 0x06006625 RID: 26149 RVA: 0x0023C810 File Offset: 0x0023AA10
		protected override bool TestRunInt(Slate slate)
		{
			return this.value1.GetValue(slate) < this.value2.GetValue(slate) && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x06006626 RID: 26150 RVA: 0x0023C844 File Offset: 0x0023AA44
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) < this.value2.GetValue(slate) && this.node != null)
			{
				this.node.Run();
			}
		}

		// Token: 0x04003E32 RID: 15922
		public SlateRef<double> value1;

		// Token: 0x04003E33 RID: 15923
		public SlateRef<double> value2;

		// Token: 0x04003E34 RID: 15924
		public QuestNode node;
	}
}
