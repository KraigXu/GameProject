using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_InspectString : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.targets.GetValue(slate).EnumerableNullOrEmpty<ISelectable>())
			{
				return;
			}
			QuestPart_InspectString questPart_InspectString = new QuestPart_InspectString();
			questPart_InspectString.targets.AddRange(this.targets.GetValue(slate));
			questPart_InspectString.inspectString = this.inspectString.GetValue(slate);
			questPart_InspectString.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_InspectString);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		
		public SlateRef<IEnumerable<ISelectable>> targets;

		
		public SlateRef<string> inspectString;
	}
}
