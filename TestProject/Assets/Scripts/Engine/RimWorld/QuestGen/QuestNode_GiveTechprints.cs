using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001173 RID: 4467
	public class QuestNode_GiveTechprints : QuestNode
	{
		// Token: 0x060067D9 RID: 26585 RVA: 0x0024511C File Offset: 0x0024331C
		protected override bool TestRunInt(Slate slate)
		{
			ResearchProjectDef researchProjectDef = this.FindTargetProject(slate);
			if (researchProjectDef == null || researchProjectDef.TechprintRequirementMet)
			{
				return false;
			}
			if (this.storeProjectAs.GetValue(slate) != null)
			{
				slate.Set<ResearchProjectDef>(this.storeProjectAs.GetValue(slate), researchProjectDef, false);
			}
			return true;
		}

		// Token: 0x060067DA RID: 26586 RVA: 0x00245164 File Offset: 0x00243364
		private ResearchProjectDef FindTargetProject(Slate slate)
		{
			if (this.fixedProject.GetValue(slate) != null)
			{
				return this.fixedProject.GetValue(slate);
			}
			return (from p in DefDatabase<ResearchProjectDef>.AllDefsListForReading
			where !p.IsFinished && !p.TechprintRequirementMet
			select p).RandomElement<ResearchProjectDef>();
		}

		// Token: 0x060067DB RID: 26587 RVA: 0x002451BC File Offset: 0x002433BC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			ResearchProjectDef researchProjectDef = this.FindTargetProject(slate);
			QuestPart_GiveTechprints questPart_GiveTechprints = new QuestPart_GiveTechprints();
			questPart_GiveTechprints.amount = 1;
			questPart_GiveTechprints.project = researchProjectDef;
			questPart_GiveTechprints.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_GiveTechprints.outSignalWasGiven = QuestGenUtility.HardcodedSignalWithQuestID("AddedTechprints");
			QuestGen.quest.AddPart(questPart_GiveTechprints);
			if (this.storeProjectAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<ResearchProjectDef>(this.storeProjectAs.GetValue(slate), researchProjectDef, false);
			}
		}

		// Token: 0x0400400A RID: 16394
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400400B RID: 16395
		public SlateRef<ResearchProjectDef> fixedProject;

		// Token: 0x0400400C RID: 16396
		[NoTranslate]
		public SlateRef<string> storeProjectAs;
	}
}
