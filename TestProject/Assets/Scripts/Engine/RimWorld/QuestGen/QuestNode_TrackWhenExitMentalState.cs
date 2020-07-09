using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_TrackWhenExitMentalState : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Get<Map>("map", null, false) != null;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_TrackWhenExitMentalState questPart_TrackWhenExitMentalState = new QuestPart_TrackWhenExitMentalState();
			questPart_TrackWhenExitMentalState.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_TrackWhenExitMentalState.tag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			questPart_TrackWhenExitMentalState.mentalStateDef = this.mentalStateDef.GetValue(slate);
			questPart_TrackWhenExitMentalState.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignal.GetValue(slate));
			questPart_TrackWhenExitMentalState.inSignals = new List<string>();
			foreach (string signal in this.inSignals.GetValue(slate))
			{
				questPart_TrackWhenExitMentalState.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			QuestGen.quest.AddPart(questPart_TrackWhenExitMentalState);
		}

		
		[NoTranslate]
		public SlateRef<string> tag;

		
		public SlateRef<MentalStateDef> mentalStateDef;

		
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignals;

		
		[NoTranslate]
		public SlateRef<string> outSignal;
	}
}
