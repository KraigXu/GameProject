using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001223 RID: 4643
	public class CaravanArrivalAction_VisitSite : CaravanArrivalAction
	{
		// Token: 0x17001207 RID: 4615
		// (get) Token: 0x06006BB9 RID: 27577 RVA: 0x00259359 File Offset: 0x00257559
		public override string Label
		{
			get
			{
				return this.site.ApproachOrderString;
			}
		}

		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x06006BBA RID: 27578 RVA: 0x00259366 File Offset: 0x00257566
		public override string ReportString
		{
			get
			{
				return this.site.ApproachingReportString;
			}
		}

		// Token: 0x06006BBB RID: 27579 RVA: 0x00258549 File Offset: 0x00256749
		public CaravanArrivalAction_VisitSite()
		{
		}

		// Token: 0x06006BBC RID: 27580 RVA: 0x00259373 File Offset: 0x00257573
		public CaravanArrivalAction_VisitSite(Site site)
		{
			this.site = site;
		}

		// Token: 0x06006BBD RID: 27581 RVA: 0x00259384 File Offset: 0x00257584
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.site != null && this.site.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_VisitSite.CanVisit(caravan, this.site);
		}

		// Token: 0x06006BBE RID: 27582 RVA: 0x002593D0 File Offset: 0x002575D0
		public override void Arrived(Caravan caravan)
		{
			if (!this.site.HasMap)
			{
				LongEventHandler.QueueLongEvent(delegate
				{
					this.DoEnter(caravan, this.site);
				}, "GeneratingMapForNewEncounter", false, null, true);
				return;
			}
			this.DoEnter(caravan, this.site);
		}

		// Token: 0x06006BBF RID: 27583 RVA: 0x0025942C File Offset: 0x0025762C
		private void DoEnter(Caravan caravan, Site site)
		{
			LookTargets lookTargets = new LookTargets(caravan.PawnsListForReading);
			bool draftColonists = site.Faction == null || site.Faction.HostileTo(Faction.OfPlayer);
			bool flag = !site.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(site.Tile, CaravanArrivalAction_VisitSite.MapSize, null);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(orGenerateMap.mapPawns.AllPawns, "LetterRelatedPawnsSite".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, true, true);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("LetterCaravanEnteredMap".Translate(caravan.Label, site).CapitalizeFirst());
				LetterDef letterDef;
				LookTargets lookTargets2;
				this.AppendThreatInfo(stringBuilder, site, orGenerateMap, out letterDef, out lookTargets2);
				List<HediffDef> list = null;
				foreach (SitePart sitePart in site.parts)
				{
					if (!sitePart.def.arrivedLetterHediffHyperlinks.NullOrEmpty<HediffDef>())
					{
						if (list == null)
						{
							list = new List<HediffDef>();
						}
						foreach (HediffDef item in sitePart.def.arrivedLetterHediffHyperlinks)
						{
							if (!list.Contains(item))
							{
								list.Add(item);
							}
						}
					}
				}
				ChoiceLetter choiceLetter = LetterMaker.MakeLetter("LetterLabelCaravanEnteredMap".Translate(site), stringBuilder.ToString(), letterDef ?? LetterDefOf.NeutralEvent, lookTargets2.IsValid() ? lookTargets2 : lookTargets, null, null, null);
				choiceLetter.hyperlinkHediffDefs = list;
				Find.LetterStack.ReceiveLetter(choiceLetter, null);
			}
			else
			{
				Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(site), "LetterCaravanEnteredMap".Translate(caravan.Label, site).CapitalizeFirst(), LetterDefOf.NeutralEvent, lookTargets, null, null, null, null);
			}
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.DoNotDrop, draftColonists, null);
		}

		// Token: 0x06006BC0 RID: 27584 RVA: 0x0025966C File Offset: 0x0025786C
		private void AppendThreatInfo(StringBuilder sb, Site site, Map map, out LetterDef letterDef, out LookTargets allLookTargets)
		{
			allLookTargets = new LookTargets();
			CaravanArrivalAction_VisitSite.tmpUsedDefs.Clear();
			CaravanArrivalAction_VisitSite.tmpDefs.Clear();
			for (int i = 0; i < site.parts.Count; i++)
			{
				CaravanArrivalAction_VisitSite.tmpDefs.Add(site.parts[i].def);
			}
			letterDef = null;
			for (int j = 0; j < CaravanArrivalAction_VisitSite.tmpDefs.Count; j++)
			{
				LetterDef letterDef2;
				LookTargets lookTargets;
				string arrivedLetterPart = CaravanArrivalAction_VisitSite.tmpDefs[j].Worker.GetArrivedLetterPart(map, out letterDef2, out lookTargets);
				if (arrivedLetterPart != null)
				{
					if (!CaravanArrivalAction_VisitSite.tmpUsedDefs.Contains(CaravanArrivalAction_VisitSite.tmpDefs[j]))
					{
						CaravanArrivalAction_VisitSite.tmpUsedDefs.Add(CaravanArrivalAction_VisitSite.tmpDefs[j]);
						if (sb.Length > 0)
						{
							sb.AppendLine();
							sb.AppendLine();
						}
						sb.Append(arrivedLetterPart);
					}
					if (letterDef == null)
					{
						letterDef = letterDef2;
					}
					if (lookTargets.IsValid())
					{
						allLookTargets = new LookTargets(allLookTargets.targets.Concat(lookTargets.targets));
					}
				}
			}
		}

		// Token: 0x06006BC1 RID: 27585 RVA: 0x0025977D File Offset: 0x0025797D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
		}

		// Token: 0x06006BC2 RID: 27586 RVA: 0x00259798 File Offset: 0x00257998
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, Site site)
		{
			if (site == null || !site.Spawned)
			{
				return false;
			}
			if (site.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(site.EnterCooldownDaysLeft().ToString("0.#")));
			}
			return true;
		}

		// Token: 0x06006BC3 RID: 27587 RVA: 0x002597F4 File Offset: 0x002579F4
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Site site)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSite>(() => CaravanArrivalAction_VisitSite.CanVisit(caravan, site), () => new CaravanArrivalAction_VisitSite(site), site.ApproachOrderString, caravan, site.Tile, site, null);
		}

		// Token: 0x04004339 RID: 17209
		private Site site;

		// Token: 0x0400433A RID: 17210
		public static readonly IntVec3 MapSize = new IntVec3(120, 1, 120);

		// Token: 0x0400433B RID: 17211
		private static List<SitePartDef> tmpDefs = new List<SitePartDef>();

		// Token: 0x0400433C RID: 17212
		private static List<SitePartDef> tmpUsedDefs = new List<SitePartDef>();
	}
}
