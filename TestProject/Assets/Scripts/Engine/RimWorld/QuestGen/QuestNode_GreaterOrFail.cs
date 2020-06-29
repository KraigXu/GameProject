using System;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GreaterOrFail : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.value1.GetValue(slate) > this.value2.GetValue(slate) && (this.node == null || this.node.TestRun(slate));
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate) && this.node != null)
			{
				this.node.Run();
			}
		}

		
		public SlateRef<double> value1;

		
		public SlateRef<double> value2;

		
		public QuestNode node;
	}
}
