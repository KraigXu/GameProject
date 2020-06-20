using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001162 RID: 4450
	public class QuestNode_ChangeFactionGoodwill : QuestNode
	{
		// Token: 0x0600679E RID: 26526 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600679F RID: 26527 RVA: 0x002439E4 File Offset: 0x00241BE4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_FactionGoodwillChange questPart_FactionGoodwillChange = new QuestPart_FactionGoodwillChange();
			questPart_FactionGoodwillChange.change = this.change.GetValue(slate);
			questPart_FactionGoodwillChange.faction = (this.faction.GetValue(slate) ?? this.factionOf.GetValue(slate).Faction);
			questPart_FactionGoodwillChange.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_FactionGoodwillChange);
		}

		// Token: 0x04003FB8 RID: 16312
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04003FB9 RID: 16313
		public SlateRef<Faction> faction;

		// Token: 0x04003FBA RID: 16314
		public SlateRef<Thing> factionOf;

		// Token: 0x04003FBB RID: 16315
		public SlateRef<int> change;
	}
}
