using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_DropMonumentMarkerCopy : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_DropMonumentMarkerCopy questPart_DropMonumentMarkerCopy = new QuestPart_DropMonumentMarkerCopy();
			questPart_DropMonumentMarkerCopy.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_DropMonumentMarkerCopy.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_DropMonumentMarkerCopy.outSignalResult = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalResult.GetValue(slate));
			QuestGen.quest.AddPart(questPart_DropMonumentMarkerCopy);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		[NoTranslate]
		public SlateRef<string> outSignalResult;
	}
}
