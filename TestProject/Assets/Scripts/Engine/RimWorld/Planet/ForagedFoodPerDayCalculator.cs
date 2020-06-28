using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001243 RID: 4675
	public static class ForagedFoodPerDayCalculator
	{
		// Token: 0x06006CF0 RID: 27888 RVA: 0x00261E80 File Offset: 0x00260080
		public static Pair<ThingDef, float> ForagedFoodPerDay(List<Pawn> pawns, BiomeDef biome, Faction faction, bool caravanMovingNow, bool caravanNightResting, StringBuilder explanation = null)
		{
			float foragedFoodCountPerInterval = ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(pawns, biome, faction, explanation);
			float progressPerTick = ForagedFoodPerDayCalculator.GetProgressPerTick(caravanMovingNow, caravanNightResting, explanation);
			float num = foragedFoodCountPerInterval * progressPerTick * 60000f;
			float num2;
			if (num != 0f)
			{
				num2 = num * biome.foragedFood.GetStatValueAbstract(StatDefOf.Nutrition, null);
			}
			else
			{
				num2 = 0f;
			}
			if (explanation != null)
			{
				explanation.AppendLine();
				explanation.AppendLine();
				TaggedString taggedString = "TotalNutrition".Translate() + ": " + num2.ToString("0.##") + " / " + "day".Translate();
				if (num2 > 0f)
				{
					taggedString += "\n= " + biome.LabelCap + ": " + biome.foragedFood.LabelCap + " x" + num.ToString("0.##") + " / " + "day".Translate();
				}
				explanation.Append(taggedString);
			}
			return new Pair<ThingDef, float>(biome.foragedFood, num);
		}

		// Token: 0x06006CF1 RID: 27889 RVA: 0x00261FAC File Offset: 0x002601AC
		public static float GetProgressPerTick(bool caravanMovingNow, bool caravanNightResting, StringBuilder explanation = null)
		{
			float num = 0.0001f;
			if (!caravanMovingNow && !caravanNightResting)
			{
				num *= 2f;
				if (explanation != null)
				{
					explanation.AppendLine();
					explanation.Append("CaravanNotMoving".Translate() + ": " + 2f.ToStringPercent());
				}
			}
			return num;
		}

		// Token: 0x06006CF2 RID: 27890 RVA: 0x00262008 File Offset: 0x00260208
		public static float GetForagedFoodCountPerInterval(List<Pawn> pawns, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			float num = (biome.foragedFood != null) ? biome.forageability : 0f;
			if (explanation != null)
			{
				explanation.Append("ForagedNutritionPerDay".Translate() + ":");
			}
			float num2 = 0f;
			bool flag = false;
			int i = 0;
			int count = pawns.Count;
			while (i < count)
			{
				Pawn pawn = pawns[i];
				bool flag2;
				float baseForagedNutritionPerDay = ForagedFoodPerDayCalculator.GetBaseForagedNutritionPerDay(pawn, out flag2);
				if (!flag2)
				{
					num2 += baseForagedNutritionPerDay;
					flag = true;
					if (explanation != null)
					{
						explanation.AppendLine();
						explanation.Append("  - " + pawn.LabelShortCap + ": +" + baseForagedNutritionPerDay.ToString("0.##"));
					}
				}
				i++;
			}
			float num3 = num2;
			num2 /= 6f;
			if (explanation != null)
			{
				explanation.AppendLine();
				if (flag)
				{
					explanation.Append("  = " + num3.ToString("0.##"));
				}
				else
				{
					explanation.Append("  (" + "NoneCapable".Translate().ToLower() + ")");
				}
				explanation.AppendLine();
				explanation.AppendLine();
				explanation.Append("Biome".Translate() + ": x" + num.ToStringPercent() + " (" + biome.label + ")");
				if (faction.def.forageabilityFactor != 1f)
				{
					explanation.AppendLine();
					explanation.Append("  " + "FactionType".Translate() + ": " + faction.def.forageabilityFactor.ToStringPercent());
				}
			}
			num2 *= num;
			num2 *= faction.def.forageabilityFactor;
			if (biome.foragedFood != null)
			{
				return num2 / biome.foragedFood.ingestible.CachedNutrition;
			}
			return num2;
		}

		// Token: 0x06006CF3 RID: 27891 RVA: 0x0026220C File Offset: 0x0026040C
		public static float GetBaseForagedNutritionPerDay(Pawn p, out bool skip)
		{
			if (!p.IsFreeColonist || p.InMentalState || p.Downed || p.CarriedByCaravan())
			{
				skip = true;
				return 0f;
			}
			skip = false;
			if (!StatDefOf.ForagedNutritionPerDay.Worker.IsDisabledFor(p))
			{
				return p.GetStatValue(StatDefOf.ForagedNutritionPerDay, true);
			}
			return 0f;
		}

		// Token: 0x06006CF4 RID: 27892 RVA: 0x00262269 File Offset: 0x00260469
		public static Pair<ThingDef, float> ForagedFoodPerDay(Caravan caravan, StringBuilder explanation = null)
		{
			return ForagedFoodPerDayCalculator.ForagedFoodPerDay(caravan.PawnsListForReading, caravan.Biome, caravan.Faction, caravan.pather.MovingNow, caravan.NightResting, explanation);
		}

		// Token: 0x06006CF5 RID: 27893 RVA: 0x00262294 File Offset: 0x00260494
		public static float GetProgressPerTick(Caravan caravan, StringBuilder explanation = null)
		{
			return ForagedFoodPerDayCalculator.GetProgressPerTick(caravan.pather.MovingNow, caravan.NightResting, explanation);
		}

		// Token: 0x06006CF6 RID: 27894 RVA: 0x002622AD File Offset: 0x002604AD
		public static float GetForagedFoodCountPerInterval(Caravan caravan, StringBuilder explanation = null)
		{
			return ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(caravan.PawnsListForReading, caravan.Biome, caravan.Faction, explanation);
		}

		// Token: 0x06006CF7 RID: 27895 RVA: 0x002622C8 File Offset: 0x002604C8
		public static Pair<ThingDef, float> ForagedFoodPerDay(List<TransferableOneWay> transferables, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = 0; j < transferableOneWay.CountToTransfer; j++)
					{
						ForagedFoodPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			Pair<ThingDef, float> result = ForagedFoodPerDayCalculator.ForagedFoodPerDay(ForagedFoodPerDayCalculator.tmpPawns, biome, faction, true, false, explanation);
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06006CF8 RID: 27896 RVA: 0x00262354 File Offset: 0x00260554
		public static Pair<ThingDef, float> ForagedFoodPerDayLeftAfterTransfer(List<TransferableOneWay> transferables, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = transferableOneWay.things.Count - 1; j >= transferableOneWay.CountToTransfer; j--)
					{
						ForagedFoodPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			Pair<ThingDef, float> result = ForagedFoodPerDayCalculator.ForagedFoodPerDay(ForagedFoodPerDayCalculator.tmpPawns, biome, faction, true, false, explanation);
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06006CF9 RID: 27897 RVA: 0x002623EA File Offset: 0x002605EA
		public static Pair<ThingDef, float> ForagedFoodPerDayLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			ForagedFoodPerDayCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, ForagedFoodPerDayCalculator.tmpThingCounts);
			Pair<ThingDef, float> result = ForagedFoodPerDayCalculator.ForagedFoodPerDay(ForagedFoodPerDayCalculator.tmpThingCounts, biome, faction, explanation);
			ForagedFoodPerDayCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06006CFA RID: 27898 RVA: 0x0026241C File Offset: 0x0026061C
		public static Pair<ThingDef, float> ForagedFoodPerDay(List<ThingCount> thingCounts, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < thingCounts.Count; i++)
			{
				if (thingCounts[i].Count > 0)
				{
					Pawn pawn = thingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						ForagedFoodPerDayCalculator.tmpPawns.Add(pawn);
					}
				}
			}
			Pair<ThingDef, float> result = ForagedFoodPerDayCalculator.ForagedFoodPerDay(ForagedFoodPerDayCalculator.tmpPawns, biome, faction, true, false, explanation);
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x040043BB RID: 17339
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x040043BC RID: 17340
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x040043BD RID: 17341
		private const float BaseProgressPerTick = 0.0001f;

		// Token: 0x040043BE RID: 17342
		public const float NotMovingProgressFactor = 2f;
	}
}
