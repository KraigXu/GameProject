using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A10 RID: 2576
	public class StorytellerComp_FactionInteraction : StorytellerComp
	{
		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06003D2D RID: 15661 RVA: 0x001438E8 File Offset: 0x00141AE8
		private StorytellerCompProperties_FactionInteraction Props
		{
			get
			{
				return (StorytellerCompProperties_FactionInteraction)this.props;
			}
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x001438F5 File Offset: 0x00141AF5
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

		// Token: 0x06003D2F RID: 15663 RVA: 0x0014390C File Offset: 0x00141B0C
		public override string ToString()
		{
			return base.ToString() + " (" + this.Props.incident.defName + ")";
		}
	}
}
