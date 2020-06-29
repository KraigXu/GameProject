using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordToil_Stage : LordToil
	{
		
		// (get) Token: 0x060032DE RID: 13022 RVA: 0x0011AE95 File Offset: 0x00119095
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.stagingPoint;
			}
		}

		
		// (get) Token: 0x060032DF RID: 13023 RVA: 0x0011AEA2 File Offset: 0x001190A2
		private LordToilData_Stage Data
		{
			get
			{
				return (LordToilData_Stage)this.data;
			}
		}

		
		// (get) Token: 0x060032E0 RID: 13024 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		
		public LordToil_Stage(IntVec3 stagingLoc)
		{
			this.data = new LordToilData_Stage();
			this.Data.stagingPoint = stagingLoc;
		}

		
		public override void UpdateAllDuties()
		{
			LordToilData_Stage data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Defend, data.stagingPoint, -1f);
				this.lord.ownedPawns[i].mindState.duty.radius = 28f;
			}
		}
	}
}
