using System;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_EdgeSandbags : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGenCore.globalSettings.map;
			int num = 0;
			int num2 = 0;
			int num3 = -1;
			if (rp.rect.EdgeCellsCount < (SymbolResolver_EdgeSandbags.LineLengthRange.max + SymbolResolver_EdgeSandbags.GapLengthRange.max) * 2)
			{
				num = rp.rect.EdgeCellsCount;
			}
			else if (Rand.Bool)
			{
				num = SymbolResolver_EdgeSandbags.LineLengthRange.RandomInRange;
			}
			else
			{
				num2 = SymbolResolver_EdgeSandbags.GapLengthRange.RandomInRange;
			}
			foreach (IntVec3 intVec in rp.rect.EdgeCells)
			{
				num3++;
				if (num2 > 0)
				{
					num2--;
					if (num2 == 0)
					{
						if (num3 == rp.rect.EdgeCellsCount - 2)
						{
							num2 = 1;
						}
						else
						{
							num = SymbolResolver_EdgeSandbags.LineLengthRange.RandomInRange;
						}
					}
				}
				else if (intVec.Standable(map) && !intVec.Roofed(map) && intVec.SupportsStructureType(map, ThingDefOf.Sandbags.terrainAffordanceNeeded))
				{
					if (!GenSpawn.WouldWipeAnythingWith(intVec, Rot4.North, ThingDefOf.Sandbags, map, (Thing x) => x.def.category == ThingCategory.Building || x.def.category == ThingCategory.Item))
					{
						if (num > 0)
						{
							num--;
							if (num == 0)
							{
								num2 = SymbolResolver_EdgeSandbags.GapLengthRange.RandomInRange;
							}
						}
						Thing thing = ThingMaker.MakeThing(ThingDefOf.Sandbags, ThingDefOf.Cloth);
						thing.SetFaction(rp.faction, null);
						GenSpawn.Spawn(thing, intVec, map, WipeMode.Vanish);
					}
				}
			}
		}

		
		private static readonly IntRange LineLengthRange = new IntRange(2, 5);

		
		private static readonly IntRange GapLengthRange = new IntRange(1, 5);
	}
}
