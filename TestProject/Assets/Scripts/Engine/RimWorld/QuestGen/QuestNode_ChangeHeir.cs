using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001163 RID: 4451
	public class QuestNode_ChangeHeir : QuestNode
	{
		// Token: 0x060067A1 RID: 26529 RVA: 0x00243A70 File Offset: 0x00241C70
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ChangeHeir questPart_ChangeHeir = new QuestPart_ChangeHeir();
			questPart_ChangeHeir.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ChangeHeir.faction = (this.faction.GetValue(slate) ?? this.factionOf.GetValue(slate).Faction);
			questPart_ChangeHeir.holder = this.holder.GetValue(slate);
			questPart_ChangeHeir.heir = this.heir.GetValue(slate);
			QuestGen.quest.AddPart(questPart_ChangeHeir);
		}

		// Token: 0x060067A2 RID: 26530 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04003FBC RID: 16316
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04003FBD RID: 16317
		public SlateRef<Faction> faction;

		// Token: 0x04003FBE RID: 16318
		public SlateRef<Thing> factionOf;

		// Token: 0x04003FBF RID: 16319
		public SlateRef<Pawn> holder;

		// Token: 0x04003FC0 RID: 16320
		public SlateRef<Pawn> heir;
	}
}
