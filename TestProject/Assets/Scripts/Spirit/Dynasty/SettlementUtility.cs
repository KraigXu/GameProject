using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200125B RID: 4699
	public class SettlementUtility
	{
		// Token: 0x06006DE4 RID: 28132 RVA: 0x0026652C File Offset: 0x0026472C
		public static bool IsPlayerAttackingAnySettlementOf(Faction faction)
		{
			if (faction == Faction.OfPlayer)
			{
				return false;
			}
			if (!faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Settlement settlement = maps[i].info.parent as Settlement;
				if (settlement != null && settlement.Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006DE5 RID: 28133 RVA: 0x00266590 File Offset: 0x00264790
		public static void Attack(Caravan caravan, Settlement settlement)
		{
			if (!settlement.HasMap)
			{
				LongEventHandler.QueueLongEvent(delegate
				{
					SettlementUtility.AttackNow(caravan, settlement);
				}, "GeneratingMapForNewEncounter", false, null, true);
				return;
			}
			SettlementUtility.AttackNow(caravan, settlement);
		}

		// Token: 0x06006DE6 RID: 28134 RVA: 0x002665EC File Offset: 0x002647EC
		private static void AttackNow(Caravan caravan, Settlement settlement)
		{
			bool flag = !settlement.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(settlement.Tile, null);
			TaggedString label = "LetterLabelCaravanEnteredEnemyBase".Translate();
			TaggedString text = "LetterCaravanEnteredEnemyBase".Translate(caravan.Label, settlement.Label.ApplyTag(TagType.Settlement, settlement.Faction.GetUniqueLoadID())).CapitalizeFirst();
			SettlementUtility.AffectRelationsOnAttacked(settlement, ref text);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsSettlement".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, caravan.PawnsListForReading, settlement.Faction, null, null, null);
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.DoNotDrop, true, null);
		}

		// Token: 0x06006DE7 RID: 28135 RVA: 0x002666D4 File Offset: 0x002648D4
		public static void AffectRelationsOnAttacked(Settlement settlement, ref TaggedString letterText)
		{
			if (settlement.Faction != null && settlement.Faction != Faction.OfPlayer)
			{
				FactionRelationKind playerRelationKind = settlement.Faction.PlayerRelationKind;
				if (!settlement.Faction.HostileTo(Faction.OfPlayer))
				{
					settlement.Faction.TrySetRelationKind(Faction.OfPlayer, FactionRelationKind.Hostile, false, null, null);
				}
				else if (settlement.Faction.TryAffectGoodwillWith(Faction.OfPlayer, -50, false, false, null, null))
				{
					if (!letterText.NullOrEmpty())
					{
						letterText += "\n\n";
					}
					letterText += "RelationsWith".Translate(settlement.Faction.Name.ApplyTag(settlement.Faction)) + ": " + -50.ToStringWithSign();
				}
				settlement.Faction.TryAppendRelationKindChangedInfo(ref letterText, playerRelationKind, settlement.Faction.PlayerRelationKind, null);
			}
		}
	}
}
