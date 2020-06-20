using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BF3 RID: 3059
	public static class NamePlayerFactionAndSettlementUtility
	{
		// Token: 0x060048BF RID: 18623 RVA: 0x0018BF7D File Offset: 0x0018A17D
		public static bool CanNameFactionNow()
		{
			return NamePlayerFactionAndSettlementUtility.CanNameFaction(Find.TickManager.TicksGame);
		}

		// Token: 0x060048C0 RID: 18624 RVA: 0x0018BF8E File Offset: 0x0018A18E
		public static bool CanNameSettlementNow(Settlement factionBase)
		{
			return NamePlayerFactionAndSettlementUtility.CanNameSettlement(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks);
		}

		// Token: 0x060048C1 RID: 18625 RVA: 0x0018BFA7 File Offset: 0x0018A1A7
		public static bool CanNameFactionSoon()
		{
			return NamePlayerFactionAndSettlementUtility.CanNameFaction(Find.TickManager.TicksGame + 30000);
		}

		// Token: 0x060048C2 RID: 18626 RVA: 0x0018BFBE File Offset: 0x0018A1BE
		public static bool CanNameSettlementSoon(Settlement factionBase)
		{
			return NamePlayerFactionAndSettlementUtility.CanNameSettlement(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks + 30000);
		}

		// Token: 0x060048C3 RID: 18627 RVA: 0x0018BFDD File Offset: 0x0018A1DD
		private static bool CanNameFaction(int ticksPassed)
		{
			return !Faction.OfPlayer.HasName && (float)ticksPassed / 60000f >= 4.3f && NamePlayerFactionAndSettlementUtility.CanNameAnythingNow();
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x0018C004 File Offset: 0x0018A204
		private static bool CanNameSettlement(Settlement factionBase, int ticksPassed)
		{
			return factionBase.Faction == Faction.OfPlayer && !factionBase.namedByPlayer && (float)ticksPassed / 60000f >= 4.3f && factionBase.HasMap && factionBase.Map.dangerWatcher.DangerRating != StoryDanger.High && factionBase.Map.mapPawns.FreeColonistsSpawnedCount != 0 && NamePlayerFactionAndSettlementUtility.CanNameAnythingNow();
		}

		// Token: 0x060048C5 RID: 18629 RVA: 0x0018C06C File Offset: 0x0018A26C
		private static bool CanNameAnythingNow()
		{
			if (Find.AnyPlayerHomeMap == null || Find.CurrentMap == null || !Find.CurrentMap.IsPlayerHome || Find.GameEnder.gameEnding)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					if (maps[i].mapPawns.FreeColonistsSpawnedCount >= 2)
					{
						flag = true;
					}
					if (!maps[i].attackTargetsCache.TargetsHostileToColony.Any((IAttackTarget x) => GenHostility.IsActiveThreatToPlayer(x)))
					{
						flag2 = true;
					}
				}
			}
			return flag && flag2;
		}

		// Token: 0x040029A5 RID: 10661
		private const float MinDaysPassedToNameFaction = 4.3f;

		// Token: 0x040029A6 RID: 10662
		private const float MinDaysPassedToNameSettlement = 4.3f;

		// Token: 0x040029A7 RID: 10663
		private const int SoonTicks = 30000;
	}
}
