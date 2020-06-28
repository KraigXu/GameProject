using System;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020011A1 RID: 4513
	public class QuestNode_ResolveQuestDescription : QuestNode
	{
		// Token: 0x06006874 RID: 26740 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006875 RID: 26741 RVA: 0x00247942 File Offset: 0x00245B42
		protected override void RunInt()
		{
			if (this.rules.GetValue(QuestGen.slate) != null)
			{
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(QuestGen.slate));
			}
			QuestNode_ResolveQuestDescription.Resolve();
		}

		// Token: 0x06006876 RID: 26742 RVA: 0x00247970 File Offset: 0x00245B70
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

		// Token: 0x040040C4 RID: 16580
		public SlateRef<RulePack> rules;

		// Token: 0x040040C5 RID: 16581
		public const string TextRoot = "questDescription";
	}
}
