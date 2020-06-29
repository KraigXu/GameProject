using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public abstract class LordToil_Gathering : LordToil
	{
		
		public LordToil_Gathering(IntVec3 spot, GatheringDef gatheringDef)
		{
			this.spot = spot;
			this.gatheringDef = gatheringDef;
		}

		
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return this.gatheringDef.duty.hook;
		}

		
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(this.gatheringDef.duty, this.spot, -1f);
			}
		}

		
		protected IntVec3 spot;

		
		protected GatheringDef gatheringDef;
	}
}
