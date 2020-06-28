using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A07 RID: 2567
	public class StorytellerComp_CategoryMTB : StorytellerComp
	{
		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06003D17 RID: 15639 RVA: 0x00143749 File Offset: 0x00141949
		protected StorytellerCompProperties_CategoryMTB Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryMTB)this.props;
			}
		}

		// Token: 0x06003D18 RID: 15640 RVA: 0x00143756 File Offset: 0x00141956
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

		// Token: 0x06003D19 RID: 15641 RVA: 0x0014376D File Offset: 0x0014196D
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.category;
		}
	}
}
