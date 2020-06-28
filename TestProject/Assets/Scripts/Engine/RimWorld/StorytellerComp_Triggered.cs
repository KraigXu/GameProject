using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A23 RID: 2595
	public class StorytellerComp_Triggered : StorytellerComp
	{
		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06003D5F RID: 15711 RVA: 0x00143EDF File Offset: 0x001420DF
		private StorytellerCompProperties_Triggered Props
		{
			get
			{
				return (StorytellerCompProperties_Triggered)this.props;
			}
		}

		// Token: 0x06003D60 RID: 15712 RVA: 0x00143EEC File Offset: 0x001420EC
		public override void Notify_PawnEvent(Pawn p, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
			if (!p.RaceProps.Humanlike || !p.IsColonist)
			{
				return;
			}
			if (ev == AdaptationEvent.Died || ev == AdaptationEvent.Kidnapped || ev == AdaptationEvent.LostBecauseMapClosed || ev == AdaptationEvent.Downed)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction)
				{
					if (pawn.RaceProps.Humanlike && !pawn.Downed)
					{
						return;
					}
				}
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (anyPlayerHomeMap != null)
				{
					IncidentParms parms = StorytellerUtility.DefaultParmsNow(this.Props.incident.category, anyPlayerHomeMap);
					if (this.Props.incident.Worker.CanFireNow(parms, false))
					{
						QueuedIncident qi = new QueuedIncident(new FiringIncident(this.Props.incident, this, parms), Find.TickManager.TicksGame + this.Props.delayTicks, 0);
						Find.Storyteller.incidentQueue.Add(qi);
					}
				}
			}
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x00143FF4 File Offset: 0x001421F4
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incident;
		}
	}
}
