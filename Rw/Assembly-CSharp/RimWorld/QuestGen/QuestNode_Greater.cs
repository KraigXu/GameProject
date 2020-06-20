using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020010F2 RID: 4338
	public class QuestNode_Greater : QuestNode
	{
		// Token: 0x06006604 RID: 26116 RVA: 0x0023C138 File Offset: 0x0023A338
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06006605 RID: 26117 RVA: 0x0023C18C File Offset: 0x0023A38C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate))
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

		// Token: 0x04003E0C RID: 15884
		public SlateRef<double> value1;

		// Token: 0x04003E0D RID: 15885
		public SlateRef<double> value2;

		// Token: 0x04003E0E RID: 15886
		public QuestNode node;

		// Token: 0x04003E0F RID: 15887
		public QuestNode elseNode;
	}
}
