using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020011AF RID: 4527
	public class QuestNode_RandomRaid : QuestNode
	{
		// Token: 0x060068A1 RID: 26785 RVA: 0x00248996 File Offset: 0x00246B96
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Exists("map", false) && slate.Exists("enemyFaction", false);
		}

		// Token: 0x060068A2 RID: 26786 RVA: 0x00248D1C File Offset: 0x00246F1C
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

		// Token: 0x0400410B RID: 16651
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400410C RID: 16652
		public SlateRef<bool> useCurrentThreatPoints;

		// Token: 0x0400410D RID: 16653
		private static readonly FloatRange RaidPointsRandomFactor = new FloatRange(0.9f, 1.1f);
	}
}
