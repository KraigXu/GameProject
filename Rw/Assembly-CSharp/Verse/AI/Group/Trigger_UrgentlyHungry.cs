using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000608 RID: 1544
	public class Trigger_UrgentlyHungry : Trigger
	{
		// Token: 0x06002A32 RID: 10802 RVA: 0x000F6688 File Offset: 0x000F4888
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					if (lord.ownedPawns[i].needs.food.CurCategory >= HungerCategory.UrgentlyHungry)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
