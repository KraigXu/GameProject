using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Scanner : CompProperties
	{
		
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.scanFindMtbDays <= 0f)
			{
				yield return "scanFindMtbDays not set";
			}
			yield break;
		}

		
		public float scanFindMtbDays;

		
		public float scanFindGuaranteedDays = -1f;

		
		public StatDef scanSpeedStat;
	}
}
