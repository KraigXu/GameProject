using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200121E RID: 4638
	public class CaravanArrivalAction_OfferGifts : CaravanArrivalAction
	{
		// Token: 0x170011FD RID: 4605
		// (get) Token: 0x06006B89 RID: 27529 RVA: 0x00258931 File Offset: 0x00256B31
		public override string Label
		{
			get
			{
				return "OfferGifts".Translate();
			}
		}

		// Token: 0x170011FE RID: 4606
		// (get) Token: 0x06006B8A RID: 27530 RVA: 0x00258942 File Offset: 0x00256B42
		public override string ReportString
		{
			get
			{
				return "CaravanOfferingGifts".Translate(this.settlement.Label);
			}
		}

		// Token: 0x06006B8B RID: 27531 RVA: 0x00258549 File Offset: 0x00256749
		public CaravanArrivalAction_OfferGifts()
		{
		}

		// Token: 0x06006B8C RID: 27532 RVA: 0x00258963 File Offset: 0x00256B63
		public CaravanArrivalAction_OfferGifts(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06006B8D RID: 27533 RVA: 0x00258974 File Offset: 0x00256B74
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
			return CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, this.settlement);
		}

		// Token: 0x06006B8E RID: 27534 RVA: 0x002589C0 File Offset: 0x00256BC0
		public override void Arrived(Caravan caravan)
		{
			CameraJumper.TryJumpAndSelect(caravan);
			Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, null, null);
			Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, true));
		}

		// Token: 0x06006B8F RID: 27535 RVA: 0x002589F8 File Offset: 0x00256BF8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06006B90 RID: 27536 RVA: 0x00258A14 File Offset: 0x00256C14
		public static FloatMenuAcceptanceReport CanOfferGiftsTo(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && !settlement.HasMap && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow && CaravanArrivalAction_OfferGifts.HasNegotiator(caravan);
		}

		// Token: 0x06006B91 RID: 27537 RVA: 0x00258A84 File Offset: 0x00256C84
		private static bool HasNegotiator(Caravan caravan)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan, null, null);
			return pawn != null && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
		}

		// Token: 0x06006B92 RID: 27538 RVA: 0x00258AB8 File Offset: 0x00256CB8
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_OfferGifts>(() => CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, settlement), () => new CaravanArrivalAction_OfferGifts(settlement), "OfferGifts".Translate(), caravan, settlement.Tile, settlement, null);
		}

		// Token: 0x04004334 RID: 17204
		private Settlement settlement;
	}
}
