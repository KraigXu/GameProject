using System;

namespace Verse.AI.Group
{
	// Token: 0x02000601 RID: 1537
	public class Trigger_PawnCannotReachMapEdge : Trigger
	{
		// Token: 0x06002A24 RID: 10788 RVA: 0x000F6460 File Offset: 0x000F4660
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 197 == 0)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn = lord.ownedPawns[i];
					if (pawn.Spawned && !pawn.Dead && !pawn.Downed && !pawn.CanReachMapEdge())
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
