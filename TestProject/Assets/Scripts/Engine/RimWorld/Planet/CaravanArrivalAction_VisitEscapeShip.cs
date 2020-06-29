﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public class CaravanArrivalAction_VisitEscapeShip : CaravanArrivalAction
	{
		
		// (get) Token: 0x06006B9D RID: 27549 RVA: 0x00258D49 File Offset: 0x00256F49
		public override string Label
		{
			get
			{
				return "VisitEscapeShip".Translate(this.target.Label);
			}
		}

		
		// (get) Token: 0x06006B9E RID: 27550 RVA: 0x00258D6A File Offset: 0x00256F6A
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(this.target.Label);
			}
		}

		
		public CaravanArrivalAction_VisitEscapeShip()
		{
		}

		
		public CaravanArrivalAction_VisitEscapeShip(EscapeShipComp escapeShip)
		{
			this.target = (MapParent)escapeShip.parent;
		}

		
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

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.target, "target", false);
		}

		
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

		
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, MapParent escapeShip)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitEscapeShip>(() => CaravanArrivalAction_VisitEscapeShip.CanVisit(caravan, escapeShip), () => new CaravanArrivalAction_VisitEscapeShip(escapeShip.GetComponent<EscapeShipComp>()), "VisitEscapeShip".Translate(escapeShip.Label), caravan, escapeShip.Tile, escapeShip, null);
		}

		
		private MapParent target;
	}
}
