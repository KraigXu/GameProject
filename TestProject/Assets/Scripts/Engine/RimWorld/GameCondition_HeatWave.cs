using System;

namespace RimWorld
{
	
	public class GameCondition_HeatWave : GameCondition
	{
		
		
		public override int TransitionTicks
		{
			get
			{
				return 12000;
			}
		}

		
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 17f);
		}

		
		private const float MaxTempOffset = 17f;
	}
}
