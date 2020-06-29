using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GenerateThreats : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Get<Map>("map", null, false) != null;
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		
		[NoTranslate]
		public SlateRef<string> storeThreatExampleAs;

		
		[NoTranslate]
		public SlateRef<int> threatStartTicks;

		
		public SlateRef<ThreatsGeneratorParams> parms;

		
		public SlateRef<Faction> faction;
	}
}
