using System;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_EqualOrFail : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)) && (this.node == null || this.node.TestRun(slate));
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)) && this.node != null)
			{
				this.node.Run();
			}
		}

		
		public SlateRef<object> value1;

		
		public SlateRef<object> value2;

		
		public SlateRef<Type> compareAs;

		
		public QuestNode node;
	}
}
