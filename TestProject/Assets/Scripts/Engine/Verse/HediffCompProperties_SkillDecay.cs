using System;

namespace Verse
{
	
	public class HediffCompProperties_SkillDecay : HediffCompProperties
	{
		
		public HediffCompProperties_SkillDecay()
		{
			this.compClass = typeof(HediffComp_SkillDecay);
		}

		
		public SimpleCurve decayPerDayPercentageLevelCurve;
	}
}
