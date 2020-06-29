using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public class TransportPodsArrivalAction_VisitSite : TransportPodsArrivalAction
	{
		
		public TransportPodsArrivalAction_VisitSite()
		{
		}

		
		public TransportPodsArrivalAction_VisitSite(Site site, PawnsArrivalModeDef arrivalMode)
		{
			this.site = site;
			this.arrivalMode = arrivalMode;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
		}

		
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.site != null && this.site.Tile != destinationTile)
			{
				return false;
			}
			return TransportPodsArrivalAction_VisitSite.CanVisit(pods, this.site);
		}

		
		public override bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return !this.site.HasMap;
		}

		
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			Thing lookTarget = TransportPodsArrivalActionUtility.GetLookTarget(pods);
			bool flag = !this.site.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.site.Tile, CaravanArrivalAction_VisitSite.MapSize, null);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(orGenerateMap.mapPawns.AllPawns, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, true, true);
			}
			Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
			this.arrivalMode.Worker.TravelingTransportPodsArrived(pods, orGenerateMap);
		}

		
		public static FloatMenuAcceptanceReport CanVisit(IEnumerable<IThingHolder> pods, Site site)
		{
			if (site == null || !site.Spawned)
			{
				return false;
			}
			if (!TransportPodsArrivalActionUtility.AnyNonDownedColonist(pods))
			{
				return false;
			}
			if (site.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(site.EnterCooldownDaysLeft().ToString("0.#")));
			}
			return true;
		}

		
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Site site)
		{
			
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter = ((() => TransportPodsArrivalAction_VisitSite.CanVisit(pods, site)));

			Func<TransportPodsArrivalAction_VisitSite> arrivalActionGetter = ((() => new TransportPodsArrivalAction_VisitSite(site, PawnsArrivalModeDefOf.EdgeDrop)));

			foreach (FloatMenuOption floatMenuOption in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSite>(acceptanceReportGetter, arrivalActionGetter, "DropAtEdge".Translate(), representative, site.Tile, null))
			{
				
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter2 = ((() => TransportPodsArrivalAction_VisitSite.CanVisit(pods, site)));

			
			Func<TransportPodsArrivalAction_VisitSite> arrivalActionGetter2 = ((() => new TransportPodsArrivalAction_VisitSite(site, PawnsArrivalModeDefOf.CenterDrop)));

			foreach (FloatMenuOption floatMenuOption2 in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSite>(acceptanceReportGetter2, arrivalActionGetter2, "DropInCenter".Translate(), representative, site.Tile, null))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		
		private Site site;

		
		private PawnsArrivalModeDef arrivalMode;
	}
}
