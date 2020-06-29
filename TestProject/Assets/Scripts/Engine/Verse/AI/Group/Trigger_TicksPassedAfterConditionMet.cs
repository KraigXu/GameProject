using System;

namespace Verse.AI.Group
{
	
	public class Trigger_TicksPassedAfterConditionMet : Trigger_TicksPassed
	{
		
		
		protected new TriggerData_TicksPassedAfterConditionMet Data
		{
			get
			{
				return (TriggerData_TicksPassedAfterConditionMet)this.data;
			}
		}

		
		public Trigger_TicksPassedAfterConditionMet(int tickLimit, Func<bool> condition, int checkEveryTicks = 1) : base(tickLimit)
		{
			this.condition = condition;
			this.checkEveryTicks = checkEveryTicks;
			this.data = new TriggerData_TicksPassedAfterConditionMet();
		}

		
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (!this.Data.conditionMet && signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.checkEveryTicks == 0)
			{
				this.Data.conditionMet = this.condition();
			}
			return this.Data.conditionMet && base.ActivateOn(lord, signal);
		}

		
		private Func<bool> condition;

		
		private int checkEveryTicks;
	}
}
