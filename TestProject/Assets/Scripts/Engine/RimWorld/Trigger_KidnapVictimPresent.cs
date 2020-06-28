using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007B9 RID: 1977
	public class Trigger_KidnapVictimPresent : Trigger
	{
		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06003338 RID: 13112 RVA: 0x0011BF4D File Offset: 0x0011A14D
		private TriggerData_PawnCycleInd Data
		{
			get
			{
				return (TriggerData_PawnCycleInd)this.data;
			}
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x0011BF5A File Offset: 0x0011A15A
		public Trigger_KidnapVictimPresent()
		{
			this.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x0011BF70 File Offset: 0x0011A170
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 120 == 0)
			{
				if (this.data == null || !(this.data is TriggerData_PawnCycleInd))
				{
					BackCompatibility.TriggerDataPawnCycleIndNull(this);
				}
				if (Find.TickManager.TicksGame - lord.lastPawnHarmTick > 300)
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
						Pawn pawn2;
						if (pawn.Spawned && !pawn.Downed && pawn.MentalStateDef == null && KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 8f, out pawn2, null) && !GenAI.InDangerousCombat(pawn))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04001B91 RID: 7057
		private const int CheckInterval = 120;

		// Token: 0x04001B92 RID: 7058
		private const int MinTicksSinceDamage = 300;
	}
}
