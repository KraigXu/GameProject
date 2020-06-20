using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200116A RID: 4458
	public class QuestNode_DropMonumentMarkerCopy : QuestNode
	{
		// Token: 0x060067B6 RID: 26550 RVA: 0x00243DE5 File Offset: 0x00241FE5
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x060067B7 RID: 26551 RVA: 0x00243DF4 File Offset: 0x00241FF4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_DropMonumentMarkerCopy questPart_DropMonumentMarkerCopy = new QuestPart_DropMonumentMarkerCopy();
			questPart_DropMonumentMarkerCopy.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_DropMonumentMarkerCopy.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_DropMonumentMarkerCopy.outSignalResult = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalResult.GetValue(slate));
			QuestGen.quest.AddPart(questPart_DropMonumentMarkerCopy);
		}

		// Token: 0x04003FD0 RID: 16336
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04003FD1 RID: 16337
		[NoTranslate]
		public SlateRef<string> outSignalResult;
	}
}
