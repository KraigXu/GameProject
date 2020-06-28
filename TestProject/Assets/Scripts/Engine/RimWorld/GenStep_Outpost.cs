using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A6C RID: 2668
	public class GenStep_Outpost : GenStep
	{
		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x06003EEB RID: 16107 RVA: 0x0014E7AA File Offset: 0x0014C9AA
		public override int SeedPart
		{
			get
			{
				return 398638181;
			}
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x0014E7B4 File Offset: 0x0014C9B4
		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect rectToDefend;
			if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out rectToDefend))
			{
				rectToDefend = CellRect.SingleCell(map.Center);
			}
			List<CellRect> list;
			if (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list))
			{
				list = new List<CellRect>();
				MapGenerator.SetVar<List<CellRect>>("UsedRects", list);
			}
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				faction = map.ParentFaction;
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = this.GetOutpostRect(rectToDefend, list, map);
			resolveParams.faction = faction;
			resolveParams.edgeDefenseWidth = new int?(2);
			resolveParams.edgeDefenseTurretsCount = new int?(Rand.RangeInclusive(0, 1));
			resolveParams.edgeDefenseMortarsCount = new int?(0);
			if (parms.sitePart != null)
			{
				resolveParams.settlementPawnGroupPoints = new float?(parms.sitePart.parms.threatPoints);
				resolveParams.settlementPawnGroupSeed = new int?(OutpostSitePartUtility.GetPawnGroupMakerSeed(parms.sitePart.parms));
			}
			else
			{
				resolveParams.settlementPawnGroupPoints = new float?(this.defaultPawnGroupPointsRange.RandomInRange);
			}
			BaseGen.globalSettings.map = map;
			BaseGen.globalSettings.minBuildings = 1;
			BaseGen.globalSettings.minBarracks = 1;
			BaseGen.symbolStack.Push("settlement", resolveParams, null);
			BaseGen.Generate();
			list.Add(resolveParams.rect);
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x0014E910 File Offset: 0x0014CB10
		private CellRect GetOutpostRect(CellRect rectToDefend, List<CellRect> usedRects, Map map)
		{
			GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.minX - 1 - this.size, rectToDefend.CenterCell.z - this.size / 2, this.size, this.size));
			GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.maxX + 1, rectToDefend.CenterCell.z - this.size / 2, this.size, this.size));
			GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - this.size / 2, rectToDefend.minZ - 1 - this.size, this.size, this.size));
			GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - this.size / 2, rectToDefend.maxZ + 1, this.size, this.size));
			CellRect mapRect = new CellRect(0, 0, map.Size.x, map.Size.z);
			GenStep_Outpost.possibleRects.RemoveAll((CellRect x) => !x.FullyContainedWithin(mapRect));
			if (!GenStep_Outpost.possibleRects.Any<CellRect>())
			{
				return rectToDefend;
			}
			IEnumerable<CellRect> source = from x in GenStep_Outpost.possibleRects
			where !usedRects.Any((CellRect y) => x.Overlaps(y))
			select x;
			if (!source.Any<CellRect>())
			{
				GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.minX - 1 - this.size * 2, rectToDefend.CenterCell.z - this.size / 2, this.size, this.size));
				GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.maxX + 1 + this.size, rectToDefend.CenterCell.z - this.size / 2, this.size, this.size));
				GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - this.size / 2, rectToDefend.minZ - 1 - this.size * 2, this.size, this.size));
				GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - this.size / 2, rectToDefend.maxZ + 1 + this.size, this.size, this.size));
			}
			if (source.Any<CellRect>())
			{
				return source.RandomElement<CellRect>();
			}
			return GenStep_Outpost.possibleRects.RandomElement<CellRect>();
		}

		// Token: 0x0400249F RID: 9375
		public int size = 16;

		// Token: 0x040024A0 RID: 9376
		public FloatRange defaultPawnGroupPointsRange = SymbolResolver_Settlement.DefaultPawnsPoints;

		// Token: 0x040024A1 RID: 9377
		private static List<CellRect> possibleRects = new List<CellRect>();
	}
}
