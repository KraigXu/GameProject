using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000BFD RID: 3069
	public class SitePartWorker_ConditionCauser_PsychicDroner : SitePartWorker_ConditionCauser
	{
		// Token: 0x060048FE RID: 18686 RVA: 0x0018CC3C File Offset: 0x0018AE3C
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			string label = part.conditionCauser.TryGetComp<CompCauseGameCondition_PsychicEmanation>().gender.GetLabel(false);
			outExtraDescriptionRules.Add(new Rule_String("affectedGender", label));
		}
	}
}
