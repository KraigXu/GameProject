using System;

namespace Verse.AI.Group
{
	
	public class LordToilData_ExitMap : LordToilData
	{
		
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.None, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.interruptCurrentJob, "interruptCurrentJob", false, false);
		}

		
		public LocomotionUrgency locomotion;

		
		public bool canDig;

		
		public bool interruptCurrentJob;
	}
}
