﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BEA RID: 3050
	public static class FactionGenerator
	{
		// Token: 0x06004881 RID: 18561 RVA: 0x0018A7A4 File Offset: 0x001889A4
		public static void GenerateFactionsIntoWorld()
		{
			int num = 0;
			using (IEnumerator<FactionDef> enumerator = DefDatabase<FactionDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FactionDef factionDef = enumerator.Current;
					for (int i = 0; i < factionDef.requiredCountAtGameStart; i++)
					{
						Faction faction = FactionGenerator.NewGeneratedFaction(factionDef);
						Find.FactionManager.Add(faction);
						if (!factionDef.hidden)
						{
							num++;
						}
					}
				}
				goto IL_A9;
			}
			IL_5F:
			Faction faction2 = FactionGenerator.NewGeneratedFaction((from fa in DefDatabase<FactionDef>.AllDefs
			where fa.canMakeRandomly && Find.FactionManager.AllFactions.Count((Faction f) => f.def == fa) < fa.maxCountAtGameStart
			select fa).RandomElement<FactionDef>());
			Find.World.factionManager.Add(faction2);
			num++;
			IL_A9:
			if (num >= 5)
			{
				int num2 = GenMath.RoundRandom((float)Find.WorldGrid.TilesCount / 100000f * FactionGenerator.SettlementsPer100kTiles.RandomInRange * Find.World.info.overallPopulation.GetScaleFactor());
				num2 -= Find.WorldObjects.Settlements.Count;
				for (int j = 0; j < num2; j++)
				{
					Faction faction3 = (from x in Find.World.factionManager.AllFactionsListForReading
					where !x.def.isPlayer && !x.def.hidden
					select x).RandomElementByWeight((Faction x) => x.def.settlementGenerationWeight);
					Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
					settlement.SetFaction(faction3);
					settlement.Tile = TileFinder.RandomSettlementTileFor(faction3, false, null);
					settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, null);
					Find.WorldObjects.Add(settlement);
				}
				return;
			}
			goto IL_5F;
		}

		// Token: 0x06004882 RID: 18562 RVA: 0x0018A970 File Offset: 0x00188B70
		public static void EnsureRequiredEnemies(Faction player)
		{
			using (IEnumerator<FactionDef> enumerator = DefDatabase<FactionDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FactionDef facDef = enumerator.Current;
					if (facDef.mustStartOneEnemy && Find.World.factionManager.AllFactions.Any((Faction f) => f.def == facDef) && !Find.World.factionManager.AllFactions.Any((Faction f) => f.def == facDef && f.HostileTo(player)))
					{
						Faction faction = (from f in Find.World.factionManager.AllFactions
						where f.def == facDef
						select f).RandomElement<Faction>();
						int num = faction.GoodwillWith(player);
						int goodwillChange = DiplomacyTuning.ForcedStartingEnemyGoodwillRange.RandomInRange - num;
						faction.TryAffectGoodwillWith(player, goodwillChange, false, false, null, null);
						faction.TrySetRelationKind(player, FactionRelationKind.Hostile, false, null, null);
					}
				}
			}
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x0018AAC8 File Offset: 0x00188CC8
		public static Faction NewGeneratedFaction(FactionDef facDef)
		{
			Faction faction = new Faction();
			faction.def = facDef;
			faction.loadID = Find.UniqueIDsManager.GetNextFactionID();
			faction.colorFromSpectrum = FactionGenerator.NewRandomColorFromSpectrum(faction);
			if (!facDef.isPlayer)
			{
				if (facDef.fixedName != null)
				{
					faction.Name = facDef.fixedName;
				}
				else
				{
					string text = "";
					for (int i = 0; i < 10; i++)
					{
						string text2 = NameGenerator.GenerateName(facDef.factionNameMaker, from fac in Find.FactionManager.AllFactionsVisible
						select fac.Name, false, null);
						if (text2.Length <= 20)
						{
							text = text2;
						}
					}
					if (text.NullOrEmpty())
					{
						text = NameGenerator.GenerateName(facDef.factionNameMaker, from fac in Find.FactionManager.AllFactionsVisible
						select fac.Name, false, null);
					}
					faction.Name = text;
				}
			}
			faction.centralMelanin = Rand.Value;
			foreach (Faction other in Find.FactionManager.AllFactionsListForReading)
			{
				faction.TryMakeInitialRelationsWith(other);
			}
			if (!facDef.hidden && !facDef.isPlayer)
			{
				Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
				settlement.SetFaction(faction);
				settlement.Tile = TileFinder.RandomSettlementTileFor(faction, false, null);
				settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, null);
				Find.WorldObjects.Add(settlement);
			}
			faction.GenerateNewLeader();
			return faction;
		}

		// Token: 0x06004884 RID: 18564 RVA: 0x0018AC7C File Offset: 0x00188E7C
		public static float NewRandomColorFromSpectrum(Faction faction)
		{
			float num = -1f;
			float result = 0f;
			for (int i = 0; i < 10; i++)
			{
				float value = Rand.Value;
				float num2 = 1f;
				List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
				for (int j = 0; j < allFactionsListForReading.Count; j++)
				{
					Faction faction2 = allFactionsListForReading[j];
					if (faction2.def == faction.def)
					{
						float num3 = Mathf.Abs(value - faction2.colorFromSpectrum);
						if (num3 < num2)
						{
							num2 = num3;
						}
					}
				}
				if (num2 > num)
				{
					num = num2;
					result = value;
				}
			}
			return result;
		}

		// Token: 0x04002990 RID: 10640
		private const int MinStartVisibleFactions = 5;

		// Token: 0x04002991 RID: 10641
		private const int MaxPreferredFactionNameLength = 20;

		// Token: 0x04002992 RID: 10642
		private static readonly FloatRange SettlementsPer100kTiles = new FloatRange(75f, 85f);
	}
}
