﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	
	public class SketchResolver_AddInnerMonuments : SketchResolver
	{
		
		protected override void ResolveInt(ResolveParams parms)
		{
			CellRect outerRect = parms.rect ?? parms.sketch.OccupiedRect;
			HashSet<IntVec3> processed = new HashSet<IntVec3>();
			using (IEnumerator<IntVec3> enumerator = outerRect.Cells.InRandomOrder(null).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IntVec3 c = enumerator.Current;
					CellRect cellRect = SketchGenUtility.FindBiggestRectAt(c, outerRect, parms.sketch, processed, (IntVec3 x) => !parms.sketch.ThingsAt(x).Any<SketchThing>() && parms.sketch.AnyTerrainAt(c));
					if (cellRect.Width >= 7 && cellRect.Height >= 7)
					{
						int newX = Rand.RangeInclusive(5, cellRect.Width - 2);
						int newZ = Rand.RangeInclusive(5, cellRect.Height - 2);
						Sketch sketch = new Sketch();
						ResolveParams parms2 = parms;
						parms2.sketch = sketch;
						parms2.monumentSize = new IntVec2?(new IntVec2(newX, newZ));
						parms2.rect = null;
						SketchResolverDefOf.Monument.Resolve(parms2);
						parms.sketch.MergeAt(sketch, cellRect.CenterCell, Sketch.SpawnPosType.OccupiedCenter, false);
					}
				}
			}
		}

		
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		
		private const int MinRectWidth = 7;

		
		private const int MinRectHeight = 7;
	}
}
