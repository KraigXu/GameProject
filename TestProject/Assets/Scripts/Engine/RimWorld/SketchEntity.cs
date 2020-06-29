using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class SketchEntity : IExposable
	{
		
		// (get) Token: 0x0600405F RID: 16479
		public abstract string Label { get; }

		
		// (get) Token: 0x06004060 RID: 16480
		public abstract CellRect OccupiedRect { get; }

		
		// (get) Token: 0x06004061 RID: 16481
		public abstract float SpawnOrder { get; }

		
		// (get) Token: 0x06004062 RID: 16482 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool LostImportantReferences
		{
			get
			{
				return false;
			}
		}

		
		public abstract void DrawGhost(IntVec3 at, Color color);

		
		public abstract bool IsSameSpawned(IntVec3 at, Map map);

		
		public abstract bool IsSameSpawnedOrBlueprintOrFrame(IntVec3 at, Map map);

		
		public abstract bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false);

		
		public abstract bool IsSpawningBlockedPermanently(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false);

		
		public abstract bool CanBuildOnTerrain(IntVec3 at, Map map);

		
		public abstract bool Spawn(IntVec3 at, Map map, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, List<Thing> spawnedThings = null, bool dormant = false);

		
		public abstract bool SameForSubtracting(SketchEntity other);

		
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

		
		public virtual SketchEntity DeepCopy()
		{
			SketchEntity sketchEntity = (SketchEntity)Activator.CreateInstance(base.GetType());
			sketchEntity.pos = this.pos;
			return sketchEntity;
		}

		
		public virtual void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.pos, "pos", default(IntVec3), false);
		}

		
		public IntVec3 pos;
	}
}
