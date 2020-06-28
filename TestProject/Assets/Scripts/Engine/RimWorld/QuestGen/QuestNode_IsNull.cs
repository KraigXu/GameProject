using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020010F6 RID: 4342
	public class QuestNode_IsNull : QuestNode
	{
		// Token: 0x06006610 RID: 26128 RVA: 0x0023C3B2 File Offset: 0x0023A5B2
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) == null)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06006611 RID: 26129 RVA: 0x0023C3F0 File Offset: 0x0023A5F0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate) == null)
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

		// Token: 0x04003E1B RID: 15899
		[NoTranslate]
		public SlateRef<object> value;

		// Token: 0x04003E1C RID: 15900
		public QuestNode node;

		// Token: 0x04003E1D RID: 15901
		public QuestNode elseNode;
	}
}
