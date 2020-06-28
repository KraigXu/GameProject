using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020011AE RID: 4526
	public class QuestNode_Raid : QuestNode
	{
		// Token: 0x0600689E RID: 26782 RVA: 0x00248996 File Offset: 0x00246B96
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Exists("map", false) && slate.Exists("enemyFaction", false);
		}

		// Token: 0x0600689F RID: 26783 RVA: 0x002489CC File Offset: 0x00246BCC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map target = QuestGen.slate.Get<Map>("map", null, false);
			float a = QuestGen.slate.Get<float>("points", 0f, false);
			Faction faction = QuestGen.slate.Get<Faction>("enemyFaction", null, false);
			QuestPart_Incident questPart_Incident = new QuestPart_Incident();
			questPart_Incident.debugLabel = "raid";
			questPart_Incident.incident = IncidentDefOf.RaidEnemy;
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.forced = true;
			incidentParms.target = target;
			incidentParms.points = Mathf.Max(a, faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat));
			incidentParms.faction = faction;
			incidentParms.pawnGroupMakerSeed = new int?(Rand.Int);
			incidentParms.inSignalEnd = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalLeave.GetValue(slate));
			incidentParms.questTag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					incidentParms.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					incidentParms.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			IncidentWorker_Raid incidentWorker_Raid = (IncidentWorker_Raid)questPart_Incident.incident.Worker;
			incidentWorker_Raid.ResolveRaidStrategy(incidentParms, PawnGroupKindDefOf.Combat);
			incidentWorker_Raid.ResolveRaidArriveMode(incidentParms);
			if (incidentParms.raidArrivalMode.walkIn)
			{
				incidentParms.spawnCenter = (this.walkInSpot.GetValue(slate) ?? (QuestGen.slate.Get<IntVec3?>("walkInSpot", null, false) ?? IntVec3.Invalid));
			}
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, incidentParms, false);
			defaultPawnGroupMakerParms.points = IncidentWorker_Raid.AdjustedRaidPoints(defaultPawnGroupMakerParms.points, incidentParms.raidArrivalMode, incidentParms.raidStrategy, defaultPawnGroupMakerParms.faction, PawnGroupKindDefOf.Combat);
			IEnumerable<PawnKindDef> pawnKinds = PawnGroupMakerUtility.GeneratePawnKindsExample(defaultPawnGroupMakerParms);
			questPart_Incident.SetIncidentParmsAndRemoveTarget(incidentParms);
			questPart_Incident.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_Incident);
			QuestGen.AddQuestDescriptionRules(new List<Rule>
			{
				new Rule_String("raidPawnKinds", PawnUtility.PawnKindsToLineList(pawnKinds, "  - ")),
				new Rule_String("raidArrivalModeInfo", incidentParms.raidArrivalMode.textWillArrive.Formatted(faction))
			});
		}

		// Token: 0x04004102 RID: 16642
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04004103 RID: 16643
		public SlateRef<IntVec3?> walkInSpot;

		// Token: 0x04004104 RID: 16644
		public SlateRef<string> customLetterLabel;

		// Token: 0x04004105 RID: 16645
		public SlateRef<string> customLetterText;

		// Token: 0x04004106 RID: 16646
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x04004107 RID: 16647
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x04004108 RID: 16648
		[NoTranslate]
		public SlateRef<string> inSignalLeave;

		// Token: 0x04004109 RID: 16649
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x0400410A RID: 16650
		private const string RootSymbol = "root";
	}
}
