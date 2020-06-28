using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001222 RID: 4642
	public class CaravanArrivalAction_VisitSettlement : CaravanArrivalAction
	{
		// Token: 0x17001205 RID: 4613
		// (get) Token: 0x06006BB0 RID: 27568 RVA: 0x002591A5 File Offset: 0x002573A5
		public override string Label
		{
			get
			{
				return "VisitSettlement".Translate(this.settlement.Label);
			}
		}

		// Token: 0x17001206 RID: 4614
		// (get) Token: 0x06006BB1 RID: 27569 RVA: 0x002591C6 File Offset: 0x002573C6
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(this.settlement.Label);
			}
		}

		// Token: 0x06006BB2 RID: 27570 RVA: 0x00258549 File Offset: 0x00256749
		public CaravanArrivalAction_VisitSettlement()
		{
		}

		// Token: 0x06006BB3 RID: 27571 RVA: 0x002591E7 File Offset: 0x002573E7
		public CaravanArrivalAction_VisitSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06006BB4 RID: 27572 RVA: 0x002591F8 File Offset: 0x002573F8
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_VisitSettlement.CanVisit(caravan, this.settlement);
		}

		// Token: 0x06006BB5 RID: 27573 RVA: 0x00259244 File Offset: 0x00257444
		public override void Arrived(Caravan caravan)
		{
			if (caravan.IsPlayerControlled)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(this.settlement), "LetterCaravanEnteredMap".Translate(caravan.Label, this.settlement).CapitalizeFirst(), LetterDefOf.NeutralEvent, caravan, null, null, null, null);
			}
		}

		// Token: 0x06006BB6 RID: 27574 RVA: 0x002592AF File Offset: 0x002574AF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06006BB7 RID: 27575 RVA: 0x002592C8 File Offset: 0x002574C8
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && settlement.Visitable;
		}

		// Token: 0x06006BB8 RID: 27576 RVA: 0x002592E4 File Offset: 0x002574E4
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSettlement>(() => CaravanArrivalAction_VisitSettlement.CanVisit(caravan, settlement), () => new CaravanArrivalAction_VisitSettlement(settlement), "VisitSettlement".Translate(settlement.Label), caravan, settlement.Tile, settlement, null);
		}

		// Token: 0x04004338 RID: 17208
		private Settlement settlement;
	}
}
