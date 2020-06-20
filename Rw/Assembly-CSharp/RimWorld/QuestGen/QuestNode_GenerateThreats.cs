using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020011A9 RID: 4521
	public class QuestNode_GenerateThreats : QuestNode
	{
		// Token: 0x0600688F RID: 26767 RVA: 0x00246B9C File Offset: 0x00244D9C
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Get<Map>("map", null, false) != null;
		}

		// Token: 0x06006890 RID: 26768 RVA: 0x00248094 File Offset: 0x00246294
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = slate.Get<Map>("map", null, false);
			QuestPart_ThreatsGenerator questPart_ThreatsGenerator = new QuestPart_ThreatsGenerator();
			questPart_ThreatsGenerator.threatStartTicks = this.threatStartTicks.GetValue(slate);
			questPart_ThreatsGenerator.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_ThreatsGenerator.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			ThreatsGeneratorParams value = this.parms.GetValue(slate);
			value.faction = (this.faction.GetValue(slate) ?? value.faction);
			questPart_ThreatsGenerator.parms = value;
			questPart_ThreatsGenerator.mapParent = map.Parent;
			QuestGen.quest.AddPart(questPart_ThreatsGenerator);
			if (!this.storeThreatExampleAs.GetValue(slate).NullOrEmpty())
			{
				PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
				pawnGroupMakerParms.groupKind = PawnGroupKindDefOf.Combat;
				pawnGroupMakerParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
				PawnGroupMakerParms pawnGroupMakerParms2 = pawnGroupMakerParms;
				Faction faction;
				if ((faction = value.faction) == null)
				{
					faction = (from x in Find.FactionManager.GetFactions(false, false, true, TechLevel.Industrial)
					where x.HostileTo(Faction.OfPlayer)
					select x).RandomElement<Faction>();
				}
				pawnGroupMakerParms2.faction = faction;
				float num = value.threatPoints ?? (StorytellerUtility.DefaultThreatPointsNow(map) * value.currentThreatPointsFactor);
				if (value.minThreatPoints != null)
				{
					num = Mathf.Max(num, value.minThreatPoints.Value);
				}
				pawnGroupMakerParms.points = IncidentWorker_Raid.AdjustedRaidPoints(num, PawnsArrivalModeDefOf.EdgeWalkIn, RaidStrategyDefOf.ImmediateAttack, pawnGroupMakerParms.faction, PawnGroupKindDefOf.Combat);
				IEnumerable<PawnKindDef> pawnKinds = PawnGroupMakerUtility.GeneratePawnKindsExample(pawnGroupMakerParms);
				slate.Set<string>(this.storeThreatExampleAs.GetValue(slate), PawnUtility.PawnKindsToLineList(pawnKinds, "  - "), false);
			}
		}

		// Token: 0x040040E1 RID: 16609
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040040E2 RID: 16610
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x040040E3 RID: 16611
		[NoTranslate]
		public SlateRef<string> storeThreatExampleAs;

		// Token: 0x040040E4 RID: 16612
		[NoTranslate]
		public SlateRef<int> threatStartTicks;

		// Token: 0x040040E5 RID: 16613
		public SlateRef<ThreatsGeneratorParams> parms;

		// Token: 0x040040E6 RID: 16614
		public SlateRef<Faction> faction;
	}
}
