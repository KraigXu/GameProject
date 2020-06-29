using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordToil_PrepareCaravan_Wait : LordToil
	{
		
		// (get) Token: 0x0600326F RID: 12911 RVA: 0x0011854D File Offset: 0x0011674D
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		
		// (get) Token: 0x06003270 RID: 12912 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		
		public LordToil_PrepareCaravan_Wait(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
			}
		}

		
		private IntVec3 meetingPoint;
	}
}
