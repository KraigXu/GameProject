using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A1C RID: 2588
	public class StorytellerComp_SingleMTB : StorytellerComp
	{
		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06003D50 RID: 15696 RVA: 0x00143DE3 File Offset: 0x00141FE3
		private StorytellerCompProperties_SingleMTB Props
		{
			get
			{
				return (StorytellerCompProperties_SingleMTB)this.props;
			}
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x00143DF0 File Offset: 0x00141FF0
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (!this.Props.incident.TargetAllowed(target))
			{
				yield break;
			}
			if (Rand.MTBEventOccurs(this.Props.mtbDays, 60000f, 1000f))
			{
				IncidentParms parms = this.GenerateParms(this.Props.incident.category, target);
				if (this.Props.incident.Worker.CanFireNow(parms, false))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x00143E07 File Offset: 0x00142007
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incident;
		}
	}
}
