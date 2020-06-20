using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200126F RID: 4719
	public class TransportPodsArrivalAction_VisitSite : TransportPodsArrivalAction
	{
		// Token: 0x06006E69 RID: 28265 RVA: 0x0026813D File Offset: 0x0026633D
		public TransportPodsArrivalAction_VisitSite()
		{
		}

		// Token: 0x06006E6A RID: 28266 RVA: 0x00268C16 File Offset: 0x00266E16
		public TransportPodsArrivalAction_VisitSite(Site site, PawnsArrivalModeDef arrivalMode)
		{
			this.site = site;
			this.arrivalMode = arrivalMode;
		}

		// Token: 0x06006E6B RID: 28267 RVA: 0x00268C2C File Offset: 0x00266E2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
		}

		// Token: 0x06006E6C RID: 28268 RVA: 0x00268C58 File Offset: 0x00266E58
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

		// Token: 0x06006E6D RID: 28269 RVA: 0x00268CA1 File Offset: 0x00266EA1
		public override bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return !this.site.HasMap;
		}

		// Token: 0x06006E6E RID: 28270 RVA: 0x00268CB4 File Offset: 0x00266EB4
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

		// Token: 0x06006E6F RID: 28271 RVA: 0x00268D68 File Offset: 0x00266F68
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

		// Token: 0x06006E70 RID: 28272 RVA: 0x00268DD1 File Offset: 0x00266FD1
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Site site)
		{
			Func<FloatMenuAcceptanceReport> <>9__0;
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter;
			if ((acceptanceReportGetter = <>9__0) == null)
			{
				acceptanceReportGetter = (<>9__0 = (() => TransportPodsArrivalAction_VisitSite.CanVisit(pods, site)));
			}
			Func<TransportPodsArrivalAction_VisitSite> <>9__1;
			Func<TransportPodsArrivalAction_VisitSite> arrivalActionGetter;
			if ((arrivalActionGetter = <>9__1) == null)
			{
				arrivalActionGetter = (<>9__1 = (() => new TransportPodsArrivalAction_VisitSite(site, PawnsArrivalModeDefOf.EdgeDrop)));
			}
			foreach (FloatMenuOption floatMenuOption in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSite>(acceptanceReportGetter, arrivalActionGetter, "DropAtEdge".Translate(), representative, site.Tile, null))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			Func<FloatMenuAcceptanceReport> <>9__2;
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter2;
			if ((acceptanceReportGetter2 = <>9__2) == null)
			{
				acceptanceReportGetter2 = (<>9__2 = (() => TransportPodsArrivalAction_VisitSite.CanVisit(pods, site)));
			}
			Func<TransportPodsArrivalAction_VisitSite> <>9__3;
			Func<TransportPodsArrivalAction_VisitSite> arrivalActionGetter2;
			if ((arrivalActionGetter2 = <>9__3) == null)
			{
				arrivalActionGetter2 = (<>9__3 = (() => new TransportPodsArrivalAction_VisitSite(site, PawnsArrivalModeDefOf.CenterDrop)));
			}
			foreach (FloatMenuOption floatMenuOption2 in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSite>(acceptanceReportGetter2, arrivalActionGetter2, "DropInCenter".Translate(), representative, site.Tile, null))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04004428 RID: 17448
		private Site site;

		// Token: 0x04004429 RID: 17449
		private PawnsArrivalModeDef arrivalMode;
	}
}
