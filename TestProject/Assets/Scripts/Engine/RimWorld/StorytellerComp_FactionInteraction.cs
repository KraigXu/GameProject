using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class StorytellerComp_FactionInteraction : StorytellerComp
	{
		
		
		private StorytellerCompProperties_FactionInteraction Props
		{
			get
			{
				return (StorytellerCompProperties_FactionInteraction)this.props;
			}
		}

		
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (this.Props.minDanger != StoryDanger.None)
			{
				Map map = target as Map;
				if (map == null || map.dangerWatcher.DangerRating < this.Props.minDanger)
				{
					yield break;
				}
			}
			float num = StorytellerUtility.AllyIncidentFraction(this.Props.fullAlliesOnly);
			if (num <= 0f)
			{
				yield break;
			}
			int incCount = IncidentCycleUtility.IncidentCountThisInterval(target, Find.Storyteller.storytellerComps.IndexOf(this), this.Props.minDaysPassed, 60f, 0f, this.Props.minSpacingDays, this.Props.baseIncidentsPerYear, this.Props.baseIncidentsPerYear, num);
			int num2;
			for (int i = 0; i < incCount; i = num2 + 1)
			{
				IncidentParms parms = this.GenerateParms(this.Props.incident.category, target);
				if (this.Props.incident.Worker.CanFireNow(parms, false))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
				num2 = i;
			}
			yield break;
		}

		
		public override string ToString()
		{
			return base.ToString() + " (" + this.Props.incident.defName + ")";
		}
	}
}
