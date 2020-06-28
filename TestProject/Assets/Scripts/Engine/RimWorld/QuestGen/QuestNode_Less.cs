using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020010FB RID: 4347
	public class QuestNode_Less : QuestNode
	{
		// Token: 0x0600661F RID: 26143 RVA: 0x0023C6C0 File Offset: 0x0023A8C0
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) < this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06006620 RID: 26144 RVA: 0x0023C714 File Offset: 0x0023A914
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) < this.value2.GetValue(slate))
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

		// Token: 0x04003E2A RID: 15914
		public SlateRef<double> value1;

		// Token: 0x04003E2B RID: 15915
		public SlateRef<double> value2;

		// Token: 0x04003E2C RID: 15916
		public QuestNode node;

		// Token: 0x04003E2D RID: 15917
		public QuestNode elseNode;
	}
}
