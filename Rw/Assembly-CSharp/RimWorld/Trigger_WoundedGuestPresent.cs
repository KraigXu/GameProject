using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007BB RID: 1979
	public class Trigger_WoundedGuestPresent : Trigger
	{
		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x0600333D RID: 13117 RVA: 0x0011BF4D File Offset: 0x0011A14D
		private TriggerData_PawnCycleInd Data
		{
			get
			{
				return (TriggerData_PawnCycleInd)this.data;
			}
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x0011BF5A File Offset: 0x0011A15A
		public Trigger_WoundedGuestPresent()
		{
			this.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x0011C068 File Offset: 0x0011A268
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 800 == 0)
			{
				TriggerData_PawnCycleInd data = this.Data;
				data.pawnCycleInd++;
				if (data.pawnCycleInd >= lord.ownedPawns.Count)
				{
					data.pawnCycleInd = 0;
				}
				if (lord.ownedPawns.Any<Pawn>())
				{
					Pawn pawn = lord.ownedPawns[data.pawnCycleInd];
					if (pawn.Spawned && !pawn.Downed && !pawn.InMentalState && KidnapAIUtility.ReachableWoundedGuest(pawn) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04001B94 RID: 7060
		private const int CheckInterval = 800;
	}
}
