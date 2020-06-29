using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_FloorFill : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGenCore.globalSettings.map;
			TerrainGrid terrainGrid = map.terrainGrid;
			TerrainDef terrainDef = rp.floorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false);
			bool flag = rp.floorOnlyIfTerrainSupports ?? false;
			bool flag2 = rp.allowBridgeOnAnyImpassableTerrain ?? false;
			foreach (IntVec3 c in rp.rect)
			{
				if ((rp.chanceToSkipFloor == null || !Rand.Chance(rp.chanceToSkipFloor.Value)) && (!flag || GenConstruct.CanBuildOnTerrain(terrainDef, c, map, Rot4.North, null, null) || (flag2 && c.GetTerrain(map).passability == Traversability.Impassable)))
				{
					terrainGrid.SetTerrain(c, terrainDef);
					if (rp.filthDef != null)
					{
						FilthMaker.TryMakeFilth(c, map, rp.filthDef, (rp.filthDensity != null) ? Mathf.RoundToInt(rp.filthDensity.Value.RandomInRange) : 1, FilthSourceFlags.None);
					}
				}
			}
		}
	}
}
