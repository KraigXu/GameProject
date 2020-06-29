using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_SendShuttleAway : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.shuttle.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_SendShuttleAway questPart_SendShuttleAway = new QuestPart_SendShuttleAway();
			questPart_SendShuttleAway.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SendShuttleAway.shuttle = this.shuttle.GetValue(slate);
			questPart_SendShuttleAway.dropEverything = this.dropEverything.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SendShuttleAway);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<Thing> shuttle;

		
		public SlateRef<bool> dropEverything;
	}
}
