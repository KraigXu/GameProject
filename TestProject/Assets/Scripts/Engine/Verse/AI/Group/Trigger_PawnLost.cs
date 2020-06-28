using System;

namespace Verse.AI.Group
{
	// Token: 0x02000603 RID: 1539
	public class Trigger_PawnLost : Trigger
	{
		// Token: 0x06002A28 RID: 10792 RVA: 0x000F6540 File Offset: 0x000F4740
		public Trigger_PawnLost(PawnLostCondition condition = PawnLostCondition.Undefined, Pawn pawn = null)
		{
			this.condition = condition;
			this.pawn = pawn;
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x000F6556 File Offset: 0x000F4756
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (this.condition == PawnLostCondition.Undefined || signal.condition == this.condition) && (this.pawn == null || this.pawn == signal.Pawn);
		}

		// Token: 0x0400192E RID: 6446
		private Pawn pawn;

		// Token: 0x0400192F RID: 6447
		private PawnLostCondition condition;
	}
}
