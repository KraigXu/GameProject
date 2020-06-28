using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001249 RID: 4681
	[StaticConstructorOnStartup]
	public static class SettleUtility
	{
		// Token: 0x17001239 RID: 4665
		// (get) Token: 0x06006D0B RID: 27915 RVA: 0x00262E7C File Offset: 0x0026107C
		public static bool PlayerSettlementsCountLimitReached
		{
			get
			{
				int num = 0;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome && maps[i].Parent is Settlement)
					{
						num++;
					}
				}
				return num >= Prefs.MaxNumberOfPlayerSettlements;
			}
		}

		// Token: 0x06006D0C RID: 27916 RVA: 0x00262ED4 File Offset: 0x002610D4
		public static Settlement AddNewHome(int tile, Faction faction)
		{
			Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
			settlement.Tile = tile;
			settlement.SetFaction(faction);
			settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, null);
			Find.WorldObjects.Add(settlement);
			return settlement;
		}

		// Token: 0x040043CA RID: 17354
		public static readonly Texture2D SettleCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Settle", true);
	}
}
