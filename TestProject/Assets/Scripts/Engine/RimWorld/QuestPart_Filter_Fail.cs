using System;

namespace RimWorld
{
	// Token: 0x02000952 RID: 2386
	public class QuestPart_Filter_Fail : QuestPart_Filter
	{
		// Token: 0x0600387A RID: 14458 RVA: 0x0012E3AC File Offset: 0x0012C5AC
		protected override bool Pass(SignalArgs args)
		{
			QuestEndOutcome questEndOutcome;
			return args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome) && questEndOutcome == QuestEndOutcome.Fail;
		}
	}
}
