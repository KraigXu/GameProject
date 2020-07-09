using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_ResolveQuestName : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			if (this.rules.GetValue(QuestGen.slate) != null)
			{
				QuestGen.AddQuestNameRules(this.rules.GetValue(QuestGen.slate));
			}
			QuestNode_ResolveQuestName.Resolve();
		}

		
		public static void Resolve()
		{
			string text;
			if (!QuestGen.slate.TryGet<string>("resolvedQuestName", out text, false))
			{
				text = QuestNode_ResolveQuestName.GenerateName().StripTags();
				QuestGen.slate.Set<string>("resolvedQuestName", text, false);
			}
			QuestGen.quest.name = text;
		}

		
		private static string GenerateName()
		{
			GrammarRequest request = default(GrammarRequest);
			request.Rules.AddRange(QuestGen.QuestNameRulesReadOnly);
			foreach (KeyValuePair<string, string> keyValuePair in QuestGen.QuestNameConstantsReadOnly)
			{
				request.Constants.Add(keyValuePair.Key, keyValuePair.Value);
			}
			QuestGenUtility.AddSlateVars(ref request);
			Predicate<string> predicate = (string x) => !Find.QuestManager.QuestsListForReading.Any((Quest y) => y.name == x);
			if (QuestGen.Root.nameMustBeUnique)
			{
				return NameGenerator.GenerateName(request, predicate, false, "questName", null);
			}
			string text = null;
			int i;
			for (i = 0; i < 20; i++)
			{
				text = NameGenerator.GenerateName(request, null, false, "questName", null);
				if (predicate(text))
				{
					break;
				}
			}
			if (i == 20)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Generated duplicate quest name. QuestScriptDef: ",
					QuestGen.Root,
					". Quest name: ",
					text
				}), false);
			}
			return text;
		}

		
		public SlateRef<RulePack> rules;

		
		public const string TextRoot = "questName";

		
		private const int MaxTriesTryAvoidDuplicateName = 20;
	}
}
