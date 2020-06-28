using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001177 RID: 4471
	public class QuestNode_LeaveOnCleanup : QuestNode
	{
		// Token: 0x060067E6 RID: 26598 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060067E7 RID: 26599 RVA: 0x00245468 File Offset: 0x00243668
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Pawn> value = this.pawns.GetValue(slate);
			if (value.EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_Leave questPart_Leave = new QuestPart_Leave();
			questPart_Leave.pawns.AddRange(value);
			questPart_Leave.sendStandardLetter = (this.sendStandardLetter.GetValue(slate) ?? questPart_Leave.sendStandardLetter);
			questPart_Leave.leaveOnCleanup = true;
			QuestGen.quest.AddPart(questPart_Leave);
		}

		// Token: 0x04004018 RID: 16408
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04004019 RID: 16409
		public SlateRef<bool?> sendStandardLetter;
	}
}
