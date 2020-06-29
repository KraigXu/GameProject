using System;
using Verse.Grammar;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_TextRules : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
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

		
		public SlateRef<RulePack> rules;

		
		public SlateRef<TextRulesTarget> target;
	}
}
