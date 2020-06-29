﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public class CaravanArrivalAction_Enter : CaravanArrivalAction
	{
		
		// (get) Token: 0x06006B80 RID: 27520 RVA: 0x002586CE File Offset: 0x002568CE
		public override string Label
		{
			get
			{
				return "EnterMap".Translate(this.mapParent.Label);
			}
		}

		
		// (get) Token: 0x06006B81 RID: 27521 RVA: 0x002586EF File Offset: 0x002568EF
		public override string ReportString
		{
			get
			{
				return "CaravanEntering".Translate(this.mapParent.Label);
			}
		}

		
		public CaravanArrivalAction_Enter()
		{
		}

		
		public CaravanArrivalAction_Enter(MapParent mapParent)
		{
			this.mapParent = mapParent;
		}

		
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.mapParent != null && this.mapParent.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_Enter.CanEnter(caravan, this.mapParent);
		}

		
		public override void Arrived(Caravan caravan)
		{
			Map map = this.mapParent.Map;
			if (map == null)
			{
				return;
			}
			CaravanDropInventoryMode dropInventoryMode = map.IsPlayerHome ? CaravanDropInventoryMode.UnloadIndividually : CaravanDropInventoryMode.DoNotDrop;
			bool draftColonists = this.mapParent.Faction != null && this.mapParent.Faction.HostileTo(Faction.OfPlayer);
			if (caravan.IsPlayerControlled || this.mapParent.Faction == Faction.OfPlayer)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(this.mapParent), "LetterCaravanEnteredMap".Translate(caravan.Label, this.mapParent).CapitalizeFirst(), LetterDefOf.NeutralEvent, caravan.PawnsListForReading, null, null, null, null);
			}
			CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Edge, dropInventoryMode, draftColonists, null);
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		
		public static FloatMenuAcceptanceReport CanEnter(Caravan caravan, MapParent mapParent)
		{
			if (mapParent == null || !mapParent.Spawned || !mapParent.HasMap)
			{
				return false;
			}
			if (mapParent.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(mapParent.EnterCooldownDaysLeft().ToString("0.#")));
			}
			return true;
		}

		
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, MapParent mapParent)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_Enter>(() => CaravanArrivalAction_Enter.CanEnter(caravan, mapParent), () => new CaravanArrivalAction_Enter(mapParent), "EnterMap".Translate(mapParent.Label), caravan, mapParent.Tile, mapParent, null);
		}

		
		private MapParent mapParent;
	}
}
