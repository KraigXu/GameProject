using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200115B RID: 4443
	public class QuestNode_AddHediff : QuestNode
	{
		// Token: 0x06006789 RID: 26505 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600678A RID: 26506 RVA: 0x002435F8 File Offset: 0x002417F8
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

		// Token: 0x04003FA4 RID: 16292
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04003FA5 RID: 16293
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04003FA6 RID: 16294
		public SlateRef<HediffDef> hediffDef;

		// Token: 0x04003FA7 RID: 16295
		public SlateRef<IEnumerable<BodyPartDef>> partsToAffect;

		// Token: 0x04003FA8 RID: 16296
		public SlateRef<bool> checkDiseaseContractChance;

		// Token: 0x04003FA9 RID: 16297
		public SlateRef<bool> addToHyperlinks;
	}
}
