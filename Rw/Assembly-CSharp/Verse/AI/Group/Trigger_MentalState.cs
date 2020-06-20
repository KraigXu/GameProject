using System;

namespace Verse.AI.Group
{
	// Token: 0x02000606 RID: 1542
	public class Trigger_MentalState : Trigger
	{
		// Token: 0x06002A2E RID: 10798 RVA: 0x000F6600 File Offset: 0x000F4800
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					if (lord.ownedPawns[i].InMentalState)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
