﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	
	public class SketchResolver_AddColumns : SketchResolver
	{
		
		protected override void ResolveInt(ResolveParams parms)
		{
			CellRect cellRect = parms.rect ?? parms.sketch.OccupiedRect;
			bool allowWood = parms.allowWood ?? true;
			bool flag = parms.requireFloor ?? false;
			this.rects.Clear();
			this.processed.Clear();
			Predicate<IntVec3> 9__1;
			foreach (IntVec3 c in cellRect.Cells.InRandomOrder(null))
			{
				CellRect outerRect = cellRect;
				Sketch sketch = parms.sketch;
				HashSet<IntVec3> hashSet = this.processed;
				Predicate<IntVec3> canTraverse;
				if ((canTraverse ) == null)
				{
					canTraverse = (9__1 = ((IntVec3 x) => !this.AnyColumnBlockerAt(x, parms.sketch)));
				}
				CellRect item = SketchGenUtility.FindBiggestRectAt(c, outerRect, sketch, hashSet, canTraverse);
				if (!item.IsEmpty)
				{
					this.rects.Add(item);
				}
			}
			ThingDef stuff = GenStuff.RandomStuffInexpensiveFor(ThingDefOf.Column, null, (ThingDef x) => SketchGenUtility.IsStuffAllowed(x, allowWood, parms.useOnlyStonesAvailableOnMap, true, ThingDefOf.Column));
			for (int i = 0; i < this.rects.Count; i++)
			{
				if (this.rects[i].Width >= 3 && this.rects[i].Height >= 3 && Rand.Chance(0.8f))
				{
					CellRect cellRect2 = this.rects[i].ContractedBy(1);
					Sketch sketch2 = new Sketch();
					if (Rand.Bool)
					{
						int newZ = Rand.RangeInclusive(cellRect2.minZ, cellRect2.CenterCell.z);
						int num = (cellRect2.Width >= 4) ? Rand.Element<int>(2, 3) : 2;
						for (int j = cellRect2.minX; j <= cellRect2.maxX; j += num)
						{
							if (!flag || parms.sketch.AnyTerrainAt(new IntVec3(j, 0, newZ)))
							{
								sketch2.AddThing(ThingDefOf.Column, new IntVec3(j, 0, newZ), Rot4.North, stuff, 1, null, null, true);
							}
						}
						ResolveParams parms2 = parms;
						parms2.sketch = sketch2;
						parms2.symmetryOrigin = new int?(this.rects[i].minZ + this.rects[i].Height / 2);
						parms2.symmetryOriginIncluded = new bool?(this.rects[i].Height % 2 == 1);
						SketchResolverDefOf.Symmetry.Resolve(parms2);
					}
					else
					{
						int newX = Rand.RangeInclusive(cellRect2.minX, cellRect2.CenterCell.x);
						int num2 = (cellRect2.Height >= 4) ? Rand.Element<int>(2, 3) : 2;
						for (int k = cellRect2.minZ; k <= cellRect2.maxZ; k += num2)
						{
							if (!flag || parms.sketch.AnyTerrainAt(new IntVec3(newX, 0, k)))
							{
								sketch2.AddThing(ThingDefOf.Column, new IntVec3(newX, 0, k), Rot4.North, stuff, 1, null, null, true);
							}
						}
						ResolveParams parms3 = parms;
						parms3.sketch = sketch2;
						parms3.symmetryOrigin = new int?(this.rects[i].minX + this.rects[i].Width / 2);
						parms3.symmetryOriginIncluded = new bool?(this.rects[i].Width % 2 == 1);
						SketchResolverDefOf.Symmetry.Resolve(parms3);
					}
					parms.sketch.Merge(sketch2, false);
				}
			}
			this.rects.Clear();
			this.processed.Clear();
		}

		
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		
		private bool AnyColumnBlockerAt(IntVec3 c, Sketch sketch)
		{
			return sketch.ThingsAt(c).Any((SketchThing x) => x.def.passability == Traversability.Impassable);
		}

		
		private List<CellRect> rects = new List<CellRect>();

		
		private HashSet<IntVec3> processed = new HashSet<IntVec3>();

		
		private const float Chance = 0.8f;
	}
}
