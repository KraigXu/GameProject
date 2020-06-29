using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class StorytellerComp_SingleMTB : StorytellerComp
	{
		
		// (get) Token: 0x06003D50 RID: 15696 RVA: 0x00143DE3 File Offset: 0x00141FE3
		private StorytellerCompProperties_SingleMTB Props
		{
			get
			{
				return (StorytellerCompProperties_SingleMTB)this.props;
			}
		}

		
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

		
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incident;
		}
	}
}
