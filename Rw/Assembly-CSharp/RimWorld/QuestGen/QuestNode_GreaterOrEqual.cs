using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020010F3 RID: 4339
	public class QuestNode_GreaterOrEqual : QuestNode
	{
		// Token: 0x06006607 RID: 26119 RVA: 0x0023C1E0 File Offset: 0x0023A3E0
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) >= this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06006608 RID: 26120 RVA: 0x0023C234 File Offset: 0x0023A434
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) >= this.value2.GetValue(slate))
			{
				if (this.node != null)
				{
					this.node.Run();
					return;
				}
			}
			else if (this.elseNode != null)
			{
				this.elseNode.Run();
			}
		}

		// Token: 0x04003E10 RID: 15888
		public SlateRef<double> value1;

		// Token: 0x04003E11 RID: 15889
		public SlateRef<double> value2;

		// Token: 0x04003E12 RID: 15890
		public QuestNode node;

		// Token: 0x04003E13 RID: 15891
		public QuestNode elseNode;
	}
}
