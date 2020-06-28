using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001196 RID: 4502
	public class QuestNode_TrackWhenExitMentalState : QuestNode
	{
		// Token: 0x06006848 RID: 26696 RVA: 0x00246EA5 File Offset: 0x002450A5
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Get<Map>("map", null, false) != null;
		}

		// Token: 0x06006849 RID: 26697 RVA: 0x00246EBC File Offset: 0x002450BC
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

		// Token: 0x0400409D RID: 16541
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x0400409E RID: 16542
		public SlateRef<MentalStateDef> mentalStateDef;

		// Token: 0x0400409F RID: 16543
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignals;

		// Token: 0x040040A0 RID: 16544
		[NoTranslate]
		public SlateRef<string> outSignal;
	}
}
