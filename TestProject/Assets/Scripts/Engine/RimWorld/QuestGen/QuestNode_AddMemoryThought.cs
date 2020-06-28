using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200115D RID: 4445
	public class QuestNode_AddMemoryThought : QuestNode
	{
		// Token: 0x0600678F RID: 26511 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006790 RID: 26512 RVA: 0x00243768 File Offset: 0x00241968
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) != null)
			{
				foreach (Pawn pawn in this.pawns.GetValue(slate))
				{
					QuestPart_AddMemoryThought questPart_AddMemoryThought = new QuestPart_AddMemoryThought();
					questPart_AddMemoryThought.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
					questPart_AddMemoryThought.def = this.def.GetValue(slate);
					questPart_AddMemoryThought.pawn = pawn;
					questPart_AddMemoryThought.otherPawn = this.otherPawn.GetValue(slate);
					questPart_AddMemoryThought.addToLookTargets = (this.addToLookTargets.GetValue(slate) ?? true);
					QuestGen.quest.AddPart(questPart_AddMemoryThought);
				}
			}
		}

		// Token: 0x04003FAC RID: 16300
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04003FAD RID: 16301
		public SlateRef<ThoughtDef> def;

		// Token: 0x04003FAE RID: 16302
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04003FAF RID: 16303
		public SlateRef<Pawn> otherPawn;

		// Token: 0x04003FB0 RID: 16304
		public SlateRef<bool?> addToLookTargets;
	}
}
