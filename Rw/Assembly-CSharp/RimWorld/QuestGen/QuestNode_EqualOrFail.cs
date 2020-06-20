using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020010F1 RID: 4337
	public class QuestNode_EqualOrFail : QuestNode
	{
		// Token: 0x06006601 RID: 26113 RVA: 0x0023C094 File Offset: 0x0023A294
		protected override bool TestRunInt(Slate slate)
		{
			return QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)) && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x06006602 RID: 26114 RVA: 0x0023C0E4 File Offset: 0x0023A2E4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)) && this.node != null)
			{
				this.node.Run();
			}
		}

		// Token: 0x04003E08 RID: 15880
		public SlateRef<object> value1;

		// Token: 0x04003E09 RID: 15881
		public SlateRef<object> value2;

		// Token: 0x04003E0A RID: 15882
		public SlateRef<Type> compareAs;

		// Token: 0x04003E0B RID: 15883
		public QuestNode node;
	}
}
