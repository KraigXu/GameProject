using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001176 RID: 4470
	public class QuestNode_Leave : QuestNode
	{
		// Token: 0x060067E3 RID: 26595 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060067E4 RID: 26596 RVA: 0x002453A0 File Offset: 0x002435A0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Pawn> value = this.pawns.GetValue(slate);
			if (value.EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_Leave questPart_Leave = new QuestPart_Leave();
			questPart_Leave.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Leave.pawns.AddRange(value);
			questPart_Leave.sendStandardLetter = (this.sendStandardLetter.GetValue(slate) ?? questPart_Leave.sendStandardLetter);
			questPart_Leave.leaveOnCleanup = (this.leaveOnCleanup.GetValue(slate) ?? questPart_Leave.leaveOnCleanup);
			QuestGen.quest.AddPart(questPart_Leave);
		}

		// Token: 0x04004014 RID: 16404
		public SlateRef<string> inSignal;

		// Token: 0x04004015 RID: 16405
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04004016 RID: 16406
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x04004017 RID: 16407
		public SlateRef<bool?> leaveOnCleanup;
	}
}
