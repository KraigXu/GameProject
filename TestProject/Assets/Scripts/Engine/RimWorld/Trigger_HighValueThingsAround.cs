using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007B8 RID: 1976
	public class Trigger_HighValueThingsAround : Trigger
	{
		// Token: 0x06003336 RID: 13110 RVA: 0x0011BEF0 File Offset: 0x0011A0F0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 120 == 0)
			{
				if (TutorSystem.TutorialMode)
				{
					return false;
				}
				if (Find.TickManager.TicksGame - lord.lastPawnHarmTick > 300)
				{
					float num = StealAIUtility.TotalMarketValueAround(lord.ownedPawns);
					float num2 = StealAIUtility.StartStealingMarketValueThreshold(lord);
					return num > num2;
				}
			}
			return false;
		}

		// Token: 0x04001B8F RID: 7055
		private const int CheckInterval = 120;

		// Token: 0x04001B90 RID: 7056
		private const int MinTicksSinceDamage = 300;
	}
}
