using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200121D RID: 4637
	public class CaravanArrivalAction_Enter : CaravanArrivalAction
	{
		// Token: 0x170011FB RID: 4603
		// (get) Token: 0x06006B80 RID: 27520 RVA: 0x002586CE File Offset: 0x002568CE
		public override string Label
		{
			get
			{
				return "EnterMap".Translate(this.mapParent.Label);
			}
		}

		// Token: 0x170011FC RID: 4604
		// (get) Token: 0x06006B81 RID: 27521 RVA: 0x002586EF File Offset: 0x002568EF
		public override string ReportString
		{
			get
			{
				return "CaravanEntering".Translate(this.mapParent.Label);
			}
		}

		// Token: 0x06006B82 RID: 27522 RVA: 0x00258549 File Offset: 0x00256749
		public CaravanArrivalAction_Enter()
		{
		}

		// Token: 0x06006B83 RID: 27523 RVA: 0x00258710 File Offset: 0x00256910
		public CaravanArrivalAction_Enter(MapParent mapParent)
		{
			this.mapParent = mapParent;
		}

		// Token: 0x06006B84 RID: 27524 RVA: 0x00258720 File Offset: 0x00256920
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

		// Token: 0x06006B85 RID: 27525 RVA: 0x0025876C File Offset: 0x0025696C
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

		// Token: 0x06006B86 RID: 27526 RVA: 0x0025883C File Offset: 0x00256A3C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x06006B87 RID: 27527 RVA: 0x00258858 File Offset: 0x00256A58
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

		// Token: 0x06006B88 RID: 27528 RVA: 0x002588BC File Offset: 0x00256ABC
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, MapParent mapParent)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_Enter>(() => CaravanArrivalAction_Enter.CanEnter(caravan, mapParent), () => new CaravanArrivalAction_Enter(mapParent), "EnterMap".Translate(mapParent.Label), caravan, mapParent.Tile, mapParent, null);
		}

		// Token: 0x04004333 RID: 17203
		private MapParent mapParent;
	}
}
