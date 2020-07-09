using System;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_ReplaceLostLeaderReferences : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ReplaceLostLeaderReferences questPart_ReplaceLostLeaderReferences = new QuestPart_ReplaceLostLeaderReferences();
			questPart_ReplaceLostLeaderReferences.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
			questPart_ReplaceLostLeaderReferences.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
			QuestGen.quest.AddPart(questPart_ReplaceLostLeaderReferences);
		}

		
		public SlateRef<string> inSignal;

		
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
