﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Alert_NeedWarmClothes : Alert
	{
		
		public Alert_NeedWarmClothes()
		{
			this.defaultLabel = "NeedWarmClothes".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		
		private int NeededWarmClothesCount(Map map)
		{
			return map.mapPawns.FreeColonistsSpawnedCount;
		}

		
		private int ColonistsWithWarmClothesCount(Map map)
		{
			float num = this.LowestTemperatureComing(map);
			int num2 = 0;
			using (List<Pawn>.Enumerator enumerator = map.mapPawns.FreeColonistsSpawned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetStatValue(StatDefOf.ComfyTemperatureMin, true) <= num)
					{
						num2++;
					}
				}
			}
			return num2;
		}

		
		private void GetColonistsWithoutWarmClothes(Map map, List<Pawn> outResult)
		{
			outResult.Clear();
			float num = this.LowestTemperatureComing(map);
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (pawn.GetStatValue(StatDefOf.ComfyTemperatureMin, true) > num)
				{
					outResult.Add(pawn);
				}
			}
		}

		
		private int FreeWarmClothesSetsCount(Map map)
		{
			Alert_NeedWarmClothes.jackets.Clear();
			Alert_NeedWarmClothes.shirts.Clear();
			Alert_NeedWarmClothes.pants.Clear();
			List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.Apparel);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsInAnyStorage() && list[i].GetStatValue(StatDefOf.Insulation_Cold, true) > 0f)
				{
					if (list[i].def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
					{
						if (list[i].def.apparel.layers.Contains(ApparelLayerDefOf.OnSkin))
						{
							Alert_NeedWarmClothes.shirts.Add(list[i]);
						}
						else
						{
							Alert_NeedWarmClothes.jackets.Add(list[i]);
						}
					}
					if (list[i].def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs))
					{
						Alert_NeedWarmClothes.pants.Add(list[i]);
					}
				}
			}
			Alert_NeedWarmClothes.jackets.Sort(Alert_NeedWarmClothes.SortByInsulationCold);
			Alert_NeedWarmClothes.shirts.Sort(Alert_NeedWarmClothes.SortByInsulationCold);
			Alert_NeedWarmClothes.pants.Sort(Alert_NeedWarmClothes.SortByInsulationCold);
			float num = ThingDefOf.Human.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - this.LowestTemperatureComing(map);
			if (num <= 0f)
			{
				return GenMath.Max(Alert_NeedWarmClothes.jackets.Count, Alert_NeedWarmClothes.shirts.Count, Alert_NeedWarmClothes.pants.Count);
			}
			int num2 = 0;
			while (Alert_NeedWarmClothes.jackets.Any<Thing>() || Alert_NeedWarmClothes.shirts.Any<Thing>() || Alert_NeedWarmClothes.pants.Any<Thing>())
			{
				float num3 = 0f;
				if (Alert_NeedWarmClothes.jackets.Any<Thing>())
				{
					Thing thing = Alert_NeedWarmClothes.jackets[Alert_NeedWarmClothes.jackets.Count - 1];
					Alert_NeedWarmClothes.jackets.RemoveLast<Thing>();
					num3 += thing.GetStatValue(StatDefOf.Insulation_Cold, true);
				}
				if (num3 < num && Alert_NeedWarmClothes.shirts.Any<Thing>())
				{
					Thing thing2 = Alert_NeedWarmClothes.shirts[Alert_NeedWarmClothes.shirts.Count - 1];
					Alert_NeedWarmClothes.shirts.RemoveLast<Thing>();
					num3 += thing2.GetStatValue(StatDefOf.Insulation_Cold, true);
				}
				if (num3 < num && Alert_NeedWarmClothes.pants.Any<Thing>())
				{
					for (int j = 0; j < Alert_NeedWarmClothes.pants.Count; j++)
					{
						float statValue = Alert_NeedWarmClothes.pants[j].GetStatValue(StatDefOf.Insulation_Cold, true);
						if (statValue + num3 >= num)
						{
							num3 += statValue;
							Alert_NeedWarmClothes.pants.RemoveAt(j);
							break;
						}
					}
				}
				if (num3 < num)
				{
					break;
				}
				num2++;
			}
			Alert_NeedWarmClothes.jackets.Clear();
			Alert_NeedWarmClothes.shirts.Clear();
			Alert_NeedWarmClothes.pants.Clear();
			return num2;
		}

		
		private int MissingWarmClothesCount(Map map)
		{
			if (this.LowestTemperatureComing(map) >= ThingDefOf.Human.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null))
			{
				return 0;
			}
			return Mathf.Max(this.NeededWarmClothesCount(map) - this.ColonistsWithWarmClothesCount(map) - this.FreeWarmClothesSetsCount(map), 0);
		}

		
		private float LowestTemperatureComing(Map map)
		{
			Twelfth twelfth = GenLocalDate.Twelfth(map);
			float a = this.GetTemperature(twelfth, map);
			for (int i = 0; i < 3; i++)
			{
				twelfth = twelfth.NextTwelfth();
				a = Mathf.Min(a, this.GetTemperature(twelfth, map));
			}
			return Mathf.Min(a, map.mapTemperature.OutdoorTemp);
		}

		
		public override TaggedString GetExplanation()
		{
			Map map = this.MapWithMissingWarmClothes();
			if (map == null)
			{
				return "";
			}
			int num = this.MissingWarmClothesCount(map);
			if (num == this.NeededWarmClothesCount(map))
			{
				return "NeedWarmClothesDesc1All".Translate() + "\n\n" + "NeedWarmClothesDesc2".Translate(this.LowestTemperatureComing(map).ToStringTemperature("F0"));
			}
			return "NeedWarmClothesDesc1".Translate(num) + "\n\n" + "NeedWarmClothesDesc2".Translate(this.LowestTemperatureComing(map).ToStringTemperature("F0"));
		}

		
		public override AlertReport GetReport()
		{
			Map map = this.MapWithMissingWarmClothes();
			if (map == null)
			{
				return false;
			}
			this.colonistsWithoutWarmClothes.Clear();
			this.GetColonistsWithoutWarmClothes(map, this.colonistsWithoutWarmClothes);
			return AlertReport.CulpritsAre(this.colonistsWithoutWarmClothes);
		}

		
		private Map MapWithMissingWarmClothes()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome && this.LowestTemperatureComing(map) < 5f && this.MissingWarmClothesCount(map) > 0)
				{
					return map;
				}
			}
			return null;
		}

		
		private float GetTemperature(Twelfth twelfth, Map map)
		{
			return GenTemperature.AverageTemperatureAtTileForTwelfth(map.Tile, twelfth);
		}

		
		private static List<Thing> jackets = new List<Thing>();

		
		private static List<Thing> shirts = new List<Thing>();

		
		private static List<Thing> pants = new List<Thing>();

		
		private const float MedicinePerColonistThreshold = 2f;

		
		private const int CheckNextTwelfthsCount = 3;

		
		private const float CanShowAlertOnlyIfTempBelow = 5f;

		
		private static Comparison<Thing> SortByInsulationCold = (Thing a, Thing b) => a.GetStatValue(StatDefOf.Insulation_Cold, true).CompareTo(b.GetStatValue(StatDefOf.Insulation_Cold, true));

		
		private List<Pawn> colonistsWithoutWarmClothes = new List<Pawn>();
	}
}
