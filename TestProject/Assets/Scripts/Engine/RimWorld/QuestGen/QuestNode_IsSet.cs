using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020010F7 RID: 4343
	public class QuestNode_IsSet : QuestNode
	{
		// Token: 0x06006613 RID: 26131 RVA: 0x0023C438 File Offset: 0x0023A638
		protected override bool TestRunInt(Slate slate)
		{
			if (slate.Exists(this.name.GetValue(slate), false))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06006614 RID: 26132 RVA: 0x0023C488 File Offset: 0x0023A688
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestGen.slate.Exists(this.name.GetValue(slate), false))
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

		// Token: 0x04003E1E RID: 15902
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x04003E1F RID: 15903
		public QuestNode node;

		// Token: 0x04003E20 RID: 15904
		public QuestNode elseNode;
	}
}
