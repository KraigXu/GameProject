using System;

namespace RimWorld
{
	// Token: 0x02000954 RID: 2388
	public class QuestPart_Filter_UnknownOutcome : QuestPart_Filter
	{
		// Token: 0x0600387E RID: 14462 RVA: 0x0012E3F4 File Offset: 0x0012C5F4
		protected override bool Pass(SignalArgs args)
		{
			QuestEndOutcome questEndOutcome;
			return !args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome) || questEndOutcome == QuestEndOutcome.Unknown;
		}
	}
}
