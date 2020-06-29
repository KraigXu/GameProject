using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_DamageUntilDowned : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<IEnumerable<Pawn>> pawns;

		
		public SlateRef<bool?> allowBleedingWounds;
	}
}
