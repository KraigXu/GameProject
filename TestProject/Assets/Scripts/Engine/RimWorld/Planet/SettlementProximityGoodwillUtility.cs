using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public static class SettlementProximityGoodwillUtility
	{
		
		// (get) Token: 0x0600690E RID: 26894 RVA: 0x0024B378 File Offset: 0x00249578
		public static int MaxDist
		{
			get
			{
				return Mathf.RoundToInt(DiplomacyTuning.Goodwill_PerQuadrumFromSettlementProximity.Last<CurvePoint>().x);
			}
		}

		
		public static void CheckSettlementProximityGoodwillChange()
		{
			if (Find.TickManager.TicksGame == 0 || Find.TickManager.TicksGame % 900000 != 0)
			{
				return;
			}
			List<Settlement> settlements = Find.WorldObjects.Settlements;
			SettlementProximityGoodwillUtility.tmpGoodwillOffsets.Clear();
			for (int i = 0; i < settlements.Count; i++)
			{
				Settlement settlement = settlements[i];
				if (settlement.Faction == Faction.OfPlayer)
				{
					SettlementProximityGoodwillUtility.AppendProximityGoodwillOffsets(settlement.Tile, SettlementProximityGoodwillUtility.tmpGoodwillOffsets, true, false);
				}
			}
			if (!SettlementProximityGoodwillUtility.tmpGoodwillOffsets.Any<Pair<Settlement, int>>())
			{
				return;
			}
			SettlementProximityGoodwillUtility.SortProximityGoodwillOffsets(SettlementProximityGoodwillUtility.tmpGoodwillOffsets);
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			bool flag = false;
			TaggedString taggedString = "LetterFactionBaseProximity".Translate() + "\n\n" + SettlementProximityGoodwillUtility.ProximityGoodwillOffsetsToString(SettlementProximityGoodwillUtility.tmpGoodwillOffsets);
			for (int j = 0; j < allFactionsListForReading.Count; j++)
			{
				Faction faction = allFactionsListForReading[j];
				if (faction != Faction.OfPlayer)
				{
					int num = 0;
					for (int k = 0; k < SettlementProximityGoodwillUtility.tmpGoodwillOffsets.Count; k++)
					{
						if (SettlementProximityGoodwillUtility.tmpGoodwillOffsets[k].First.Faction == faction)
						{
							num += SettlementProximityGoodwillUtility.tmpGoodwillOffsets[k].Second;
						}
					}
					FactionRelationKind playerRelationKind = faction.PlayerRelationKind;
					if (faction.TryAffectGoodwillWith(Faction.OfPlayer, num, false, false, null, null))
					{
						flag = true;
						faction.TryAppendRelationKindChangedInfo(ref taggedString, playerRelationKind, faction.PlayerRelationKind, null);
					}
				}
			}
			if (flag)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelFactionBaseProximity".Translate(), taggedString, LetterDefOf.NegativeEvent, null);
			}
		}

		
		public static void AppendProximityGoodwillOffsets(int tile, List<Pair<Settlement, int>> outOffsets, bool ignoreIfAlreadyMinGoodwill, bool ignorePermanentlyHostile)
		{
			int maxDist = SettlementProximityGoodwillUtility.MaxDist;
			List<Settlement> settlements = Find.WorldObjects.Settlements;
			for (int i = 0; i < settlements.Count; i++)
			{
				Settlement settlement = settlements[i];
				if (settlement.Faction != null && settlement.Faction != Faction.OfPlayer && (!ignorePermanentlyHostile || !settlement.Faction.def.permanentEnemy) && (!ignoreIfAlreadyMinGoodwill || settlement.Faction.PlayerGoodwill != -100))
				{
					int num = Find.WorldGrid.TraversalDistanceBetween(tile, settlement.Tile, false, maxDist);
					if (num != 2147483647)
					{
						int num2 = Mathf.RoundToInt(DiplomacyTuning.Goodwill_PerQuadrumFromSettlementProximity.Evaluate((float)num));
						if (num2 != 0)
						{
							outOffsets.Add(new Pair<Settlement, int>(settlement, num2));
						}
					}
				}
			}
		}

		
		public static void SortProximityGoodwillOffsets(List<Pair<Settlement, int>> offsets)
		{
			offsets.SortBy((Pair<Settlement, int> x) => x.First.Faction.loadID, (Pair<Settlement, int> x) => -Mathf.Abs(x.Second));
		}

		
		public static string ProximityGoodwillOffsetsToString(List<Pair<Settlement, int>> offsets)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < offsets.Count; i++)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("  - " + offsets[i].First.LabelCap + ": " + "ProximitySingleGoodwillChange".Translate(offsets[i].Second.ToStringWithSign(), offsets[i].First.Faction.Name));
			}
			return stringBuilder.ToString();
		}

		
		public static void CheckConfirmSettle(int tile, Action settleAction)
		{
			SettlementProximityGoodwillUtility.tmpGoodwillOffsets.Clear();
			SettlementProximityGoodwillUtility.AppendProximityGoodwillOffsets(tile, SettlementProximityGoodwillUtility.tmpGoodwillOffsets, false, true);
			if (SettlementProximityGoodwillUtility.tmpGoodwillOffsets.Any<Pair<Settlement, int>>())
			{
				SettlementProximityGoodwillUtility.SortProximityGoodwillOffsets(SettlementProximityGoodwillUtility.tmpGoodwillOffsets);
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSettleNearFactionBase".Translate(SettlementProximityGoodwillUtility.MaxDist - 1, 15) + "\n\n" + SettlementProximityGoodwillUtility.ProximityGoodwillOffsetsToString(SettlementProximityGoodwillUtility.tmpGoodwillOffsets), settleAction, false, null));
				return;
			}
			settleAction();
		}

		
		private static List<Pair<Settlement, int>> tmpGoodwillOffsets = new List<Pair<Settlement, int>>();
	}
}
