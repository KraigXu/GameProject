using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_LendColonistsToFaction : QuestNode
	{
		
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

		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		
		public SlateRef<Thing> shuttle;

		
		public SlateRef<Pawn> lendColonistsToFactionOf;

		
		public SlateRef<int> returnLentColonistsInTicks;
	}
}
