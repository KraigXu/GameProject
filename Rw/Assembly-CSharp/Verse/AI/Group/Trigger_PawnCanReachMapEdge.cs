using System;

namespace Verse.AI.Group
{
	// Token: 0x02000602 RID: 1538
	public class Trigger_PawnCanReachMapEdge : Trigger
	{
		// Token: 0x06002A26 RID: 10790 RVA: 0x000F64D0 File Offset: 0x000F46D0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 193 == 0)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn = lord.ownedPawns[i];
					if (pawn.Spawned && !pawn.Dead && !pawn.Downed && !pawn.CanReachMapEdge())
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
