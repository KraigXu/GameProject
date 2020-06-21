using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200126E RID: 4718
	public class TransportPodsArrivalAction_VisitSettlement : TransportPodsArrivalAction_FormCaravan
	{
		// Token: 0x06006E63 RID: 28259 RVA: 0x00268AF9 File Offset: 0x00266CF9
		public TransportPodsArrivalAction_VisitSettlement()
		{
		}

		// Token: 0x06006E64 RID: 28260 RVA: 0x00268B01 File Offset: 0x00266D01
		public TransportPodsArrivalAction_VisitSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06006E65 RID: 28261 RVA: 0x00268B10 File Offset: 0x00266D10
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06006E66 RID: 28262 RVA: 0x00268B2C File Offset: 0x00266D2C
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return TransportPodsArrivalAction_VisitSettlement.CanVisit(pods, this.settlement);
		}

		// Token: 0x06006E67 RID: 28263 RVA: 0x00268B75 File Offset: 0x00266D75
		public static FloatMenuAcceptanceReport CanVisit(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			if (settlement == null || !settlement.Spawned || !settlement.Visitable)
			{
				return false;
			}
			if (!TransportPodsArrivalActionUtility.AnyPotentialCaravanOwner(pods, Faction.OfPlayer))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06006E68 RID: 28264 RVA: 0x00268BAC File Offset: 0x00266DAC
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSettlement>(() => TransportPodsArrivalAction_VisitSettlement.CanVisit(pods, settlement), () => new TransportPodsArrivalAction_VisitSettlement(settlement), "VisitSettlement".Translate(settlement.Label), representative, settlement.Tile, null);
		}

		// Token: 0x04004427 RID: 17447
		private Settlement settlement;
	}
}
