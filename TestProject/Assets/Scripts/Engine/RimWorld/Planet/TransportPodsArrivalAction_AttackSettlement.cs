using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public class TransportPodsArrivalAction_AttackSettlement : TransportPodsArrivalAction
	{
		
		public TransportPodsArrivalAction_AttackSettlement()
		{
		}

		
		public TransportPodsArrivalAction_AttackSettlement(Settlement settlement, PawnsArrivalModeDef arrivalMode)
		{
			this.settlement = settlement;
			this.arrivalMode = arrivalMode;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
		}

		
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

		
		public override bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return !this.settlement.HasMap;
		}

		
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

		
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter;
			if ((acceptanceReportGetter ) == null)
			{
				acceptanceReportGetter = (9__0 = (() => TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, settlement)));
			}
			
			Func<TransportPodsArrivalAction_AttackSettlement> arrivalActionGetter;
			if ((arrivalActionGetter ) == null)
			{
				arrivalActionGetter = (9__1 = (() => new TransportPodsArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.EdgeDrop)));
			}
			foreach (FloatMenuOption floatMenuOption in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(acceptanceReportGetter, arrivalActionGetter, "AttackAndDropAtEdge".Translate(settlement.Label), representative, settlement.Tile, null))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter2;
			if ((acceptanceReportGetter2 ) == null)
			{
				acceptanceReportGetter2 = (9__2 = (() => TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, settlement)));
			}
			
			Func<TransportPodsArrivalAction_AttackSettlement> arrivalActionGetter2;
			if ((arrivalActionGetter2 ) == null)
			{
				arrivalActionGetter2 = (9__3 = (() => new TransportPodsArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.CenterDrop)));
			}
			foreach (FloatMenuOption floatMenuOption2 in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(acceptanceReportGetter2, arrivalActionGetter2, "AttackAndDropInCenter".Translate(settlement.Label), representative, settlement.Tile, null))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		
		private Settlement settlement;

		
		private PawnsArrivalModeDef arrivalMode;
	}
}
