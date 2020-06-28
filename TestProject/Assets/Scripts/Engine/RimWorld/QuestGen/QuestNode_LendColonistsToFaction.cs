using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001178 RID: 4472
	public class QuestNode_LendColonistsToFaction : QuestNode
	{
		// Token: 0x060067E9 RID: 26601 RVA: 0x002454E0 File Offset: 0x002436E0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction = new QuestPart_LendColonistsToFaction
			{
				inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				shuttle = this.shuttle.GetValue(slate),
				lendColonistsToFaction = this.lendColonistsToFactionOf.GetValue(slate).Faction,
				returnLentColonistsInTicks = this.returnLentColonistsInTicks.GetValue(slate),
				returnMap = slate.Get<Map>("map", null, false).Parent
			};
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_LendColonistsToFaction.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_LendColonistsToFaction);
		}

		// Token: 0x060067EA RID: 26602 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400401A RID: 16410
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400401B RID: 16411
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x0400401C RID: 16412
		public SlateRef<Thing> shuttle;

		// Token: 0x0400401D RID: 16413
		public SlateRef<Pawn> lendColonistsToFactionOf;

		// Token: 0x0400401E RID: 16414
		public SlateRef<int> returnLentColonistsInTicks;
	}
}
