using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE7 RID: 3559
	public class Alert_NeedWarmClothes : Alert
	{
		// Token: 0x06005647 RID: 22087 RVA: 0x001C97FF File Offset: 0x001C79FF
		public Alert_NeedWarmClothes()
		{
			this.defaultLabel = "NeedWarmClothes".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06005648 RID: 22088 RVA: 0x001C982E File Offset: 0x001C7A2E
		private int NeededWarmClothesCount(Map map)
		{
			return map.mapPawns.FreeColonistsSpawnedCount;
		}

		// Token: 0x06005649 RID: 22089 RVA: 0x001C983C File Offset: 0x001C7A3C
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

		// Token: 0x0600564A RID: 22090 RVA: 0x001C98AC File Offset: 0x001C7AAC
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

		// Token: 0x0600564B RID: 22091 RVA: 0x001C9924 File Offset: 0x001C7B24
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

		// Token: 0x0600564C RID: 22092 RVA: 0x001C9BF6 File Offset: 0x001C7DF6
		private int MissingWarmClothesCount(Map map)
		{
			if (this.LowestTemperatureComing(map) >= ThingDefOf.Human.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null))
			{
				return 0;
			}
			return Mathf.Max(this.NeededWarmClothesCount(map) - this.ColonistsWithWarmClothesCount(map) - this.FreeWarmClothesSetsCount(map), 0);
		}

		// Token: 0x0600564D RID: 22093 RVA: 0x001C9C30 File Offset: 0x001C7E30
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

		// Token: 0x0600564E RID: 22094 RVA: 0x001C9C80 File Offset: 0x001C7E80
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

		// Token: 0x0600564F RID: 22095 RVA: 0x001C9D30 File Offset: 0x001C7F30
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

		// Token: 0x06005650 RID: 22096 RVA: 0x001C9D74 File Offset: 0x001C7F74
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

		// Token: 0x06005651 RID: 22097 RVA: 0x001C9DC3 File Offset: 0x001C7FC3
		private float GetTemperature(Twelfth twelfth, Map map)
		{
			return GenTemperature.AverageTemperatureAtTileForTwelfth(map.Tile, twelfth);
		}

		// Token: 0x04002F20 RID: 12064
		private static List<Thing> jackets = new List<Thing>();

		// Token: 0x04002F21 RID: 12065
		private static List<Thing> shirts = new List<Thing>();

		// Token: 0x04002F22 RID: 12066
		private static List<Thing> pants = new List<Thing>();

		// Token: 0x04002F23 RID: 12067
		private const float MedicinePerColonistThreshold = 2f;

		// Token: 0x04002F24 RID: 12068
		private const int CheckNextTwelfthsCount = 3;

		// Token: 0x04002F25 RID: 12069
		private const float CanShowAlertOnlyIfTempBelow = 5f;

		// Token: 0x04002F26 RID: 12070
		private static Comparison<Thing> SortByInsulationCold = (Thing a, Thing b) => a.GetStatValue(StatDefOf.Insulation_Cold, true).CompareTo(b.GetStatValue(StatDefOf.Insulation_Cold, true));

		// Token: 0x04002F27 RID: 12071
		private List<Pawn> colonistsWithoutWarmClothes = new List<Pawn>();
	}
}
