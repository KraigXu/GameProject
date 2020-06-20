using System;

namespace RimWorld
{
	// Token: 0x02000CC8 RID: 3272
	public class ThingSetMaker_Conditional_MinMaxTotalMarketValue : ThingSetMaker_Conditional
	{
		// Token: 0x06004F53 RID: 20307 RVA: 0x001AB701 File Offset: 0x001A9901
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return parms.totalMarketValueRange != null && parms.totalMarketValueRange.Value.max >= this.minMaxTotalMarketValue;
		}

		// Token: 0x04002C86 RID: 11398
		public float minMaxTotalMarketValue;
	}
}
