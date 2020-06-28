using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200116C RID: 4460
	public class QuestNode_EndGame : QuestNode
	{
		// Token: 0x060067BC RID: 26556 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060067BD RID: 26557 RVA: 0x00244098 File Offset: 0x00242298
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_EndGame endGame = new QuestPart_EndGame();
			endGame.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			endGame.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				endGame.introText = x;
			}, QuestGenUtility.MergeRules(this.introTextRules.GetValue(slate), this.introText.GetValue(slate), "root"));
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				endGame.endingText = x;
			}, QuestGenUtility.MergeRules(this.endingTextRules.GetValue(slate), this.endingText.GetValue(slate), "root"));
			QuestGen.quest.AddPart(endGame);
		}

		// Token: 0x04003FDE RID: 16350
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04003FDF RID: 16351
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x04003FE0 RID: 16352
		public SlateRef<string> introText;

		// Token: 0x04003FE1 RID: 16353
		public SlateRef<string> endingText;

		// Token: 0x04003FE2 RID: 16354
		public SlateRef<RulePack> introTextRules;

		// Token: 0x04003FE3 RID: 16355
		public SlateRef<RulePack> endingTextRules;

		// Token: 0x04003FE4 RID: 16356
		private const string RootSymbol = "root";
	}
}
