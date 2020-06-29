using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_WorkDisabled : QuestNode
	{
		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_WorkDisabled questPart_WorkDisabled = new QuestPart_WorkDisabled();
			questPart_WorkDisabled.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_WorkDisabled.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_WorkDisabled.disabledWorkTags = this.disabledWorkTags.GetValue(slate);
			QuestGen.quest.AddPart(questPart_WorkDisabled);
		}

		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		
		public SlateRef<IEnumerable<Pawn>> pawns;

		
		public SlateRef<WorkTags> disabledWorkTags;
	}
}
