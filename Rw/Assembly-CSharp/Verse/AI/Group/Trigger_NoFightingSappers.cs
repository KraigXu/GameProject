using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020005FD RID: 1533
	public class Trigger_NoFightingSappers : Trigger
	{
		// Token: 0x06002A1A RID: 10778 RVA: 0x000F61B8 File Offset: 0x000F43B8
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnLost)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn p = lord.ownedPawns[i];
					if (this.IsFightingSapper(p))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x000F61FF File Offset: 0x000F43FF
		private bool IsFightingSapper(Pawn p)
		{
			return !p.Downed && !p.InMentalState && (SappersUtility.IsGoodSapper(p) || SappersUtility.IsGoodBackupSapper(p));
		}
	}
}
