using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public class CaravanArrivalAction_AttackSettlement : CaravanArrivalAction
	{
		
		// (get) Token: 0x06006B77 RID: 27511 RVA: 0x00258507 File Offset: 0x00256707
		public override string Label
		{
			get
			{
				return "AttackSettlement".Translate(this.settlement.Label);
			}
		}

		
		// (get) Token: 0x06006B78 RID: 27512 RVA: 0x00258528 File Offset: 0x00256728
		public override string ReportString
		{
			get
			{
				return "CaravanAttacking".Translate(this.settlement.Label);
			}
		}

		
		public CaravanArrivalAction_AttackSettlement()
		{
		}

		
		public CaravanArrivalAction_AttackSettlement(Settlement settlement)
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
			return CaravanArrivalAction_AttackSettlement.CanAttack(caravan, this.settlement);
		}

		
		public override void Arrived(Caravan caravan)
		{
			SettlementUtility.Attack(caravan, this.settlement);
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		
		public static FloatMenuAcceptanceReport CanAttack(Caravan caravan, Settlement settlement)
		{
			if (settlement == null || !settlement.Spawned || !settlement.Attackable)
			{
				return false;
			}
			if (settlement.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(settlement.EnterCooldownDaysLeft().ToString("0.#")));
			}
			return true;
		}

		
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_AttackSettlement>(() => CaravanArrivalAction_AttackSettlement.CanAttack(caravan, settlement), () => new CaravanArrivalAction_AttackSettlement(settlement), "AttackSettlement".Translate(settlement.Label), caravan, settlement.Tile, settlement, settlement.Faction.AllyOrNeutralTo(Faction.OfPlayer) ? delegate(Action action)
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmAttackFriendlyFaction".Translate(settlement.LabelCap, settlement.Faction.Name), delegate
				{
					action();
				}, false, null));
			} : null);
		}

		
		private Settlement settlement;
	}
}
