using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A70 RID: 2672
	public class GenStep_Turrets : GenStep
	{
		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06003EFE RID: 16126 RVA: 0x0014F251 File Offset: 0x0014D451
		public override int SeedPart
		{
			get
			{
				return 895502705;
			}
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x0014F258 File Offset: 0x0014D458
		public override void Generate(Map map, GenStepParams parms)
		{
			int num = 0;
			CellRect cellRect;
			if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out cellRect))
			{
				cellRect = this.FindRandomRectToDefend(map);
			}
			else
			{
				int num2;
				if (!MapGenerator.TryGetVar<int>("RectOfInterestTurretsGenStepsCount", out num2))
				{
					num2 = 0;
				}
				num += num2 * 4;
				num2++;
				MapGenerator.SetVar<int>("RectOfInterestTurretsGenStepsCount", num2);
			}
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = (from x in Find.FactionManager.AllFactions
				where !x.defeated && x.HostileTo(Faction.OfPlayer) && !x.def.hidden && x.def.techLevel >= TechLevel.Industrial
				select x).RandomElementWithFallback(Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined));
			}
			else
			{
				faction = map.ParentFaction;
			}
			int randomInRange = this.widthRange.RandomInRange;
			CellRect rect = cellRect.ExpandedBy(7 + randomInRange + num).ClipInsideMap(map);
			int value;
			int value2;
			if (parms.sitePart != null)
			{
				value = parms.sitePart.parms.turretsCount;
				value2 = parms.sitePart.parms.mortarsCount;
			}
			else
			{
				value = this.defaultTurretsCountRange.RandomInRange;
				value2 = this.defaultMortarsCountRange.RandomInRange;
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = rect;
			resolveParams.faction = faction;
			resolveParams.edgeDefenseWidth = new int?(randomInRange);
			resolveParams.edgeDefenseTurretsCount = new int?(value);
			resolveParams.edgeDefenseMortarsCount = new int?(value2);
			resolveParams.edgeDefenseGuardsCount = new int?(this.guardsCountRange.RandomInRange);
			resolveParams.edgeThingMustReachMapEdge = new bool?(true);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("edgeDefense", resolveParams, null);
			BaseGen.Generate();
			ResolveParams resolveParams2 = default(ResolveParams);
			resolveParams2.rect = rect;
			resolveParams2.faction = faction;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("outdoorLighting", resolveParams2, null);
			BaseGen.Generate();
		}

		// Token: 0x06003F00 RID: 16128 RVA: 0x0014F438 File Offset: 0x0014D638
		private CellRect FindRandomRectToDefend(Map map)
		{
			List<CellRect> usedRects;
			if (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out usedRects))
			{
				usedRects = null;
			}
			int rectRadius = Mathf.Max(Mathf.RoundToInt((float)Mathf.Min(map.Size.x, map.Size.z) * 0.07f), 1);
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
			IntVec3 center;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(delegate(IntVec3 x)
			{
				if (!map.reachability.CanReachMapEdge(x, traverseParams))
				{
					return false;
				}
				CellRect cellRect = CellRect.CenteredOn(x, rectRadius);
				int num = 0;
				using (CellRect.Enumerator enumerator = cellRect.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 c = enumerator.Current;
						if (!c.InBounds(map))
						{
							return false;
						}
						if (usedRects != null && cellRect.IsOnEdge(c) && usedRects.Any((CellRect y) => y.Contains(c)))
						{
							return false;
						}
						if (c.Standable(map) || c.GetPlant(map) != null)
						{
							num++;
						}
					}
				}
				return (float)num / (float)cellRect.Area >= 0.6f;
			}, map, out center))
			{
				return CellRect.CenteredOn(center, rectRadius);
			}
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map), map, out center))
			{
				return CellRect.CenteredOn(center, rectRadius);
			}
			return CellRect.CenteredOn(CellFinder.RandomCell(map), rectRadius).ClipInsideMap(map);
		}

		// Token: 0x040024A6 RID: 9382
		public IntRange defaultTurretsCountRange = new IntRange(4, 5);

		// Token: 0x040024A7 RID: 9383
		public IntRange defaultMortarsCountRange = new IntRange(0, 1);

		// Token: 0x040024A8 RID: 9384
		public IntRange widthRange = new IntRange(3, 4);

		// Token: 0x040024A9 RID: 9385
		public IntRange guardsCountRange = new IntRange(1, 1);

		// Token: 0x040024AA RID: 9386
		private const int Padding = 7;

		// Token: 0x040024AB RID: 9387
		public const int DefaultGuardsCount = 1;
	}
}
