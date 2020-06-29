using System;

namespace Verse.AI.Group
{
	
	public class Trigger_TicksPassedAndNoRecentHarm : Trigger_TicksPassed
	{
		
		public Trigger_TicksPassedAndNoRecentHarm(int tickLimit) : base(tickLimit)
		{
		}

		
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return base.ActivateOn(lord, signal) && Find.TickManager.TicksGame - lord.lastPawnHarmTick >= 300;
		}

		
		private const int MinTicksSinceDamage = 300;
	}
}
