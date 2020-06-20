using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020010F8 RID: 4344
	public class QuestNode_IsTrue : QuestNode
	{
		// Token: 0x06006616 RID: 26134 RVA: 0x0023C4DB File Offset: 0x0023A6DB
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06006617 RID: 26135 RVA: 0x0023C518 File Offset: 0x0023A718
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate))
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

		// Token: 0x04003E21 RID: 15905
		public SlateRef<bool> value;

		// Token: 0x04003E22 RID: 15906
		public QuestNode node;

		// Token: 0x04003E23 RID: 15907
		public QuestNode elseNode;
	}
}
