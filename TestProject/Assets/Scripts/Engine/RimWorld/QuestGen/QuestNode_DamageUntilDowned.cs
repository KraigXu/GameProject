using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001165 RID: 4453
	public class QuestNode_DamageUntilDowned : QuestNode
	{
		// Token: 0x060067A7 RID: 26535 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060067A8 RID: 26536 RVA: 0x00243B94 File Offset: 0x00241D94
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_DamageUntilDowned questPart_DamageUntilDowned = new QuestPart_DamageUntilDowned();
			questPart_DamageUntilDowned.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_DamageUntilDowned.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_DamageUntilDowned.allowBleedingWounds = (this.allowBleedingWounds.GetValue(slate) ?? true);
			QuestGen.quest.AddPart(questPart_DamageUntilDowned);
		}

		// Token: 0x04003FC5 RID: 16325
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04003FC6 RID: 16326
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04003FC7 RID: 16327
		public SlateRef<bool?> allowBleedingWounds;
	}
}
