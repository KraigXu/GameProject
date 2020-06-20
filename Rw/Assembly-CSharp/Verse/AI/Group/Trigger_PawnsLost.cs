using System;

namespace Verse.AI.Group
{
	// Token: 0x020005F9 RID: 1529
	public class Trigger_PawnsLost : Trigger
	{
		// Token: 0x06002A12 RID: 10770 RVA: 0x000F60E1 File Offset: 0x000F42E1
		public Trigger_PawnsLost(int count)
		{
			this.count = count;
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x000F60F7 File Offset: 0x000F42F7
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && lord.numPawnsLostViolently >= this.count;
		}

		// Token: 0x04001923 RID: 6435
		private int count = 1;
	}
}
