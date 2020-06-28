using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020009D1 RID: 2513
	public class IncidentWorker_CaravanMeeting : IncidentWorker
	{
		// Token: 0x06003C09 RID: 15369 RVA: 0x0013CC20 File Offset: 0x0013AE20
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Faction faction;
			return parms.target is Map || (CaravanIncidentUtility.CanFireIncidentWhichWantsToGenerateMapAt(parms.target.Tile) && this.TryFindFaction(out faction));
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x0013CC58 File Offset: 0x0013AE58
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (parms.target is Map)
			{
				return IncidentDefOf.TravelerGroup.Worker.TryExecute(parms);
			}
			Caravan caravan = (Caravan)parms.target;
			Faction faction;
			if (!this.TryFindFaction(out faction))
			{
				return false;
			}
			List<Pawn> list = this.GenerateCaravanPawns(faction);
			if (!list.Any<Pawn>())
			{
				Log.Error("IncidentWorker_CaravanMeeting could not generate any pawns.", false);
				return false;
			}
			Caravan metCaravan = CaravanMaker.MakeCaravan(list, faction, -1, false);
			CameraJumper.TryJumpAndSelect(caravan);
			DiaNode diaNode = new DiaNode("CaravanMeeting".Translate(caravan.Name, faction.Name, PawnUtility.PawnKindsToLineList(from x in metCaravan.PawnsListForReading
			select x.kindDef, "  - ")).CapitalizeFirst());
			Pawn bestPlayerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, faction, metCaravan.TraderKind);
			if (metCaravan.CanTradeNow)
			{
				DiaOption diaOption = new DiaOption("CaravanMeeting_Trade".Translate());
				diaOption.action = delegate
				{
					Find.WindowStack.Add(new Dialog_Trade(bestPlayerNegotiator, metCaravan, false));
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(metCaravan.Goods.OfType<Pawn>(), "LetterRelatedPawnsTradingWithOtherCaravan".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, false, true);
				};
				if (bestPlayerNegotiator == null)
				{
					if (metCaravan.TraderKind.permitRequiredForTrading != null && !caravan.pawns.Any((Pawn p) => p.royalty != null && p.royalty.HasPermit(metCaravan.TraderKind.permitRequiredForTrading, faction)))
					{
						RoyalTitleDef royalTitleDef = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.First((RoyalTitleDef t) => t.permits != null && t.permits.Contains(metCaravan.TraderKind.permitRequiredForTrading));
						diaOption.Disable("CaravanMeeting_NoPermit".Translate(royalTitleDef.GetLabelForBothGenders(), faction).Resolve());
					}
					else
					{
						diaOption.Disable("CaravanMeeting_TradeIncapable".Translate());
					}
				}
				diaNode.options.Add(diaOption);
			}
			DiaOption diaOption2 = new DiaOption("CaravanMeeting_Attack".Translate());
			Action <>9__5;
			diaOption2.action = delegate
			{
				Action action;
				if ((action = <>9__5) == null)
				{
					action = (<>9__5 = delegate
					{
						Pawn t = caravan.PawnsListForReading[0];
						faction.TrySetRelationKind(Faction.OfPlayer, FactionRelationKind.Hostile, true, "GoodwillChangedReason_AttackedCaravan".Translate(), new GlobalTargetInfo?(t));
						Map map = CaravanIncidentUtility.GetOrGenerateMapForIncident(caravan, new IntVec3(100, 1, 100), WorldObjectDefOf.AttackedNonPlayerCaravan);
						IntVec3 playerSpot;
						IntVec3 enemySpot;
						MultipleCaravansCellFinder.FindStartingCellsFor2Groups(map, out playerSpot, out enemySpot);
						CaravanEnterMapUtility.Enter(caravan, map, (Pawn p) => CellFinder.RandomClosewalkCellNear(playerSpot, map, 12, null), CaravanDropInventoryMode.DoNotDrop, true);
						List<Pawn> list2 = metCaravan.PawnsListForReading.ToList<Pawn>();
						CaravanEnterMapUtility.Enter(metCaravan, map, (Pawn p) => CellFinder.RandomClosewalkCellNear(enemySpot, map, 12, null), CaravanDropInventoryMode.DoNotDrop, false);
						LordMaker.MakeNewLord(faction, new LordJob_DefendAttackedTraderCaravan(list2[0].Position), map, list2);
						Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
						CameraJumper.TryJumpAndSelect(t);
						PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(list2, "LetterRelatedPawnsGroupGeneric".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, true, true);
					});
				}
				LongEventHandler.QueueLongEvent(action, "GeneratingMapForNewEncounter", false, null, true);
			};
			diaOption2.resolveTree = true;
			diaNode.options.Add(diaOption2);
			DiaOption diaOption3 = new DiaOption("CaravanMeeting_MoveOn".Translate());
			diaOption3.action = delegate
			{
				this.RemoveAllPawnsAndPassToWorld(metCaravan);
			};
			diaOption3.resolveTree = true;
			diaNode.options.Add(diaOption3);
			string title = "CaravanMeetingTitle".Translate(caravan.Label);
			Find.WindowStack.Add(new Dialog_NodeTreeWithFactionInfo(diaNode, faction, true, false, title));
			Find.Archive.Add(new ArchivedDialog(diaNode.text, title, faction));
			return true;
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x0013CF5F File Offset: 0x0013B15F
		private bool TryFindFaction(out Faction faction)
		{
			return (from x in Find.FactionManager.AllFactionsListForReading
			where !x.IsPlayer && !x.HostileTo(Faction.OfPlayer) && !x.def.hidden && x.def.humanlikeFaction && x.def.caravanTraderKinds.Any<TraderKindDef>() && !x.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>()
			select x).TryRandomElement(out faction);
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x0013CF95 File Offset: 0x0013B195
		private List<Pawn> GenerateCaravanPawns(Faction faction)
		{
			return PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Trader,
				faction = faction,
				points = TraderCaravanUtility.GenerateGuardPoints(),
				dontUseSingleUseRocketLaunchers = true
			}, true).ToList<Pawn>();
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x0013CFCC File Offset: 0x0013B1CC
		private void RemoveAllPawnsAndPassToWorld(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Find.WorldPawns.PassToWorld(pawnsListForReading[i], PawnDiscardDecideMode.Decide);
			}
			caravan.RemoveAllPawns();
		}

		// Token: 0x04002370 RID: 9072
		private const int MapSize = 100;
	}
}
