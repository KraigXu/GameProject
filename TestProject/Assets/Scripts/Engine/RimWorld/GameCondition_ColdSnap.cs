using System;

namespace RimWorld
{
	
	public class GameCondition_ColdSnap : GameCondition
	{
		
		// (get) Token: 0x06003BB3 RID: 15283 RVA: 0x0013B220 File Offset: 0x00139420
		public override int TransitionTicks
		{
			get
			{
				return 12000;
			}
		}

		
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, -20f);
		}

		
		private const float MaxTempOffset = -20f;
	}
}
