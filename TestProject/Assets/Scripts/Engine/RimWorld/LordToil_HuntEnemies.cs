using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordToil_HuntEnemies : LordToil
	{
		
		// (get) Token: 0x060032AA RID: 12970 RVA: 0x00119C84 File Offset: 0x00117E84
		private LordToilData_HuntEnemies Data
		{
			get
			{
				return (LordToilData_HuntEnemies)this.data;
			}
		}

		
		// (get) Token: 0x060032AB RID: 12971 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		
		public LordToil_HuntEnemies(IntVec3 fallbackLocation)
		{
			this.data = new LordToilData_HuntEnemies();
			this.Data.fallbackLocation = fallbackLocation;
		}

		
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
