using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class StorytellerComp_ThreatsGenerator : StorytellerComp
	{
		
		
		protected StorytellerCompProperties_ThreatsGenerator Props
		{
			get
			{
				return (StorytellerCompProperties_ThreatsGenerator)this.props;
			}
		}

		
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			Map map = target as Map;
			foreach (FiringIncident firingIncident in ThreatsGenerator.MakeIntervalIncidents(this.Props.parms, target, (map != null) ? map.generationTick : 0))
			{
				firingIncident.source = this;
				yield return firingIncident;
			}
			IEnumerator<FiringIncident> enumerator = null;
			yield break;
			yield break;
		}
	}
}
