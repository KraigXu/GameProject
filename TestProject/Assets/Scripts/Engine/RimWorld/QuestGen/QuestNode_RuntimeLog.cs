using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_RuntimeLog : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Log questPart_Log = new QuestPart_Log();
			questPart_Log.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Log.message = this.message.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Log);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		[NoTranslate]
		public SlateRef<string> message;
	}
}
