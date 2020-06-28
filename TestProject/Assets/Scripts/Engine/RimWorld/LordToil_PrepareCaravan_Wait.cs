using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200078B RID: 1931
	public class LordToil_PrepareCaravan_Wait : LordToil
	{
		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x0600326F RID: 12911 RVA: 0x0011854D File Offset: 0x0011674D
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06003270 RID: 12912 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x00118C75 File Offset: 0x00116E75
		public LordToil_PrepareCaravan_Wait(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x00118C84 File Offset: 0x00116E84
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
			}
		}

		// Token: 0x04001B55 RID: 6997
		private IntVec3 meetingPoint;
	}
}
