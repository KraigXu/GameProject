using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000BFE RID: 3070
	public class SitePartWorker_ConditionCauser_PsychicSuppressor : SitePartWorker_ConditionCauser
	{
		// Token: 0x06004900 RID: 18688 RVA: 0x0018CC7C File Offset: 0x0018AE7C
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			string label = part.conditionCauser.TryGetComp<CompCauseGameCondition_PsychicSuppression>().gender.GetLabel(false);
			outExtraDescriptionRules.Add(new Rule_String("affectedGender", label));
		}
	}
}
