using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetNearbySettlement : QuestNode
	{
		
		private Settlement RandomNearbyTradeableSettlement(int originTile, Slate slate)
		{
			return Find.WorldObjects.SettlementBases.Where(delegate(Settlement settlement)
			{
				if (!settlement.Visitable)
				{
					return false;
				}
				if (!this.allowActiveTradeRequest.GetValue(slate))
				{
					if (settlement.GetComponent<TradeRequestComp>() != null && settlement.GetComponent<TradeRequestComp>().ActiveRequest)
					{
						return false;
					}
					List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
					for (int i = 0; i < questsListForReading.Count; i++)
					{
						if (!questsListForReading[i].Historical)
						{
							List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
							for (int j = 0; j < partsListForReading.Count; j++)
							{
								QuestPart_InitiateTradeRequest questPart_InitiateTradeRequest;
								if ((questPart_InitiateTradeRequest = (partsListForReading[j] as QuestPart_InitiateTradeRequest)) != null && questPart_InitiateTradeRequest.settlement == settlement)
								{
									return false;
								}
							}
						}
					}
				}
				return Find.WorldGrid.ApproxDistanceInTiles(originTile, settlement.Tile) < this.maxTileDistance.GetValue(slate) && Find.WorldReachability.CanReach(originTile, settlement.Tile);
			}).RandomElementWithFallback(null);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			Settlement settlement = this.RandomNearbyTradeableSettlement(map.Tile, slate);
			QuestGen.slate.Set<Settlement>(this.storeAs.GetValue(slate), settlement, false);
			if (!string.IsNullOrEmpty(this.storeFactionAs.GetValue(slate)))
			{
				QuestGen.slate.Set<Faction>(this.storeFactionAs.GetValue(slate), settlement.Faction, false);
			}
			if (!this.storeFactionLeaderAs.GetValue(slate).NullOrEmpty())
			{
				QuestGen.slate.Set<Pawn>(this.storeFactionLeaderAs.GetValue(slate), settlement.Faction.leader, false);
			}
		}

		
		protected override bool TestRunInt(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			Settlement settlement = this.RandomNearbyTradeableSettlement(map.Tile, slate);
			if (map != null && settlement != null)
			{
				slate.Set<Settlement>(this.storeAs.GetValue(slate), settlement, false);
				if (!string.IsNullOrEmpty(this.storeFactionAs.GetValue(slate)))
				{
					slate.Set<Faction>(this.storeFactionAs.GetValue(slate), settlement.Faction, false);
				}
				if (!string.IsNullOrEmpty(this.storeFactionLeaderAs.GetValue(slate)))
				{
					slate.Set<Pawn>(this.storeFactionLeaderAs.GetValue(slate), settlement.Faction.leader, false);
				}
				return true;
			}
			return false;
		}

		
		public SlateRef<bool> allowActiveTradeRequest = true;

		
		public SlateRef<float> maxTileDistance;

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		[NoTranslate]
		public SlateRef<string> storeFactionAs;

		
		[NoTranslate]
		public SlateRef<string> storeFactionLeaderAs;
	}
}
