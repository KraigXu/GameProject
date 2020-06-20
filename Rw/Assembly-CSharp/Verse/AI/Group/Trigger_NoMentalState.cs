using System;

namespace Verse.AI.Group
{
	// Token: 0x02000607 RID: 1543
	public class Trigger_NoMentalState : Trigger
	{
		// Token: 0x06002A30 RID: 10800 RVA: 0x000F6644 File Offset: 0x000F4844
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					if (lord.ownedPawns[i].InMentalState)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
	}
}
