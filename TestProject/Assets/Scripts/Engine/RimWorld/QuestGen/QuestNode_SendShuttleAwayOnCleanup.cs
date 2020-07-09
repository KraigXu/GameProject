using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_SendShuttleAwayOnCleanup : QuestNode
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
			QuestPart_SendShuttleAwayOnCleanup questPart_SendShuttleAwayOnCleanup = new QuestPart_SendShuttleAwayOnCleanup();
			questPart_SendShuttleAwayOnCleanup.shuttle = this.shuttle.GetValue(slate);
			questPart_SendShuttleAwayOnCleanup.dropEverything = this.dropEverything.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SendShuttleAwayOnCleanup);
		}

		
		public SlateRef<Thing> shuttle;

		
		public SlateRef<bool> dropEverything;
	}
}
