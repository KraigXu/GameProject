using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011BF RID: 4543
	public static class SettlementProximityGoodwillUtility
	{
		// Token: 0x17001177 RID: 4471
		// (get) Token: 0x0600690E RID: 26894 RVA: 0x0024B378 File Offset: 0x00249578
		public static int MaxDist
		{
			get
			{
				return Mathf.RoundToInt(DiplomacyTuning.Goodwill_PerQuadrumFromSettlementProximity.Last<CurvePoint>().x);
			}
		}

		// Token: 0x0600690F RID: 26895 RVA: 0x0024B39C File Offset: 0x0024959C
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

		// Token: 0x06006910 RID: 26896 RVA: 0x0024B550 File Offset: 0x00249750
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

		// Token: 0x06006911 RID: 26897 RVA: 0x0024B610 File Offset: 0x00249810
		public static void SortProximityGoodwillOffsets(List<Pair<Settlement, int>> offsets)
		{
			offsets.SortBy((Pair<Settlement, int> x) => x.First.Faction.loadID, (Pair<Settlement, int> x) => -Mathf.Abs(x.Second));
		}

		// Token: 0x06006912 RID: 26898 RVA: 0x0024B664 File Offset: 0x00249864
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

		// Token: 0x06006913 RID: 26899 RVA: 0x0024B71C File Offset: 0x0024991C
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

		// Token: 0x04004159 RID: 16729
		private static List<Pair<Settlement, int>> tmpGoodwillOffsets = new List<Pair<Settlement, int>>();
	}
}
