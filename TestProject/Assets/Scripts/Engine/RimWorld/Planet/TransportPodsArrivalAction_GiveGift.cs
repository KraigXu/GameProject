using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200126B RID: 4715
	public class TransportPodsArrivalAction_GiveGift : TransportPodsArrivalAction
	{
		// Token: 0x06006E4E RID: 28238 RVA: 0x0026813D File Offset: 0x0026633D
		public TransportPodsArrivalAction_GiveGift()
		{
		}

		// Token: 0x06006E4F RID: 28239 RVA: 0x00268503 File Offset: 0x00266703
		public TransportPodsArrivalAction_GiveGift(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06006E50 RID: 28240 RVA: 0x00268512 File Offset: 0x00266712
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06006E51 RID: 28241 RVA: 0x0026852C File Offset: 0x0026672C
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
			return TransportPodsArrivalAction_GiveGift.CanGiveGiftTo(pods, this.settlement);
		}

		// Token: 0x06006E52 RID: 28242 RVA: 0x00268578 File Offset: 0x00266778
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			for (int i = 0; i < pods.Count; i++)
			{
				for (int j = 0; j < pods[i].innerContainer.Count; j++)
				{
					Pawn pawn = pods[i].innerContainer[j] as Pawn;
					if (pawn != null)
					{
						if (pawn.RaceProps.Humanlike)
						{
							GenGuest.AddPrisonerSoldThoughts(pawn);
						}
						else if (pawn.RaceProps.Animal && pawn.relations != null)
						{
							Pawn firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, null);
							if (firstDirectRelationPawn != null && firstDirectRelationPawn.needs.mood != null)
							{
								pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Bond, firstDirectRelationPawn);
								firstDirectRelationPawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SoldMyBondedAnimalMood, null);
							}
						}
					}
				}
			}
			FactionGiftUtility.GiveGift(pods, this.settlement);
		}

		// Token: 0x06006E53 RID: 28243 RVA: 0x00268664 File Offset: 0x00266864
		public static FloatMenuAcceptanceReport CanGiveGiftTo(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			foreach (IThingHolder thingHolder in pods)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				for (int i = 0; i < directlyHeldThings.Count; i++)
				{
					Pawn p;
					if ((p = (directlyHeldThings[i] as Pawn)) != null && p.IsQuestLodger())
					{
						return false;
					}
				}
			}
			return settlement != null && settlement.Spawned && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && !settlement.HasMap;
		}

		// Token: 0x06006E54 RID: 28244 RVA: 0x00268724 File Offset: 0x00266924
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			if (settlement.Faction == Faction.OfPlayer)
			{
				return Enumerable.Empty<FloatMenuOption>();
			}
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_GiveGift>(() => TransportPodsArrivalAction_GiveGift.CanGiveGiftTo(pods, settlement), () => new TransportPodsArrivalAction_GiveGift(settlement), "GiveGiftViaTransportPods".Translate(settlement.Faction.Name, FactionGiftUtility.GetGoodwillChange(pods, settlement).ToStringWithSign()), representative, settlement.Tile, delegate(Action action)
			{
				TradeRequestComp tradeReqComp = settlement.GetComponent<TradeRequestComp>();
				if (tradeReqComp.ActiveRequest && pods.Any((IThingHolder p) => p.GetDirectlyHeldThings().Contains(tradeReqComp.requestThingDef)))
				{
					Find.WindowStack.Add(new Dialog_MessageBox("GiveGiftViaTransportPodsTradeRequestWarning".Translate(), "Yes".Translate(), delegate
					{
						action();
					}, "No".Translate(), null, null, false, null, null));
					return;
				}
				action();
			});
		}

		// Token: 0x04004422 RID: 17442
		private Settlement settlement;
	}
}
