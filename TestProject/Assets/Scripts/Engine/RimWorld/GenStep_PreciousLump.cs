using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class GenStep_PreciousLump : GenStep_ScatterLumpsMineable
	{
		
		// (get) Token: 0x06003EF0 RID: 16112 RVA: 0x0014EBC5 File Offset: 0x0014CDC5
		public override int SeedPart
		{
			get
			{
				return 1634184421;
			}
		}

		
		public override void Generate(Map map, GenStepParams parms)
		{
			if (parms.sitePart != null && parms.sitePart.parms.preciousLumpResources != null)
			{
				this.forcedDefToScatter = parms.sitePart.parms.preciousLumpResources;
			}
			else
			{
				this.forcedDefToScatter = this.mineables.RandomElement<ThingDef>();
			}
			this.count = 1;
			float randomInRange = this.totalValueRange.RandomInRange;
			float baseMarketValue = this.forcedDefToScatter.building.mineableThing.BaseMarketValue;
			this.forcedLumpSize = Mathf.Max(Mathf.RoundToInt(randomInRange / ((float)this.forcedDefToScatter.building.mineableYield * baseMarketValue)), 1);
			base.Generate(map, parms);
		}

		
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			List<CellRect> list;
			return (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list) || !list.Any((CellRect x) => x.Contains(c))) && map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false));
		}

		
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			base.ScatterAt(c, map, parms, stackCount);
			int minX = this.recentLumpCells.Min((IntVec3 x) => x.x);
			int minZ = this.recentLumpCells.Min((IntVec3 x) => x.z);
			int maxX = this.recentLumpCells.Max((IntVec3 x) => x.x);
			int maxZ = this.recentLumpCells.Max((IntVec3 x) => x.z);
			CellRect var = CellRect.FromLimits(minX, minZ, maxX, maxZ);
			MapGenerator.SetVar<CellRect>("RectOfInterest", var);
		}

		
		public List<ThingDef> mineables;

		
		public FloatRange totalValueRange = new FloatRange(1000f, 2000f);
	}
}
