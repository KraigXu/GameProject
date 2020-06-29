using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class HediffCompProperties_Disorientation : HediffCompProperties
	{
		
		public HediffCompProperties_Disorientation()
		{
			this.compClass = typeof(HediffComp_Disorientation);
		}

		
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in this.ConfigErrors(parentDef))
			{
				
			}
			IEnumerator<string> enumerator = null;
			if (this.wanderMtbHours <= 0f)
			{
				yield return "wanderMtbHours must be greater than zero";
			}
			if (this.singleWanderDurationTicks <= 0)
			{
				yield return "singleWanderDurationTicks must be greater than zero";
			}
			if (this.wanderRadius <= 0f)
			{
				yield return "wanderRadius must be greater than zero";
			}
			yield break;
			yield break;
		}

		
		public float wanderMtbHours = -1f;

		
		public float wanderRadius;

		
		public int singleWanderDurationTicks = -1;
	}
}
