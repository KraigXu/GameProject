using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200117C RID: 4476
	public class QuestNode_Message : QuestNode
	{
		// Token: 0x060067F5 RID: 26613 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060067F6 RID: 26614 RVA: 0x002458D4 File Offset: 0x00243AD4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Message message = new QuestPart_Message();
			message.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			message.messageType = (this.messageType.GetValue(slate) ?? MessageTypeDefOf.NeutralEvent);
			message.lookTargets = QuestGenUtility.ToLookTargets(this.lookTargets, slate);
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				message.message = x;
			}, QuestGenUtility.MergeRules(this.rules.GetValue(slate), this.text.GetValue(slate), "root"));
			QuestGen.quest.AddPart(message);
		}

		// Token: 0x04004032 RID: 16434
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04004033 RID: 16435
		public SlateRef<MessageTypeDef> messageType;

		// Token: 0x04004034 RID: 16436
		public SlateRef<string> text;

		// Token: 0x04004035 RID: 16437
		public SlateRef<RulePack> rules;

		// Token: 0x04004036 RID: 16438
		[NoTranslate]
		public SlateRef<IEnumerable<object>> lookTargets;

		// Token: 0x04004037 RID: 16439
		private const string RootSymbol = "root";
	}
}
