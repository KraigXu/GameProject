using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_PawnsKilled : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && (this.node == null || this.node.TestRun(slate));
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MapParent parent = slate.Get<Map>("map", null, false).Parent;
			string text = QuestGen.GenerateNewSignal("PawnOfRaceKilled", true);
			QuestPart_PawnsKilled questPart_PawnsKilled = new QuestPart_PawnsKilled();
			questPart_PawnsKilled.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_PawnsKilled.race = this.race.GetValue(slate);
			questPart_PawnsKilled.requiredInstigatorFaction = Faction.OfPlayer;
			questPart_PawnsKilled.count = this.count.GetValue(slate);
			questPart_PawnsKilled.mapParent = parent;
			questPart_PawnsKilled.outSignalPawnKilled = text;
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_PawnsKilled);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_PawnsKilled.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_PawnsKilled);
			QuestPart_PawnsAvailable questPart_PawnsAvailable = new QuestPart_PawnsAvailable();
			questPart_PawnsAvailable.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (!this.outSignalPawnsNotAvailable.GetValue(slate).NullOrEmpty())
			{
				questPart_PawnsAvailable.outSignalPawnsNotAvailable = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalPawnsNotAvailable.GetValue(slate));
			}
			questPart_PawnsAvailable.race = this.race.GetValue(slate);
			questPart_PawnsAvailable.requiredCount = this.count.GetValue(slate);
			questPart_PawnsAvailable.mapParent = parent;
			questPart_PawnsAvailable.inSignalDecrement = text;
			QuestGen.quest.AddPart(questPart_PawnsAvailable);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		
		[NoTranslate]
		public SlateRef<string> outSignalPawnsNotAvailable;

		
		public SlateRef<ThingDef> race;

		
		public SlateRef<int> count;

		
		public QuestNode node;

		
		private const string PawnOfRaceKilledSignal = "PawnOfRaceKilled";
	}
}
