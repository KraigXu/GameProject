using System;

namespace RimWorld
{
	
	public class GameCondition_HeatWave : GameCondition
	{
		
		// (get) Token: 0x06003BB0 RID: 15280 RVA: 0x0013B220 File Offset: 0x00139420
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
