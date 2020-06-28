using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A11 RID: 2577
	public class StorytellerCompProperties_OnOffCycle : StorytellerCompProperties
	{
		// Token: 0x17000AD7 RID: 2775
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

		// Token: 0x06003D32 RID: 15666 RVA: 0x0014394F File Offset: 0x00141B4F
		public StorytellerCompProperties_OnOffCycle()
		{
			this.compClass = typeof(StorytellerComp_OnOffCycle);
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x00143972 File Offset: 0x00141B72
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

		// Token: 0x040023B2 RID: 9138
		public float onDays;

		// Token: 0x040023B3 RID: 9139
		public float offDays;

		// Token: 0x040023B4 RID: 9140
		public float minSpacingDays;

		// Token: 0x040023B5 RID: 9141
		public FloatRange numIncidentsRange = FloatRange.Zero;

		// Token: 0x040023B6 RID: 9142
		public SimpleCurve acceptFractionByDaysPassedCurve;

		// Token: 0x040023B7 RID: 9143
		public SimpleCurve acceptPercentFactorPerThreatPointsCurve;

		// Token: 0x040023B8 RID: 9144
		public SimpleCurve acceptPercentFactorPerProgressScoreCurve;

		// Token: 0x040023B9 RID: 9145
		public IncidentDef incident;

		// Token: 0x040023BA RID: 9146
		private IncidentCategoryDef category;

		// Token: 0x040023BB RID: 9147
		public float forceRaidEnemyBeforeDaysPassed;
	}
}
