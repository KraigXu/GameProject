using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class StorytellerComp_CategoryMTB : StorytellerComp
	{
		
		
		protected StorytellerCompProperties_CategoryMTB Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryMTB)this.props;
			}
		}

		
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			float num = this.Props.mtbDays;
			if (this.Props.mtbDaysFactorByDaysPassedCurve != null)
			{
				num *= this.Props.mtbDaysFactorByDaysPassedCurve.Evaluate(GenDate.DaysPassedFloat);
			}
			if (Rand.MTBEventOccurs(num, 60000f, 1000f))
			{
				IncidentParms parms = this.GenerateParms(this.Props.category, target);
				IncidentDef def;
				if (base.UsableIncidentsInCategory(this.Props.category, parms).TryRandomElementByWeight((IncidentDef incDef) => base.IncidentChanceFinal(incDef), out def))
				{
					yield return new FiringIncident(def, this, parms);
				}
			}
			yield break;
		}

		
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.category;
		}
	}
}
