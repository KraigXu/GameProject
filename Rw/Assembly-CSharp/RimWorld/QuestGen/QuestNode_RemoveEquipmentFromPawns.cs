using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001183 RID: 4483
	public class QuestNode_RemoveEquipmentFromPawns : QuestNode
	{
		// Token: 0x0600680C RID: 26636 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600680D RID: 26637 RVA: 0x002461D4 File Offset: 0x002443D4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_RemoveEquipmentFromPawns questPart_RemoveEquipmentFromPawns = new QuestPart_RemoveEquipmentFromPawns();
			questPart_RemoveEquipmentFromPawns.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_RemoveEquipmentFromPawns.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_RemoveEquipmentFromPawns);
		}

		// Token: 0x0400405D RID: 16477
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400405E RID: 16478
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
