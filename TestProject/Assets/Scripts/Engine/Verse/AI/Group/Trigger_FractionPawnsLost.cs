using System;

namespace Verse.AI.Group
{
	// Token: 0x020005F8 RID: 1528
	public class Trigger_FractionPawnsLost : Trigger
	{
		// Token: 0x06002A10 RID: 10768 RVA: 0x000F60A0 File Offset: 0x000F42A0
		public Trigger_FractionPawnsLost(float fraction)
		{
			this.fraction = fraction;
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x000F60BA File Offset: 0x000F42BA
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (float)lord.numPawnsLostViolently >= (float)lord.numPawnsEverGained * this.fraction;
		}

		// Token: 0x04001922 RID: 6434
		private float fraction = 0.5f;
	}
}
