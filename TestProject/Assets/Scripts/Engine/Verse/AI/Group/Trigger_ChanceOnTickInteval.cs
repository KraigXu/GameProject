using System;

namespace Verse.AI.Group
{
	
	public class Trigger_ChanceOnTickInteval : Trigger
	{
		
		public Trigger_ChanceOnTickInteval(int interval, float chancePerInterval)
		{
			this.chancePerInterval = chancePerInterval;
			this.interval = interval;
		}

		
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.interval == 0 && Rand.Value < this.chancePerInterval;
		}

		
		private float chancePerInterval;

		
		private int interval;
	}
}
