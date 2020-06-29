using System;

namespace RimWorld
{
	
	public class QuestPart_Filter_UnknownOutcome : QuestPart_Filter
	{
		
		protected override bool Pass(SignalArgs args)
		{
			QuestEndOutcome questEndOutcome;
			return !args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome) || questEndOutcome == QuestEndOutcome.Unknown;
		}
	}
}
