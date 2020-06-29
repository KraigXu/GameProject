using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordToil_DefendBase : LordToil
	{
		
		// (get) Token: 0x06003288 RID: 12936 RVA: 0x00119209 File Offset: 0x00117409
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.baseCenter;
			}
		}

		
		public LordToil_DefendBase(IntVec3 baseCenter)
		{
			this.baseCenter = baseCenter;
		}

		
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.DefendBase, this.baseCenter, -1f);
			}
		}

		
		public IntVec3 baseCenter;
	}
}
