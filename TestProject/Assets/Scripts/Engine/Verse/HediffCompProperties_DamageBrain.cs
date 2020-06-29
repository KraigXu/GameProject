using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class HediffCompProperties_DamageBrain : HediffCompProperties
	{
		
		public HediffCompProperties_DamageBrain()
		{
			this.compClass = typeof(HediffComp_DamageBrain);
		}

		
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in this.ConfigErrors(parentDef))
			{
				
			}
			IEnumerator<string> enumerator = null;
			if (this.damageAmount == IntRange.zero)
			{
				yield return "damageAmount is not defined";
			}
			if (this.mtbDaysPerStage == null)
			{
				yield return "mtbDaysPerStage is not defined";
			}
			else if (this.mtbDaysPerStage.Count != parentDef.stages.Count)
			{
				yield return "mtbDaysPerStage count doesn't match Hediffs number of stages";
			}
			yield break;
			yield break;
		}

		
		public IntRange damageAmount = IntRange.zero;

		
		public List<float> mtbDaysPerStage;
	}
}
