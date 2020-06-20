using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020010FA RID: 4346
	public class QuestNode_IsZero : QuestNode
	{
		// Token: 0x0600661C RID: 26140 RVA: 0x0023C618 File Offset: 0x0023A818
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) == 0.0)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600661D RID: 26141 RVA: 0x0023C66C File Offset: 0x0023A86C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate) == 0.0)
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

		// Token: 0x04003E27 RID: 15911
		public SlateRef<double> value;

		// Token: 0x04003E28 RID: 15912
		public QuestNode node;

		// Token: 0x04003E29 RID: 15913
		public QuestNode elseNode;
	}
}
