using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000791 RID: 1937
	public class LordToil_DefendBase : LordToil
	{
		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06003288 RID: 12936 RVA: 0x00119209 File Offset: 0x00117409
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.baseCenter;
			}
		}

		// Token: 0x06003289 RID: 12937 RVA: 0x00119211 File Offset: 0x00117411
		public LordToil_DefendBase(IntVec3 baseCenter)
		{
			this.baseCenter = baseCenter;
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x00119220 File Offset: 0x00117420
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.DefendBase, this.baseCenter, -1f);
			}
		}

		// Token: 0x04001B5B RID: 7003
		public IntVec3 baseCenter;
	}
}
