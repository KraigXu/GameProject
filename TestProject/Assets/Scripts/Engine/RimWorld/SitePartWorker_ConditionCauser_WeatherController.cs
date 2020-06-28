using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000BFB RID: 3067
	public class SitePartWorker_ConditionCauser_WeatherController : SitePartWorker_ConditionCauser
	{
		// Token: 0x060048FA RID: 18682 RVA: 0x0018CBB4 File Offset: 0x0018ADB4
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			WeatherDef weather = part.conditionCauser.TryGetComp<CompCauseGameCondition_ForceWeather>().weather;
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("weather", weather));
		}
	}
}
