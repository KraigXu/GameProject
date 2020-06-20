using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200121C RID: 4636
	public class CaravanArrivalAction_AttackSettlement : CaravanArrivalAction
	{
		// Token: 0x170011F9 RID: 4601
		// (get) Token: 0x06006B77 RID: 27511 RVA: 0x00258507 File Offset: 0x00256707
		public override string Label
		{
			get
			{
				return "AttackSettlement".Translate(this.settlement.Label);
			}
		}

		// Token: 0x170011FA RID: 4602
		// (get) Token: 0x06006B78 RID: 27512 RVA: 0x00258528 File Offset: 0x00256728
		public override string ReportString
		{
			get
			{
				return "CaravanAttacking".Translate(this.settlement.Label);
			}
		}

		// Token: 0x06006B79 RID: 27513 RVA: 0x00258549 File Offset: 0x00256749
		public CaravanArrivalAction_AttackSettlement()
		{
		}

		// Token: 0x06006B7A RID: 27514 RVA: 0x00258551 File Offset: 0x00256751
		public CaravanArrivalAction_AttackSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06006B7B RID: 27515 RVA: 0x00258560 File Offset: 0x00256760
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

		// Token: 0x06006B7C RID: 27516 RVA: 0x002585A9 File Offset: 0x002567A9
		public override void Arrived(Caravan caravan)
		{
			SettlementUtility.Attack(caravan, this.settlement);
		}

		// Token: 0x06006B7D RID: 27517 RVA: 0x002585B7 File Offset: 0x002567B7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06006B7E RID: 27518 RVA: 0x002585D0 File Offset: 0x002567D0
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

		// Token: 0x06006B7F RID: 27519 RVA: 0x00258634 File Offset: 0x00256834
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

		// Token: 0x04004332 RID: 17202
		private Settlement settlement;
	}
}
