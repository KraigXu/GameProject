using System;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020011A6 RID: 4518
	public class QuestNode_TextRules : QuestNode
	{
		// Token: 0x06006884 RID: 26756 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006885 RID: 26757 RVA: 0x00247C8C File Offset: 0x00245E8C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			switch (this.target.GetValue(slate))
			{
			case TextRulesTarget.Description:
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(slate));
				return;
			case TextRulesTarget.Name:
				QuestGen.AddQuestNameRules(this.rules.GetValue(slate));
				return;
			case TextRulesTarget.DecriptionAndName:
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(slate));
				QuestGen.AddQuestNameRules(this.rules.GetValue(slate));
				return;
			default:
				return;
			}
		}

		// Token: 0x040040D1 RID: 16593
		public SlateRef<RulePack> rules;

		// Token: 0x040040D2 RID: 16594
		public SlateRef<TextRulesTarget> target;
	}
}
