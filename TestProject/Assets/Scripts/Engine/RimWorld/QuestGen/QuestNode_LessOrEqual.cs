using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020010FC RID: 4348
	public class QuestNode_LessOrEqual : QuestNode
	{
		// Token: 0x06006622 RID: 26146 RVA: 0x0023C768 File Offset: 0x0023A968
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) <= this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06006623 RID: 26147 RVA: 0x0023C7BC File Offset: 0x0023A9BC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) <= this.value2.GetValue(slate))
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

		// Token: 0x04003E2E RID: 15918
		public SlateRef<double> value1;

		// Token: 0x04003E2F RID: 15919
		public SlateRef<double> value2;

		// Token: 0x04003E30 RID: 15920
		public QuestNode node;

		// Token: 0x04003E31 RID: 15921
		public QuestNode elseNode;
	}
}
