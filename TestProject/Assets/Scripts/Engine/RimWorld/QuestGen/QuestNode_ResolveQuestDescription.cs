using System;
using Verse.Grammar;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_ResolveQuestDescription : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			if (this.rules.GetValue(QuestGen.slate) != null)
			{
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(QuestGen.slate));
			}
			QuestNode_ResolveQuestDescription.Resolve();
		}

		
		public static void Resolve()
		{
			string text;
			if (!QuestGen.slate.TryGet<string>("resolvedQuestDescription", out text, false))
			{
				text = QuestGenUtility.ResolveAbsoluteText(QuestGen.QuestDescriptionRulesReadOnly, QuestGen.QuestDescriptionConstantsReadOnly, "questDescription", true);
				QuestGen.slate.Set<string>("resolvedQuestDescription", text, false);
			}
			QuestGen.quest.description = text;
		}

		
		public SlateRef<RulePack> rules;

		
		public const string TextRoot = "questDescription";
	}
}
