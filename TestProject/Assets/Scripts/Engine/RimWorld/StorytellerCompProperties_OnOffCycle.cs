using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class StorytellerCompProperties_OnOffCycle : StorytellerCompProperties
	{
		
		// (get) Token: 0x06003D31 RID: 15665 RVA: 0x00143933 File Offset: 0x00141B33
		public IncidentCategoryDef IncidentCategory
		{
			get
			{
				if (this.incident != null)
				{
					return this.incident.category;
				}
				return this.category;
			}
		}

		
		public StorytellerCompProperties_OnOffCycle()
		{
			this.compClass = typeof(StorytellerComp_OnOffCycle);
		}

		
		public override IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
		{
			if (this.incident != null && this.category != null)
			{
				yield return "incident and category should not both be defined";
			}
			if (this.onDays <= 0f)
			{
				yield return "onDays must be above zero";
			}
			if (this.numIncidentsRange.TrueMax <= 0f)
			{
				yield return "numIncidentRange not configured";
			}
			if (this.minSpacingDays * this.numIncidentsRange.TrueMax > this.onDays * 0.9f)
			{
				yield return "minSpacingDays too high compared to max number of incidents.";
			}
			yield break;
		}

		
		public float onDays;

		
		public float offDays;

		
		public float minSpacingDays;

		
		public FloatRange numIncidentsRange = FloatRange.Zero;

		
		public SimpleCurve acceptFractionByDaysPassedCurve;

		
		public SimpleCurve acceptPercentFactorPerThreatPointsCurve;

		
		public SimpleCurve acceptPercentFactorPerProgressScoreCurve;

		
		public IncidentDef incident;

		
		private IncidentCategoryDef category;

		
		public float forceRaidEnemyBeforeDaysPassed;
	}
}
