using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007A2 RID: 1954
	public class LordToil_Stage : LordToil
	{
		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060032DE RID: 13022 RVA: 0x0011AE95 File Offset: 0x00119095
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.stagingPoint;
			}
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060032DF RID: 13023 RVA: 0x0011AEA2 File Offset: 0x001190A2
		private LordToilData_Stage Data
		{
			get
			{
				return (LordToilData_Stage)this.data;
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060032E0 RID: 13024 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x0011AEAF File Offset: 0x001190AF
		public LordToil_Stage(IntVec3 stagingLoc)
		{
			this.data = new LordToilData_Stage();
			this.Data.stagingPoint = stagingLoc;
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x0011AED0 File Offset: 0x001190D0
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
