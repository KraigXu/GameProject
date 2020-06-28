using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200121F RID: 4639
	public class CaravanArrivalAction_Trade : CaravanArrivalAction
	{
		// Token: 0x170011FF RID: 4607
		// (get) Token: 0x06006B93 RID: 27539 RVA: 0x00258B1D File Offset: 0x00256D1D
		public override string Label
		{
			get
			{
				return "TradeWithSettlement".Translate(this.settlement.Label);
			}
		}

		// Token: 0x17001200 RID: 4608
		// (get) Token: 0x06006B94 RID: 27540 RVA: 0x00258B3E File Offset: 0x00256D3E
		public override string ReportString
		{
			get
			{
				return "CaravanTrading".Translate(this.settlement.Label);
			}
		}

		// Token: 0x06006B95 RID: 27541 RVA: 0x00258549 File Offset: 0x00256749
		public CaravanArrivalAction_Trade()
		{
		}

		// Token: 0x06006B96 RID: 27542 RVA: 0x00258B5F File Offset: 0x00256D5F
		public CaravanArrivalAction_Trade(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06006B97 RID: 27543 RVA: 0x00258B70 File Offset: 0x00256D70
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
			return CaravanArrivalAction_Trade.CanTradeWith(caravan, this.settlement);
		}

		// Token: 0x06006B98 RID: 27544 RVA: 0x00258BBC File Offset: 0x00256DBC
		public override void Arrived(Caravan caravan)
		{
			CameraJumper.TryJumpAndSelect(caravan);
			Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, this.settlement.Faction, this.settlement.TraderKind);
			Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, false));
		}

		// Token: 0x06006B99 RID: 27545 RVA: 0x00258C08 File Offset: 0x00256E08
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06006B9A RID: 27546 RVA: 0x00258C24 File Offset: 0x00256E24
		public static FloatMenuAcceptanceReport CanTradeWith(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && !settlement.HasMap && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && !settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow && CaravanArrivalAction_Trade.HasNegotiator(caravan, settlement);
		}

		// Token: 0x06006B9B RID: 27547 RVA: 0x00258C94 File Offset: 0x00256E94
		private static bool HasNegotiator(Caravan caravan, Settlement settlement)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan, settlement.Faction, settlement.TraderKind);
			return pawn != null && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
		}

		// Token: 0x06006B9C RID: 27548 RVA: 0x00258CD4 File Offset: 0x00256ED4
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_Trade>(() => CaravanArrivalAction_Trade.CanTradeWith(caravan, settlement), () => new CaravanArrivalAction_Trade(settlement), "TradeWith".Translate(settlement.Label), caravan, settlement.Tile, settlement, null);
		}

		// Token: 0x04004335 RID: 17205
		private Settlement settlement;
	}
}
