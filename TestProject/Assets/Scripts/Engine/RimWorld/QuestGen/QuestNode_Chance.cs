using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001103 RID: 4355
	public class QuestNode_Chance : QuestNode
	{
		// Token: 0x06006637 RID: 26167 RVA: 0x0023CE7C File Offset: 0x0023B07C
		protected override bool TestRunInt(Slate slate)
		{
			if (this.node == null || this.elseNode == null)
			{
				return true;
			}
			if (this.node.TestRun(slate.DeepCopy()))
			{
				this.node.TestRun(slate);
				return true;
			}
			if (this.elseNode.TestRun(slate.DeepCopy()))
			{
				this.elseNode.TestRun(slate);
				return true;
			}
			return false;
		}

		// Token: 0x06006638 RID: 26168 RVA: 0x0023CEE0 File Offset: 0x0023B0E0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (Rand.Chance(this.chance.GetValue(slate)))
			{
				if (this.node == null)
				{
					return;
				}
				if (this.node.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.node.Run();
					return;
				}
				if (this.elseNode != null && this.elseNode.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.elseNode.Run();
					return;
				}
			}
			else
			{
				if (this.elseNode == null)
				{
					return;
				}
				if (this.elseNode.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.elseNode.Run();
					return;
				}
				if (this.node != null && this.node.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.node.Run();
				}
			}
		}

		// Token: 0x04003E45 RID: 15941
		public SlateRef<float> chance;

		// Token: 0x04003E46 RID: 15942
		public QuestNode node;

		// Token: 0x04003E47 RID: 15943
		public QuestNode elseNode;
	}
}
