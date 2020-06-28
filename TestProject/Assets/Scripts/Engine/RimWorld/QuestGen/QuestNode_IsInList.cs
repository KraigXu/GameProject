using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020010F5 RID: 4341
	public class QuestNode_IsInList : QuestNode
	{
		// Token: 0x0600660D RID: 26125 RVA: 0x0023C2FC File Offset: 0x0023A4FC
		protected override bool TestRunInt(Slate slate)
		{
			if (QuestGenUtility.IsInList(slate, this.name.GetValue(slate), this.value.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600660E RID: 26126 RVA: 0x0023C358 File Offset: 0x0023A558
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestGenUtility.IsInList(slate, this.name.GetValue(slate), this.value.GetValue(slate)))
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

		// Token: 0x04003E17 RID: 15895
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x04003E18 RID: 15896
		public SlateRef<object> value;

		// Token: 0x04003E19 RID: 15897
		public QuestNode node;

		// Token: 0x04003E1A RID: 15898
		public QuestNode elseNode;
	}
}
