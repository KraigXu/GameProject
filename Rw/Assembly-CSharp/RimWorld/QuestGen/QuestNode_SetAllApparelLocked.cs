using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200118A RID: 4490
	public class QuestNode_SetAllApparelLocked : QuestNode
	{
		// Token: 0x06006821 RID: 26657 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006822 RID: 26658 RVA: 0x00246534 File Offset: 0x00244734
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_SetAllApparelLocked questPart_SetAllApparelLocked = new QuestPart_SetAllApparelLocked();
			questPart_SetAllApparelLocked.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_SetAllApparelLocked.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_SetAllApparelLocked);
		}

		// Token: 0x0400406F RID: 16495
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04004070 RID: 16496
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
