using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AA5 RID: 2725
	public abstract class SketchEntity : IExposable
	{
		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x0600405F RID: 16479
		public abstract string Label { get; }

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06004060 RID: 16480
		public abstract CellRect OccupiedRect { get; }

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x06004061 RID: 16481
		public abstract float SpawnOrder { get; }

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x06004062 RID: 16482 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool LostImportantReferences
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004063 RID: 16483
		public abstract void DrawGhost(IntVec3 at, Color color);

		// Token: 0x06004064 RID: 16484
		public abstract bool IsSameSpawned(IntVec3 at, Map map);

		// Token: 0x06004065 RID: 16485
		public abstract bool IsSameSpawnedOrBlueprintOrFrame(IntVec3 at, Map map);

		// Token: 0x06004066 RID: 16486
		public abstract bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false);

		// Token: 0x06004067 RID: 16487
		public abstract bool IsSpawningBlockedPermanently(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false);

		// Token: 0x06004068 RID: 16488
		public abstract bool CanBuildOnTerrain(IntVec3 at, Map map);

		// Token: 0x06004069 RID: 16489
		public abstract bool Spawn(IntVec3 at, Map map, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, List<Thing> spawnedThings = null, bool dormant = false);

		// Token: 0x0600406A RID: 16490
		public abstract bool SameForSubtracting(SketchEntity other);

		// Token: 0x0600406B RID: 16491 RVA: 0x001583E4 File Offset: 0x001565E4
		public bool SpawnNear(IntVec3 near, Map map, float radius, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, List<Thing> spawnedThings = null, bool dormant = false)
		{
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = near + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && this.Spawn(intVec, map, faction, spawnMode, wipeIfCollides, spawnedThings, dormant))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x00158436 File Offset: 0x00156636
		public virtual SketchEntity DeepCopy()
		{
			SketchEntity sketchEntity = (SketchEntity)Activator.CreateInstance(base.GetType());
			sketchEntity.pos = this.pos;
			return sketchEntity;
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x00158454 File Offset: 0x00156654
		public virtual void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.pos, "pos", default(IntVec3), false);
		}

		// Token: 0x04002568 RID: 9576
		public IntVec3 pos;
	}
}
