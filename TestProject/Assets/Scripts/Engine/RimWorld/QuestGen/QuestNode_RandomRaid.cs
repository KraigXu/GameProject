using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_RandomRaid : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Exists("map", false) && slate.Exists("enemyFaction", false);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			float val = QuestGen.slate.Get<float>("points", 0f, false);
			Faction faction = QuestGen.slate.Get<Faction>("enemyFaction", null, false);
			QuestPart_RandomRaid questPart_RandomRaid = new QuestPart_RandomRaid();
			questPart_RandomRaid.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_RandomRaid.mapParent = map.Parent;
			questPart_RandomRaid.faction = faction;
			questPart_RandomRaid.pointsRange = QuestNode_RandomRaid.RaidPointsRandomFactor * val;
			questPart_RandomRaid.useCurrentThreatPoints = this.useCurrentThreatPoints.GetValue(slate);
			QuestGen.quest.AddPart(questPart_RandomRaid);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<bool> useCurrentThreatPoints;

		
		private static readonly FloatRange RaidPointsRandomFactor = new FloatRange(0.9f, 1.1f);
	}
}
