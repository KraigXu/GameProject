using System;

namespace RimWorld
{
	
	public class ThingSetMaker_Conditional_MinMaxTotalMarketValue : ThingSetMaker_Conditional
	{
		
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return parms.totalMarketValueRange != null && parms.totalMarketValueRange.Value.max >= this.minMaxTotalMarketValue;
		}

		
		public float minMaxTotalMarketValue;
	}
}
