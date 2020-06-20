using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001197 RID: 4503
	public class QuestNode_WorkDisabled : QuestNode
	{
		// Token: 0x0600684B RID: 26699 RVA: 0x00246F94 File Offset: 0x00245194
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

		// Token: 0x0600684C RID: 26700 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x040040A1 RID: 16545
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040040A2 RID: 16546
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x040040A3 RID: 16547
		public SlateRef<WorkTags> disabledWorkTags;
	}
}
