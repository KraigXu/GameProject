using System;

namespace Verse.AI.Group
{
	
	public class TriggerData_TicksPassedAfterConditionMet : TriggerData_TicksPassed
	{
		
		public override void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.conditionMet, "conditionMet", false, false);
		}

		
		public bool conditionMet;
	}
}
