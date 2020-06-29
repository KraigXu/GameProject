using System;

namespace Verse.AI.Group
{
	
	public class Trigger_Custom : Trigger
	{
		
		public Trigger_Custom(Func<TriggerSignal, bool> condition)
		{
			this.condition = condition;
		}

		
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return this.condition(signal);
		}

		
		private Func<TriggerSignal, bool> condition;
	}
}
