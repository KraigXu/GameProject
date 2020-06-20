using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001131 RID: 4401
	public class QuestNode_GetNearbySettlement : QuestNode
	{
		// Token: 0x060066E0 RID: 26336 RVA: 0x002402E8 File Offset: 0x0023E4E8
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

		// Token: 0x060066E1 RID: 26337 RVA: 0x00240334 File Offset: 0x0023E534
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

		// Token: 0x060066E2 RID: 26338 RVA: 0x002403E4 File Offset: 0x0023E5E4
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

		// Token: 0x04003EFA RID: 16122
		public SlateRef<bool> allowActiveTradeRequest = true;

		// Token: 0x04003EFB RID: 16123
		public SlateRef<float> maxTileDistance;

		// Token: 0x04003EFC RID: 16124
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003EFD RID: 16125
		[NoTranslate]
		public SlateRef<string> storeFactionAs;

		// Token: 0x04003EFE RID: 16126
		[NoTranslate]
		public SlateRef<string> storeFactionLeaderAs;
	}
}
