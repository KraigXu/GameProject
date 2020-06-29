using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public class CaravanArrivalAction_Trade : CaravanArrivalAction
	{
		
		// (get) Token: 0x06006B93 RID: 27539 RVA: 0x00258B1D File Offset: 0x00256D1D
		public override string Label
		{
			get
			{
				return "TradeWithSettlement".Translate(this.settlement.Label);
			}
		}

		
		// (get) Token: 0x06006B94 RID: 27540 RVA: 0x00258B3E File Offset: 0x00256D3E
		public override string ReportString
		{
			get
			{
				return "CaravanTrading".Translate(this.settlement.Label);
			}
		}

		
		public CaravanArrivalAction_Trade()
		{
		}

		
		public CaravanArrivalAction_Trade(Settlement settlement)
		{
			this.settlement = settlement;
		}

		
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

		
		public override void Arrived(Caravan caravan)
		{
			CameraJumper.TryJumpAndSelect(caravan);
			Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, this.settlement.Faction, this.settlement.TraderKind);
			Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, false));
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		
		public static FloatMenuAcceptanceReport CanTradeWith(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && !settlement.HasMap && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && !settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow && CaravanArrivalAction_Trade.HasNegotiator(caravan, settlement);
		}

		
		private static bool HasNegotiator(Caravan caravan, Settlement settlement)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan, settlement.Faction, settlement.TraderKind);
			return pawn != null && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
		}

		
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_Trade>(() => CaravanArrivalAction_Trade.CanTradeWith(caravan, settlement), () => new CaravanArrivalAction_Trade(settlement), "TradeWith".Translate(settlement.Label), caravan, settlement.Tile, settlement, null);
		}

		
		private Settlement settlement;
	}
}
