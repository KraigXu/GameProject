using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020011AB RID: 4523
	public class QuestNode_Incident : QuestNode
	{
		// Token: 0x06006895 RID: 26773 RVA: 0x00245B4D File Offset: 0x00243D4D
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x06006896 RID: 26774 RVA: 0x00248344 File Offset: 0x00246544
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

		// Token: 0x040040EA RID: 16618
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040040EB RID: 16619
		public SlateRef<IncidentDef> incidentDef;

		// Token: 0x040040EC RID: 16620
		public SlateRef<string> customLetterLabel;

		// Token: 0x040040ED RID: 16621
		public SlateRef<string> customLetterText;

		// Token: 0x040040EE RID: 16622
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x040040EF RID: 16623
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x040040F0 RID: 16624
		private const string RootSymbol = "root";
	}
}
