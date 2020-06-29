using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_AddHediff : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null || this.hediffDef.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_AddHediff questPart_AddHediff = new QuestPart_AddHediff();
			questPart_AddHediff.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_AddHediff.hediffDef = this.hediffDef.GetValue(slate);
			questPart_AddHediff.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_AddHediff.checkDiseaseContractChance = this.checkDiseaseContractChance.GetValue(slate);
			if (this.partsToAffect.GetValue(slate) != null)
			{
				questPart_AddHediff.partsToAffect = new List<BodyPartDef>();
				questPart_AddHediff.partsToAffect.AddRange(this.partsToAffect.GetValue(slate));
			}
			questPart_AddHediff.addToHyperlinks = this.addToHyperlinks.GetValue(slate);
			QuestGen.quest.AddPart(questPart_AddHediff);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<IEnumerable<Pawn>> pawns;

		
		public SlateRef<HediffDef> hediffDef;

		
		public SlateRef<IEnumerable<BodyPartDef>> partsToAffect;

		
		public SlateRef<bool> checkDiseaseContractChance;

		
		public SlateRef<bool> addToHyperlinks;
	}
}
