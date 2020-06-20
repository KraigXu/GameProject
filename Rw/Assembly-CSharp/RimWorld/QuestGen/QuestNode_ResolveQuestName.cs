using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020011A2 RID: 4514
	public class QuestNode_ResolveQuestName : QuestNode
	{
		// Token: 0x06006878 RID: 26744 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006879 RID: 26745 RVA: 0x002479C8 File Offset: 0x00245BC8
		protected override void RunInt()
		{
			if (this.rules.GetValue(QuestGen.slate) != null)
			{
				QuestGen.AddQuestNameRules(this.rules.GetValue(QuestGen.slate));
			}
			QuestNode_ResolveQuestName.Resolve();
		}

		// Token: 0x0600687A RID: 26746 RVA: 0x002479F8 File Offset: 0x00245BF8
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

		// Token: 0x0600687B RID: 26747 RVA: 0x00247A40 File Offset: 0x00245C40
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

		// Token: 0x040040C6 RID: 16582
		public SlateRef<RulePack> rules;

		// Token: 0x040040C7 RID: 16583
		public const string TextRoot = "questName";

		// Token: 0x040040C8 RID: 16584
		private const int MaxTriesTryAvoidDuplicateName = 20;
	}
}
