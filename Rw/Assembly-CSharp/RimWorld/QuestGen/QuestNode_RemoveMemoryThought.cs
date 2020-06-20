using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001184 RID: 4484
	public class QuestNode_RemoveMemoryThought : QuestNode
	{
		// Token: 0x0600680F RID: 26639 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006810 RID: 26640 RVA: 0x0024624C File Offset: 0x0024444C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_RemoveMemoryThought questPart_RemoveMemoryThought = new QuestPart_RemoveMemoryThought();
			questPart_RemoveMemoryThought.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_RemoveMemoryThought.def = this.def.GetValue(slate);
			questPart_RemoveMemoryThought.pawn = this.pawn.GetValue(slate);
			questPart_RemoveMemoryThought.count = this.count.GetValue(slate);
			questPart_RemoveMemoryThought.otherPawn = this.otherPawn.GetValue(slate);
			questPart_RemoveMemoryThought.addToLookTargets = (this.addToLookTargets.GetValue(slate) ?? true);
			QuestGen.quest.AddPart(questPart_RemoveMemoryThought);
		}

		// Token: 0x0400405F RID: 16479
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04004060 RID: 16480
		public SlateRef<ThoughtDef> def;

		// Token: 0x04004061 RID: 16481
		public SlateRef<Pawn> pawn;

		// Token: 0x04004062 RID: 16482
		public SlateRef<Pawn> otherPawn;

		// Token: 0x04004063 RID: 16483
		public SlateRef<int?> count;

		// Token: 0x04004064 RID: 16484
		public SlateRef<bool?> addToLookTargets;
	}
}
