using System;
using Verse;

namespace RimWorld
{
	
	public abstract class SketchBuildable : SketchEntity
	{
		
		
		public abstract BuildableDef Buildable { get; }

		
		
		public abstract ThingDef Stuff { get; }

		
		
		public override string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.Buildable, this.Stuff, 1);
			}
		}

		
		
		public override bool LostImportantReferences
		{
			get
			{
				return this.Buildable == null;
			}
		}

		
		public abstract Thing GetSpawnedBlueprintOrFrame(IntVec3 at, Map map);

		
		public override bool IsSameSpawnedOrBlueprintOrFrame(IntVec3 at, Map map)
		{
			return at.InBounds(map) && (this.IsSameSpawned(at, map) || this.GetSpawnedBlueprintOrFrame(at, map) != null);
		}
	}
}
