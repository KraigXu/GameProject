using System;

namespace Verse.AI.Group
{
	// Token: 0x02000605 RID: 1541
	public class Trigger_PawnKilled : Trigger
	{
		// Token: 0x06002A2C RID: 10796 RVA: 0x000F65DC File Offset: 0x000F47DC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && signal.condition == PawnLostCondition.IncappedOrKilled && signal.Pawn.Dead;
		}
	}
}
