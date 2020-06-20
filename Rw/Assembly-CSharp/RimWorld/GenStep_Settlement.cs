using System;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A60 RID: 2656
	public class GenStep_Settlement : GenStep_Scatterer
	{
		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x06003EC1 RID: 16065 RVA: 0x0014DA98 File Offset: 0x0014BC98
		public override int SeedPart
		{
			get
			{
				return 1806208471;
			}
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x0014DAA0 File Offset: 0x0014BCA0
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.Standable(map))
			{
				return false;
			}
			if (c.Roofed(map))
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
			{
				return false;
			}
			int min = GenStep_Settlement.SettlementSizeRange.min;
			CellRect cellRect = new CellRect(c.x - min / 2, c.z - min / 2, min, min);
			return cellRect.FullyContainedWithin(new CellRect(0, 0, map.Size.x, map.Size.z));
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x0014DB38 File Offset: 0x0014BD38
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			int randomInRange = GenStep_Settlement.SettlementSizeRange.RandomInRange;
			int randomInRange2 = GenStep_Settlement.SettlementSizeRange.RandomInRange;
			CellRect rect = new CellRect(c.x - randomInRange / 2, c.z - randomInRange2 / 2, randomInRange, randomInRange2);
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				faction = map.ParentFaction;
			}
			rect.ClipInsideMap(map);
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = rect;
			resolveParams.faction = faction;
			BaseGen.globalSettings.map = map;
			BaseGen.globalSettings.minBuildings = 1;
			BaseGen.globalSettings.minBarracks = 1;
			BaseGen.symbolStack.Push("settlement", resolveParams, null);
			BaseGen.Generate();
		}

		// Token: 0x04002492 RID: 9362
		private static readonly IntRange SettlementSizeRange = new IntRange(34, 38);
	}
}
