using System;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_SetTicksUntilAcceptanceExpiry : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.quest.ticksUntilAcceptanceExpiry = this.ticks.GetValue(slate);
		}

		
		public SlateRef<int> ticks;
	}
}
