using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001191 RID: 4497
	public class QuestNode_SituationalThought : QuestNode
	{
		// Token: 0x06006838 RID: 26680 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006839 RID: 26681 RVA: 0x002468F0 File Offset: 0x00244AF0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_SituationalThought questPart_SituationalThought = new QuestPart_SituationalThought();
			questPart_SituationalThought.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SituationalThought.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_SituationalThought.def = this.def.GetValue(slate);
			questPart_SituationalThought.pawn = this.pawn.GetValue(slate);
			questPart_SituationalThought.delayTicks = this.delayTicks.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SituationalThought);
		}

		// Token: 0x04004084 RID: 16516
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04004085 RID: 16517
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x04004086 RID: 16518
		public SlateRef<ThoughtDef> def;

		// Token: 0x04004087 RID: 16519
		public SlateRef<Pawn> pawn;

		// Token: 0x04004088 RID: 16520
		public SlateRef<int> delayTicks;
	}
}
