using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	
	public class GenStep_ScatterShrines : GenStep_ScatterRuinsSimple
	{
		
		
		public override int SeedPart
		{
			get
			{
				return 1801222485;
			}
		}

		
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.building.isNaturalRock;
		}

		
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int stackCount = 1)
		{
			int randomInRange = GenStep_ScatterShrines.SizeRange.RandomInRange;
			int randomInRange2 = GenStep_ScatterShrines.SizeRange.RandomInRange;
			CellRect rect = new CellRect(loc.x, loc.z, randomInRange, randomInRange2);
			rect.ClipInsideMap(map);
			if (rect.Width != randomInRange || rect.Height != randomInRange2)
			{
				return;
			}
			foreach (IntVec3 c in rect.Cells)
			{
				List<Thing> list = map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def == ThingDefOf.AncientCryptosleepCasket)
					{
						return;
					}
				}
			}
			if (!base.CanPlaceAncientBuildingInRange(rect, map))
			{
				return;
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = rect;
			resolveParams.disableSinglePawn = new bool?(true);
			resolveParams.disableHives = new bool?(true);
			resolveParams.makeWarningLetter = new bool?(true);
			BaseGenCore.globalSettings.map = map;
			BaseGenCore.symbolStack.Push("ancientTemple", resolveParams, null);
			BaseGenCore.Generate();
		}

		
		private static readonly IntRange SizeRange = new IntRange(15, 20);
	}
}
