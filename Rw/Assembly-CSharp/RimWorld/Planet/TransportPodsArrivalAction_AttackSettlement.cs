using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001269 RID: 4713
	public class TransportPodsArrivalAction_AttackSettlement : TransportPodsArrivalAction
	{
		// Token: 0x06006E41 RID: 28225 RVA: 0x0026813D File Offset: 0x0026633D
		public TransportPodsArrivalAction_AttackSettlement()
		{
		}

		// Token: 0x06006E42 RID: 28226 RVA: 0x00268145 File Offset: 0x00266345
		public TransportPodsArrivalAction_AttackSettlement(Settlement settlement, PawnsArrivalModeDef arrivalMode)
		{
			this.settlement = settlement;
			this.arrivalMode = arrivalMode;
		}

		// Token: 0x06006E43 RID: 28227 RVA: 0x0026815B File Offset: 0x0026635B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
		}

		// Token: 0x06006E44 RID: 28228 RVA: 0x00268184 File Offset: 0x00266384
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
			return TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, this.settlement);
		}

		// Token: 0x06006E45 RID: 28229 RVA: 0x002681CD File Offset: 0x002663CD
		public override bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return !this.settlement.HasMap;
		}

		// Token: 0x06006E46 RID: 28230 RVA: 0x002681E0 File Offset: 0x002663E0
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			Thing lookTarget = TransportPodsArrivalActionUtility.GetLookTarget(pods);
			bool flag = !this.settlement.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.settlement.Tile, null);
			TaggedString label = "LetterLabelCaravanEnteredEnemyBase".Translate();
			TaggedString text = "LetterTransportPodsLandedInEnemyBase".Translate(this.settlement.Label).CapitalizeFirst();
			SettlementUtility.AffectRelationsOnAttacked(this.settlement, ref text);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, lookTarget, this.settlement.Faction, null, null, null);
			this.arrivalMode.Worker.TravelingTransportPodsArrived(pods, orGenerateMap);
		}

		// Token: 0x06006E47 RID: 28231 RVA: 0x002682CC File Offset: 0x002664CC
		public static FloatMenuAcceptanceReport CanAttack(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			if (settlement == null || !settlement.Spawned || !settlement.Attackable)
			{
				return false;
			}
			if (!TransportPodsArrivalActionUtility.AnyNonDownedColonist(pods))
			{
				return false;
			}
			if (settlement.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(settlement.EnterCooldownDaysLeft().ToString("0.#")));
			}
			return true;
		}

		// Token: 0x06006E48 RID: 28232 RVA: 0x0026833D File Offset: 0x0026653D
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			Func<FloatMenuAcceptanceReport> <>9__0;
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter;
			if ((acceptanceReportGetter = <>9__0) == null)
			{
				acceptanceReportGetter = (<>9__0 = (() => TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, settlement)));
			}
			Func<TransportPodsArrivalAction_AttackSettlement> <>9__1;
			Func<TransportPodsArrivalAction_AttackSettlement> arrivalActionGetter;
			if ((arrivalActionGetter = <>9__1) == null)
			{
				arrivalActionGetter = (<>9__1 = (() => new TransportPodsArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.EdgeDrop)));
			}
			foreach (FloatMenuOption floatMenuOption in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(acceptanceReportGetter, arrivalActionGetter, "AttackAndDropAtEdge".Translate(settlement.Label), representative, settlement.Tile, null))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			Func<FloatMenuAcceptanceReport> <>9__2;
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter2;
			if ((acceptanceReportGetter2 = <>9__2) == null)
			{
				acceptanceReportGetter2 = (<>9__2 = (() => TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, settlement)));
			}
			Func<TransportPodsArrivalAction_AttackSettlement> <>9__3;
			Func<TransportPodsArrivalAction_AttackSettlement> arrivalActionGetter2;
			if ((arrivalActionGetter2 = <>9__3) == null)
			{
				arrivalActionGetter2 = (<>9__3 = (() => new TransportPodsArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.CenterDrop)));
			}
			foreach (FloatMenuOption floatMenuOption2 in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(acceptanceReportGetter2, arrivalActionGetter2, "AttackAndDropInCenter".Translate(settlement.Label), representative, settlement.Tile, null))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400441E RID: 17438
		private Settlement settlement;

		// Token: 0x0400441F RID: 17439
		private PawnsArrivalModeDef arrivalMode;
	}
}
