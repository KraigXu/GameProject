using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020010F9 RID: 4345
	public class QuestNode_IsTrueOrUnset : QuestNode
	{
		// Token: 0x06006619 RID: 26137 RVA: 0x0023C560 File Offset: 0x0023A760
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) ?? true)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600661A RID: 26138 RVA: 0x0023C5BC File Offset: 0x0023A7BC
		protected override void RunInt()
		{
			if (this.value.GetValue(QuestGen.slate) ?? true)
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

		// Token: 0x04003E24 RID: 15908
		public SlateRef<bool?> value;

		// Token: 0x04003E25 RID: 15909
		public QuestNode node;

		// Token: 0x04003E26 RID: 15910
		public QuestNode elseNode;
	}
}
