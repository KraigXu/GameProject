using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020011AD RID: 4525
	public class QuestNode_ManhunterPack : QuestNode
	{
		// Token: 0x0600689B RID: 26779 RVA: 0x00248688 File Offset: 0x00246888
		protected override bool TestRunInt(Slate slate)
		{
			PawnKindDef pawnKindDef;
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Exists("map", false) && ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(slate.Get<float>("points", 0f, false), slate.Get<Map>("map", null, false).Tile, out pawnKindDef);
		}

		// Token: 0x0600689C RID: 26780 RVA: 0x002486E8 File Offset: 0x002468E8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			float points = QuestGen.slate.Get<float>("points", 0f, false);
			QuestPart_Incident questPart_Incident = new QuestPart_Incident();
			questPart_Incident.incident = IncidentDefOf.ManhunterPack;
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.forced = true;
			incidentParms.target = map;
			incidentParms.points = points;
			incidentParms.questTag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			incidentParms.spawnCenter = (this.walkInSpot.GetValue(slate) ?? (QuestGen.slate.Get<IntVec3?>("walkInSpot", null, false) ?? IntVec3.Invalid));
			incidentParms.pawnCount = this.animalCount.GetValue(slate);
			PawnKindDef pawnKindDef;
			if (ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(points, map.Tile, out pawnKindDef))
			{
				incidentParms.pawnKind = pawnKindDef;
			}
			slate.Set<PawnKindDef>("animalKindDef", pawnKindDef, false);
			int num = (incidentParms.pawnCount > 0) ? incidentParms.pawnCount : ManhunterPackIncidentUtility.GetAnimalsCount(pawnKindDef, points);
			QuestGen.slate.Set<int>("animalCount", num, false);
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
			questPart_Incident.SetIncidentParmsAndRemoveTarget(incidentParms);
			questPart_Incident.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_Incident);
			List<Rule> list = new List<Rule>();
			list.Add(new Rule_String("animalKind_label", pawnKindDef.label));
			list.Add(new Rule_String("animalKind_labelPlural", pawnKindDef.GetLabelPlural(num)));
			QuestGen.AddQuestDescriptionRules(list);
			QuestGen.AddQuestNameRules(list);
		}

		// Token: 0x040040F9 RID: 16633
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040040FA RID: 16634
		public SlateRef<string> customLetterLabel;

		// Token: 0x040040FB RID: 16635
		public SlateRef<string> customLetterText;

		// Token: 0x040040FC RID: 16636
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x040040FD RID: 16637
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x040040FE RID: 16638
		public SlateRef<IntVec3?> walkInSpot;

		// Token: 0x040040FF RID: 16639
		public SlateRef<int> animalCount;

		// Token: 0x04004100 RID: 16640
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x04004101 RID: 16641
		private const string RootSymbol = "root";
	}
}
