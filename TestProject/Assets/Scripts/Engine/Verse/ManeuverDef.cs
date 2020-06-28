using System;

namespace Verse
{
	// Token: 0x020000C4 RID: 196
	public class ManeuverDef : Def
	{
		// Token: 0x0400043C RID: 1084
		public ToolCapacityDef requiredCapacity;

		// Token: 0x0400043D RID: 1085
		public VerbProperties verb;

		// Token: 0x0400043E RID: 1086
		public RulePackDef combatLogRulesHit;

		// Token: 0x0400043F RID: 1087
		public RulePackDef combatLogRulesDeflect;

		// Token: 0x04000440 RID: 1088
		public RulePackDef combatLogRulesMiss;

		// Token: 0x04000441 RID: 1089
		public RulePackDef combatLogRulesDodge;

		// Token: 0x04000442 RID: 1090
		public LogEntryDef logEntryDef;
	}
}
