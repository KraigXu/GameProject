using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A1E RID: 2590
	public class StorytellerComp_SingleOnceFixed : StorytellerComp
	{
		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06003D55 RID: 15701 RVA: 0x0013B2B7 File Offset: 0x001394B7
		protected int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06003D56 RID: 15702 RVA: 0x00143E47 File Offset: 0x00142047
		private StorytellerCompProperties_SingleOnceFixed Props
		{
			get
			{
				return (StorytellerCompProperties_SingleOnceFixed)this.props;
			}
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x00143E54 File Offset: 0x00142054
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			int num = this.IntervalsPassed;
			if (this.Props.minColonistCount > 0)
			{
				if (target.StoryState.lastFireTicks.ContainsKey(this.Props.incident))
				{
					yield break;
				}
				if (PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count < this.Props.minColonistCount)
				{
					yield break;
				}
				num -= target.StoryState.GetTicksFromColonistCount(this.Props.minColonistCount) / 1000;
			}
			if (num == this.Props.fireAfterDaysPassed * 60)
			{
				if (this.Props.skipIfColonistHasMinTitle != null)
				{
					List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists;
					for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count; i++)
					{
						if (allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists[i].royalty != null && allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists[i].royalty.AllTitlesForReading.Any<RoyalTitle>() && allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists[i].royalty.MainTitle().seniority >= this.Props.skipIfColonistHasMinTitle.seniority)
						{
							yield break;
						}
					}
				}
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (!this.Props.skipIfOnExtremeBiome || (anyPlayerHomeMap != null && !anyPlayerHomeMap.Biome.isExtremeBiome))
				{
					IncidentDef incident = this.Props.incident;
					if (incident.TargetAllowed(target))
					{
						FiringIncident firingIncident = new FiringIncident(incident, this, this.GenerateParms(incident.category, target));
						yield return firingIncident;
					}
				}
			}
			yield break;
		}
	}
}
