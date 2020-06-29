using System;
using Verse;

namespace RimWorld
{
	
	public abstract class SketchBuildable : SketchEntity
	{
		
		// (get) Token: 0x06004058 RID: 16472
		public abstract BuildableDef Buildable { get; }

		
		// (get) Token: 0x06004059 RID: 16473
		public abstract ThingDef Stuff { get; }

		
		// (get) Token: 0x0600405A RID: 16474 RVA: 0x00158399 File Offset: 0x00156599
		public override string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.Buildable, this.Stuff, 1);
			}
		}

		
		// (get) Token: 0x0600405B RID: 16475 RVA: 0x001583AD File Offset: 0x001565AD
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
