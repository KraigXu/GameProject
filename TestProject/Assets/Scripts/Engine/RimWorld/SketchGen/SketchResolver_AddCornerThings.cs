using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	
	public class SketchResolver_AddCornerThings : SketchResolver
	{
		
		protected override void ResolveInt(ResolveParams parms)
		{
			this.wallPositions.Clear();
			for (int i = 0; i < parms.sketch.Things.Count; i++)
			{
				if (parms.sketch.Things[i].def == ThingDefOf.Wall)
				{
					this.wallPositions.Add(parms.sketch.Things[i].pos);
				}
			}
			bool allowWood = parms.allowWood ?? true;
			ThingDef stuff = GenStuff.RandomStuffInexpensiveFor(parms.cornerThing, null, (ThingDef x) => SketchGenUtility.IsStuffAllowed(x, allowWood, parms.useOnlyStonesAvailableOnMap, true, parms.cornerThing));
			bool flag = parms.requireFloor ?? false;
			try
			{
				foreach (IntVec3 intVec in this.wallPositions)
				{
					if (Rand.Chance(0.09f))
					{
						if (this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z - 1)) && !this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z)) && (!flag || (parms.sketch.TerrainAt(new IntVec3(intVec.x + 1, 0, intVec.z)) != null && parms.sketch.TerrainAt(new IntVec3(intVec.x + 1, 0, intVec.z)).layerable)))
						{
							parms.sketch.AddThing(parms.cornerThing, new IntVec3(intVec.x + 1, 0, intVec.z), Rot4.North, stuff, 1, null, null, false);
						}
						if (this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z - 1)) && !this.wallPositions.Contains(new IntVec3(intVec.x, 0, intVec.z - 1)) && (!flag || (parms.sketch.TerrainAt(new IntVec3(intVec.x, 0, intVec.z - 1)) != null && parms.sketch.TerrainAt(new IntVec3(intVec.x, 0, intVec.z - 1)).layerable)))
						{
							parms.sketch.AddThing(parms.cornerThing, new IntVec3(intVec.x, 0, intVec.z - 1), Rot4.North, stuff, 1, null, null, false);
						}
						if (this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z + 1)) && !this.wallPositions.Contains(new IntVec3(intVec.x, 0, intVec.z + 1)) && (!flag || (parms.sketch.TerrainAt(new IntVec3(intVec.x, 0, intVec.z + 1)) != null && parms.sketch.TerrainAt(new IntVec3(intVec.x, 0, intVec.z + 1)).layerable)))
						{
							parms.sketch.AddThing(parms.cornerThing, new IntVec3(intVec.x, 0, intVec.z + 1), Rot4.North, stuff, 1, null, null, false);
						}
						if (this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z + 1)) && !this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z)) && (!flag || (parms.sketch.TerrainAt(new IntVec3(intVec.x + 1, 0, intVec.z)) != null && parms.sketch.TerrainAt(new IntVec3(intVec.x + 1, 0, intVec.z)).layerable)))
						{
							parms.sketch.AddThing(parms.cornerThing, new IntVec3(intVec.x + 1, 0, intVec.z), Rot4.North, stuff, 1, null, null, false);
						}
					}
				}
			}
			finally
			{
				this.wallPositions.Clear();
			}
		}

		
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		
		private HashSet<IntVec3> wallPositions = new HashSet<IntVec3>();

		
		private const float Chance = 0.09f;
	}
}
