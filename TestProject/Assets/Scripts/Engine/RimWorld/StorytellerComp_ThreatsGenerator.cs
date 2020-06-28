using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A21 RID: 2593
	public class StorytellerComp_ThreatsGenerator : StorytellerComp
	{
		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06003D5B RID: 15707 RVA: 0x00143E9B File Offset: 0x0014209B
		protected StorytellerCompProperties_ThreatsGenerator Props
		{
			get
			{
				return (StorytellerCompProperties_ThreatsGenerator)this.props;
			}
		}

		// Token: 0x06003D5C RID: 15708 RVA: 0x00143EA8 File Offset: 0x001420A8
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
