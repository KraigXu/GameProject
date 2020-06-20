using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001220 RID: 4640
	public class CaravanArrivalAction_VisitEscapeShip : CaravanArrivalAction
	{
		// Token: 0x17001201 RID: 4609
		// (get) Token: 0x06006B9D RID: 27549 RVA: 0x00258D49 File Offset: 0x00256F49
		public override string Label
		{
			get
			{
				return "VisitEscapeShip".Translate(this.target.Label);
			}
		}

		// Token: 0x17001202 RID: 4610
		// (get) Token: 0x06006B9E RID: 27550 RVA: 0x00258D6A File Offset: 0x00256F6A
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(this.target.Label);
			}
		}

		// Token: 0x06006B9F RID: 27551 RVA: 0x00258549 File Offset: 0x00256749
		public CaravanArrivalAction_VisitEscapeShip()
		{
		}

		// Token: 0x06006BA0 RID: 27552 RVA: 0x00258D8B File Offset: 0x00256F8B
		public CaravanArrivalAction_VisitEscapeShip(EscapeShipComp escapeShip)
		{
			this.target = (MapParent)escapeShip.parent;
		}

		// Token: 0x06006BA1 RID: 27553 RVA: 0x00258DA4 File Offset: 0x00256FA4
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.target != null && this.target.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_VisitEscapeShip.CanVisit(caravan, this.target);
		}

		// Token: 0x06006BA2 RID: 27554 RVA: 0x00258DF0 File Offset: 0x00256FF0
		public override void Arrived(Caravan caravan)
		{
			if (!this.target.HasMap)
			{
				LongEventHandler.QueueLongEvent(delegate
				{
					this.DoArrivalAction(caravan);
				}, "GeneratingMapForNewEncounter", false, null, true);
				return;
			}
			this.DoArrivalAction(caravan);
		}

		// Token: 0x06006BA3 RID: 27555 RVA: 0x00258E44 File Offset: 0x00257044
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.target, "target", false);
		}

		// Token: 0x06006BA4 RID: 27556 RVA: 0x00258E60 File Offset: 0x00257060
		private void DoArrivalAction(Caravan caravan)
		{
			bool flag = !this.target.HasMap;
			if (flag)
			{
				this.target.SetFaction(Faction.OfPlayer);
			}
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.target.Tile, null);
			LookTargets lookTargets = new LookTargets(caravan.PawnsListForReading);
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.UnloadIndividually, false, null);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				Find.LetterStack.ReceiveLetter("EscapeShipFoundLabel".Translate(), (Find.Storyteller.difficulty.difficulty == 0) ? "EscapeShipFoundPeaceful".Translate() : "EscapeShipFound".Translate(), LetterDefOf.PositiveEvent, new GlobalTargetInfo(this.target.Map.Center, this.target.Map, false), null, null, null, null);
				return;
			}
			Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(this.target), "LetterCaravanEnteredMap".Translate(caravan.Label, this.target).CapitalizeFirst(), LetterDefOf.NeutralEvent, lookTargets, null, null, null, null);
		}

		// Token: 0x06006BA5 RID: 27557 RVA: 0x00258F80 File Offset: 0x00257180
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, MapParent escapeShip)
		{
			if (escapeShip == null || !escapeShip.Spawned || escapeShip.GetComponent<EscapeShipComp>() == null)
			{
				return false;
			}
			if (escapeShip.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(escapeShip.EnterCooldownDaysLeft().ToString("0.#")));
			}
			return true;
		}

		// Token: 0x06006BA6 RID: 27558 RVA: 0x00258FE4 File Offset: 0x002571E4
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, MapParent escapeShip)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitEscapeShip>(() => CaravanArrivalAction_VisitEscapeShip.CanVisit(caravan, escapeShip), () => new CaravanArrivalAction_VisitEscapeShip(escapeShip.GetComponent<EscapeShipComp>()), "VisitEscapeShip".Translate(escapeShip.Label), caravan, escapeShip.Tile, escapeShip, null);
		}

		// Token: 0x04004336 RID: 17206
		private MapParent target;
	}
}
