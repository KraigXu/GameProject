using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class StorytellerComp_OnOffCycle : StorytellerComp
	{
		
		// (get) Token: 0x06003D34 RID: 15668 RVA: 0x00143982 File Offset: 0x00141B82
		protected StorytellerCompProperties_OnOffCycle Props
		{
			get
			{
				return (StorytellerCompProperties_OnOffCycle)this.props;
			}
		}

		
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			float num = 1f;
			if (this.Props.acceptFractionByDaysPassedCurve != null)
			{
				num *= this.Props.acceptFractionByDaysPassedCurve.Evaluate(GenDate.DaysPassedFloat);
			}
			if (this.Props.acceptPercentFactorPerThreatPointsCurve != null)
			{
				num *= this.Props.acceptPercentFactorPerThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(target));
			}
			if (this.Props.acceptPercentFactorPerProgressScoreCurve != null)
			{
				num *= this.Props.acceptPercentFactorPerProgressScoreCurve.Evaluate(StorytellerUtility.GetProgressScore(target));
			}
			int incCount = IncidentCycleUtility.IncidentCountThisInterval(target, Find.Storyteller.storytellerComps.IndexOf(this), this.Props.minDaysPassed, this.Props.onDays, this.Props.offDays, this.Props.minSpacingDays, this.Props.numIncidentsRange.min, this.Props.numIncidentsRange.max, num);
			int num2;
			for (int i = 0; i < incCount; i = num2 + 1)
			{
				FiringIncident firingIncident = this.GenerateIncident(target);
				if (firingIncident != null)
				{
					yield return firingIncident;
				}
				num2 = i;
			}
			yield break;
		}

		
		private FiringIncident GenerateIncident(IIncidentTarget target)
		{
			IncidentParms parms = this.GenerateParms(this.Props.IncidentCategory, target);
			IncidentDef def;
			if ((float)GenDate.DaysPassed < this.Props.forceRaidEnemyBeforeDaysPassed)
			{
				if (!IncidentDefOf.RaidEnemy.Worker.CanFireNow(parms, false))
				{
					return null;
				}
				def = IncidentDefOf.RaidEnemy;
			}
			else if (this.Props.incident != null)
			{
				if (!this.Props.incident.Worker.CanFireNow(parms, false))
				{
					return null;
				}
				def = this.Props.incident;
			}
			else if (!base.UsableIncidentsInCategory(this.Props.IncidentCategory, parms).TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out def))
			{
				return null;
			}
			return new FiringIncident(def, this, null)
			{
				parms = parms
			};
		}

		
		public override string ToString()
		{
			return base.ToString() + " (" + ((this.Props.incident != null) ? this.Props.incident.defName : this.Props.IncidentCategory.defName) + ")";
		}
	}
}
