using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000798 RID: 1944
	public class LordToil_HuntEnemies : LordToil
	{
		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x060032AA RID: 12970 RVA: 0x00119C84 File Offset: 0x00117E84
		private LordToilData_HuntEnemies Data
		{
			get
			{
				return (LordToilData_HuntEnemies)this.data;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x060032AB RID: 12971 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x00119C91 File Offset: 0x00117E91
		public LordToil_HuntEnemies(IntVec3 fallbackLocation)
		{
			this.data = new LordToilData_HuntEnemies();
			this.Data.fallbackLocation = fallbackLocation;
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x00119CB0 File Offset: 0x00117EB0
		public override void UpdateAllDuties()
		{
			LordToilData_HuntEnemies data = this.Data;
			if (!data.fallbackLocation.IsValid)
			{
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (pawn.Spawned && RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out data.fallbackLocation) && data.fallbackLocation.IsValid)
					{
						break;
					}
				}
			}
			for (int j = 0; j < this.lord.ownedPawns.Count; j++)
			{
				Pawn pawn2 = this.lord.ownedPawns[j];
				pawn2.mindState.duty = new PawnDuty(DutyDefOf.HuntEnemiesIndividual);
				pawn2.mindState.duty.focusSecond = data.fallbackLocation;
			}
		}
	}
}
