using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_Incident : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map target = QuestGen.slate.Get<Map>("map", null, false);
			float points = QuestGen.slate.Get<float>("points", 0f, false);
			QuestPart_Incident questPart_Incident = new QuestPart_Incident();
			questPart_Incident.incident = this.incidentDef.GetValue(slate);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.forced = true;
			incidentParms.target = target;
			incidentParms.points = points;
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
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<IncidentDef> incidentDef;

		
		public SlateRef<string> customLetterLabel;

		
		public SlateRef<string> customLetterText;

		
		public SlateRef<RulePack> customLetterLabelRules;

		
		public SlateRef<RulePack> customLetterTextRules;

		
		private const string RootSymbol = "root";
	}
}
