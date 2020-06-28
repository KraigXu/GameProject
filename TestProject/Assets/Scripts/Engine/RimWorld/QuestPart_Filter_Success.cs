using System;

namespace RimWorld
{
	// Token: 0x02000953 RID: 2387
	public class QuestPart_Filter_Success : QuestPart_Filter
	{
		// Token: 0x0600387C RID: 14460 RVA: 0x0012E3D0 File Offset: 0x0012C5D0
		protected override bool Pass(SignalArgs args)
		{
			QuestEndOutcome questEndOutcome;
			return args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome) && questEndOutcome == QuestEndOutcome.Success;
		}
	}
}
