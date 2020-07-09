using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_ChangeHeir : QuestNode
	{
		
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

		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<Faction> faction;

		
		public SlateRef<Thing> factionOf;

		
		public SlateRef<Pawn> holder;

		
		public SlateRef<Pawn> heir;
	}
}
