using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A59 RID: 2649
	public class GenStep_ScatterShrines : GenStep_ScatterRuinsSimple
	{
		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x06003EA1 RID: 16033 RVA: 0x0014C2A7 File Offset: 0x0014A4A7
		public override int SeedPart
		{
			get
			{
				return 1801222485;
			}
		}

		// Token: 0x06003EA2 RID: 16034 RVA: 0x0014C2B0 File Offset: 0x0014A4B0
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.building.isNaturalRock;
		}

		// Token: 0x06003EA3 RID: 16035 RVA: 0x0014C2EC File Offset: 0x0014A4EC
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
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("ancientTemple", resolveParams, null);
			BaseGen.Generate();
		}

		// Token: 0x04002478 RID: 9336
		private static readonly IntRange SizeRange = new IntRange(15, 20);
	}
}
