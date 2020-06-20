using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020010EF RID: 4335
	public class QuestNode_Equal : QuestNode
	{
		// Token: 0x060065FD RID: 26109 RVA: 0x0023BF48 File Offset: 0x0023A148
		protected override bool TestRunInt(Slate slate)
		{
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060065FE RID: 26110 RVA: 0x0023BFB0 File Offset: 0x0023A1B0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)))
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

		// Token: 0x04003E03 RID: 15875
		[NoTranslate]
		public SlateRef<object> value1;

		// Token: 0x04003E04 RID: 15876
		[NoTranslate]
		public SlateRef<object> value2;

		// Token: 0x04003E05 RID: 15877
		public SlateRef<Type> compareAs;

		// Token: 0x04003E06 RID: 15878
		public QuestNode node;

		// Token: 0x04003E07 RID: 15879
		public QuestNode elseNode;
	}
}
