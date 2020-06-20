using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001164 RID: 4452
	public class QuestNode_ChangeNeed : QuestNode
	{
		// Token: 0x060067A4 RID: 26532 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060067A5 RID: 26533 RVA: 0x00243B0C File Offset: 0x00241D0C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ChangeNeed questPart_ChangeNeed = new QuestPart_ChangeNeed();
			questPart_ChangeNeed.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ChangeNeed.pawn = this.pawn.GetValue(slate);
			questPart_ChangeNeed.need = this.need.GetValue(slate);
			questPart_ChangeNeed.offset = this.offset.GetValue(slate);
			QuestGen.quest.AddPart(questPart_ChangeNeed);
		}

		// Token: 0x04003FC1 RID: 16321
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04003FC2 RID: 16322
		public SlateRef<Pawn> pawn;

		// Token: 0x04003FC3 RID: 16323
		public SlateRef<NeedDef> need;

		// Token: 0x04003FC4 RID: 16324
		public SlateRef<float> offset;
	}
}
