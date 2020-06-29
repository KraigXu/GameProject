using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_ResolveTextRequests : QuestNode
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
			QuestNode_ResolveTextRequests.Resolve();
		}

		
		public static void Resolve()
		{
			List<QuestTextRequest> textRequestsReadOnly = QuestGen.TextRequestsReadOnly;
			for (int i = 0; i < textRequestsReadOnly.Count; i++)
			{
				try
				{
					List<Rule> list = new List<Rule>();
					list.AddRange(QuestGen.QuestDescriptionRulesReadOnly);
					if (textRequestsReadOnly[i].extraRules != null)
					{
						list.AddRange(textRequestsReadOnly[i].extraRules);
					}
					string obj = QuestGenUtility.ResolveAbsoluteText(list, QuestGen.QuestDescriptionConstantsReadOnly, textRequestsReadOnly[i].keyword, true);
					textRequestsReadOnly[i].setter(obj);
				}
				catch (Exception arg)
				{
					Log.Error("Error while resolving text request: " + arg, false);
				}
			}
			textRequestsReadOnly.Clear();
		}

		
		public SlateRef<RulePack> rules;
	}
}
